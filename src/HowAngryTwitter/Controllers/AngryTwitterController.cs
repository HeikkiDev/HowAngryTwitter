using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Newtonsoft.Json.Linq;

/*
 * 
 * Esta nueva clase Controller es accesible en la Web poniendo /AngryTwitter para el Index() y /AngryTwitter/Welcome para el Welcome()
 * La ruta root de la Web sigue utilizando el Controller HomeController porque es lo que está definido en la clase Startup.cs (al final):
 * app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
 * 
 */

namespace HowAngryTwitter.Controllers
{
    public class AngryTwitterController : Controller
    {
        // GET: /AngryTwitter/

        public IActionResult Index()
        {
            return View();
        }

        // GET: /AngryTwitter/
        [HttpPost]
        public ActionResult Index(string username)
        {
            // Redirijo a la vista de resultados pasando como parámetro al usuario de Twitter!
            return RedirectToAction("TwitterResult", new { user = username });  // redirecting to another view
        }

        // GET: /AngryTwitter/Welcome/ 

        public IActionResult Welcome(string name)
        {
            ViewData["Name"] = name;

            return View();
        }

        // GET: /AngryTwitter/About

        public IActionResult About()
        {
            return View();
        }

        // GET: /AngryTwitter/Contact

        public IActionResult Contact()
        {
            return View();
        }

        // GET: /AngryTwitter/TwitterResult

        public async Task<ActionResult> TwitterResult(string user)
        {
            // Aquí se hace el trabajo, antes de devolver la Vista con el % de Angry Tweets

            ViewBag.SyncOrAsync = "Asynchronous";
            var _service = new Services.NetworkServices();

            // Obtenemos el Timeline de Tweets de TWITTER API
            List<Models.TwitterTimeline> TwitterList = await _service.GetTimelineTwitter(user);

            //Construyo un objeto JSON complejo para enviar a la API de Lenguaje de MCS
            JArray arrayRoot = new JArray();

            foreach (var item in TwitterList)
            {
                JObject o = JObject.FromObject(
                        new Models.Document
                        {
                            id = item.id_str,
                            text = item.text
                        }
                    );

                arrayRoot.Add(o);
            }
            
            JObject objectRoot = new JObject();
            objectRoot["documents"] = arrayRoot;
            
            // Obtenemos la lista de los Lenguajes correspondientes a cada Tweet mediante la API de Lenguaje de MCS
            string json = objectRoot.ToString();
            List<Models.LanguageDocument> LanguageDocumentsList = await _service.PostTimelineLanguages(TwitterList, json);

            var join = from twitter in TwitterList
                       from languages in LanguageDocumentsList
                       where twitter.id_str == languages.id
                       select new { Lang = languages.detectedLanguages.First().iso6391Name, TID = twitter.id_str, LID = languages.id };

            foreach (var item in TwitterList)
            {
                string language = join.FirstOrDefault(x => x.LID == item.id_str).Lang;
                item.language = language;
            }

            //
            // Obtenemos la lista de Emociones de cada Tweet del Timeline mediante la API de Emociones de MCS
            ///////

            //Construyo un objeto JSON complejo para enviar a la API de Lenguaje de MCS
            arrayRoot = new JArray();

            foreach (var item in TwitterList)
            {
                JObject o = JObject.FromObject(
                        new Models.Document
                        {
                            id = item.id_str,
                            text = item.text
                        }
                    );

                arrayRoot.Add(o);
            }

            objectRoot = new JObject();
            objectRoot["documents"] = arrayRoot;
            json = objectRoot.ToString();

            List<Models.SentimentDocument> SentimentalList = await _service.PostTimelineEmotions(TwitterList, json);

            double angryPercentage = 0;
            double angryTotalSum = 0;
            foreach (var item in SentimentalList)
            {
                angryTotalSum += item.score;
            }
            angryPercentage = angryTotalSum / SentimentalList.Count * 100;

            ViewData["AngryPercentage"] = string.Format("{0:0.00}", angryPercentage);

            return View("TwitterResult", await _service.GetTimelineTwitter(user));
        }
    }
}
