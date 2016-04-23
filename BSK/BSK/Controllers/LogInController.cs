using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSK.Models;
using System.Net.Http;
using System.Net;

namespace BSK.Controllers
{
    public class LogInController : Controller
    {
        public HttpResponseMessage Post(LogInZapytanie dane)
        {
            HttpResponseMessage odpowiedz;
            using (DB baza = new DB())
            {
                List<Uzytkownik> uzytkownicy = baza.Uzytkownicy.ToList();
                List<Rola> role = baza.Rolee.ToList();
                List<Uprawnienie> uprawnienia = baza.Uprawnienia.ToList();
                if(uzytkownicy.Any(u => u.Login == dane.Login && u.Haslo == Uzytkownik.sha256(dane.Haslo)))
                {
                    Uzytkownik uzytkownik = uzytkownicy.First(u => u.Login == dane.Login);
                    IEnumerable<Rola> uzytkownik_role = role.Where(r => uzytkownik.Uzytkownik_Rola.Select(ur => ur.ID_Roli).Contains(r.ID_Roli));
                }

                else
                {
                        //Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Niepoprawne dane!");
                }
            }


            return odpowiedz;
        }
    }
}