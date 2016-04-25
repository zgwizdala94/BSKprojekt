using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BSK.Controllers
{
    public class AutorzyController : Controller
    {
        // GET: Autorzy
        public ActionResult Index()
        {
            return View();
        }
    }
}