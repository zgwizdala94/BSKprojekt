using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSK.Models;
using System.Net;
using System.Web.Http;
using System.Net.Http;

namespace BSK.Controllers
{
    public class LogInController : ApiController
    {
        public HttpResponseMessage Post(LogInZapytanie dane)
        {
            HttpResponseMessage odpowiedz;
            using (DB baza = new DB())
            {
                List<Uzytkownik> uzytkownicyWszystko = baza.Uzytkownicy.ToList(); //3
                List<Rola> roleWszystko = baza.Rolee.ToList();
                List<Uprawnienie> uprawnieniaWszystko = baza.Uprawnienia.ToList();
                if (uzytkownicyWszystko.Any(u => u.Login == dane.Login && u.Haslo == Uzytkownik.sha256(dane.Haslo)))
                {
                    Uzytkownik uzytkownik = uzytkownicyWszystko.First(u => u.Login == dane.Login);
                    IEnumerable<Rola> uzytkownik_role = roleWszystko.Where(r => uzytkownik.Uzytkownik_Rola.Select(ur => ur.ID_Roli).Contains(r.ID_Roli));
                    if (!dane.Rola.HasValue) //logowanie uzytkownika, bez podania roli (pierwszy raz)
                    {
                        if (baza.Sesje.Any(s => s.ID_Uzytkownika == uzytkownik.ID_Uzytkownika))
                        //jezeli uzytkownik kiedykolwiek mial jakakolwiek sesje
                        {
                            var obecnaSesja = baza.Sesje.FirstOrDefault(s => s.ID_Uzytkownika == uzytkownik.ID_Uzytkownika);//1
                            if (DateTime.Now > new DateTime(obecnaSesja.Data_waznosci))
                            //jezeli jakas sesja nadal trwa, to zwroc tylko role ktora aktualnie (w tej sesji) pelni uzytkownik - na zadna inna nie moze sie zalogowac
                            {
                                Rola rola = uzytkownik_role.FirstOrDefault(ur => ur.ID_Roli == obecnaSesja.ID_Roli);
                                List<Rola> wynik = new List<Rola>();
                                wynik.Add(new Rola { ID_Roli = rola.ID_Roli, Nazwa = rola.Nazwa });
                                odpowiedz = Request.CreateResponse(HttpStatusCode.OK, wynik);
                            }
                            else
                            //sesja byla, ale juz nie trwa (minal okres waznosci) - zwroc wszystkie role danego uzytkownika
                            {
                                odpowiedz = Request.CreateResponse(HttpStatusCode.OK, uzytkownik_role); //2
                            }
                        }
                        else
                        //uzytkownik nie mial nigdy zadnej sesji wiec zwroc wszystkie jego role
                        {
                            odpowiedz = Request.CreateResponse(HttpStatusCode.OK, uzytkownik_role);
                        }
                    }
                    else
                    {
                        if (uzytkownik_role.Any(r => r.ID_Roli == dane.Rola.Value))
                        //7
                        {
                            Rola tempRola = uzytkownik_role.FirstOrDefault(r => r.ID_Roli == dane.Rola.Value);//8
                            Rola rola = new Rola { ID_Roli = tempRola.ID_Roli, Nazwa = tempRola.Nazwa };
                            IEnumerable<Uprawnienie> uprawnienia_rol = uprawnieniaWszystko.Where(upr => tempRola.Uprawnienie_Rola.Select(uprrola => uprrola.ID_Uprawnienia).Contains(upr.ID_Uprawnienia)); //4
                            List<Uprawnienie> uprawnienia = new List<Uprawnienie>();
                            foreach (Uprawnienie u in uprawnienia_rol)
                            {
                                uprawnienia.Add(new Uprawnienie { ID_Uprawnienia = u.ID_Uprawnienia, Instrukcja = u.Instrukcja, Nazwa_tabeli = u.Nazwa_tabeli }); //5
                            }
                            LogInOdpowiedz zawartoscOdpowiedzi = new LogInOdpowiedz
                            {
                                Nazwa = uzytkownik.Nazwa,
                                ID_Uzytkownika = uzytkownik.ID_Uzytkownika,
                                Rola = rola,
                                Uprawnienia = uprawnienia,
                                Data_waznosci = konwertujNaStempel(DateTime.Now.AddMinutes(10))//6
                            }; 
                            
                            // mamy juz przypisane uprawnienia do roli wybranej przez uzytkownika, teraz sesja

                            if(baza.Sesje.Any(s=>s.ID_Uzytkownika == uzytkownik.ID_Uzytkownika))
                            //jezeli istnieje jakas sesja dla tego uzytkownika (o znanej roli)
                            {
                                Sesja obecnaSesja = baza.Sesje.FirstOrDefault(s => s.ID_Uzytkownika == uzytkownik.ID_Uzytkownika); //1
                                if(DateTime.Now > new DateTime(obecnaSesja.Data_waznosci))
                                //...i ta sesja jest jeszcze wazna...
                                {
                                    if (obecnaSesja.ID_Roli != rola.ID_Roli)
                                    //...i to dla tej innej roli!!!
                                    {
                                        return Request.CreateErrorResponse(HttpStatusCode.Conflict,
                                            "Nie możesz zalogować się na tej roli, ponieważ jesteś już zalogowany na innej.");
                                    }
                                }
                                else
                                //ta sesja jest niewazna, wiec przypisujemy jej nowy id sesji
                                {
                                    obecnaSesja.ID_Sesji = HttpContext.Current.Session.SessionID; //9
                                }
                                zawartoscOdpowiedzi.ID_Sesji = obecnaSesja.ID_Sesji;
                                obecnaSesja.Data_waznosci = zawartoscOdpowiedzi.Data_waznosci;
                            }
                            else
                            //uzytkownik nie mial wczesniej sesji
                            {
                                zawartoscOdpowiedzi.ID_Sesji = HttpContext.Current.Session.SessionID;
                                baza.Sesje.Add(new Sesja
                                {
                                    ID_Roli = rola.ID_Roli,
                                    ID_Sesji = zawartoscOdpowiedzi.ID_Sesji,
                                    ID_Uzytkownika = uzytkownik.ID_Uzytkownika,
                                    Data_waznosci = zawartoscOdpowiedzi.Data_waznosci
                                });
                            }
                            odpowiedz = Request.CreateResponse(HttpStatusCode.OK, zawartoscOdpowiedzi);
                            baza.SaveChanges();
                        }
                        else
                        {
                            odpowiedz = Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Nie posiadasz roli!");
                        }

                    }
                }
                else
                // to jest else do pierwszego ifa sprawdzajacego uzytkownika i haslo! (jeszcze z rola ustawiona na null)
                {
                    odpowiedz = Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Niepoprawne dane!");
                }
            }
            return odpowiedz;
        }

        private static readonly DateTime znak = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static int konwertujNaStempel(DateTime teraz)
        {
            TimeSpan stempel = teraz - znak;
            return (int)stempel.TotalSeconds;
        }
    }
}