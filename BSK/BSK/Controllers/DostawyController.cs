using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BSK.Controllers
{
    public class DostawyController : Controller
    {
        // GET: Dostawy
        public ActionResult Index()
        {
            ViewData["ID_Dostawy"] = "dostawa_id";
            
            return View();
        }
    }
}