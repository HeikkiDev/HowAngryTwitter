using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

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
    }
}
