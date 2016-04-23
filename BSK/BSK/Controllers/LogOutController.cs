using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Net.Http;
using BSK.Models;
using System.Net;

namespace BSK.Controllers
{
    public class LogOutController : ApiController
    {
        public HttpResponseMessage Post(LogOutZapytanie dane)
        {
            HttpResponseMessage odpowiedz;
            using (DB baza = new DB())
            {
                if(baza.Sesje.Any(s=>s.ID_Sesji == dane.ID_Sesji))
                {
                    Sesja sesja = baza.Sesje.FirstOrDefault(s => s.ID_Sesji == dane.ID_Sesji);
                    baza.Sesje.Remove(sesja);
                    baza.SaveChanges();
                }
                odpowiedz = Request.CreateResponse(HttpStatusCode.OK);
            }
            return odpowiedz;
        }
    }
}