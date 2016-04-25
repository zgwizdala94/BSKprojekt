using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using BSK.Models;
using BSK.Controllers;

namespace BSK
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                AuthenticationHeaderValue authValue = actionContext.Request.Headers.Authorization;

                if (authValue != null && !String.IsNullOrWhiteSpace(authValue.Parameter))
                {
                    using (DB context = new DB())
                    {
                        if (context.Sesje.Any(s => s.ID_Sesji == authValue.Parameter))
                        {
                            var sesja = context.Sesje.FirstOrDefault(s => s.ID_Sesji == authValue.Parameter);
                            if (LogInController.konwertujNaStempel(DateTime.Now) < sesja.Data_waznosci)
                            {
                                var firstOrDefault = context.Rolee.FirstOrDefault(r => r.ID_Roli == sesja.ID_Roli);
                                if (firstOrDefault != null)
                                {
                                    var rolesPermissions = firstOrDefault.Uprawnienie_Rola;
                                    if (rolesPermissions != null)
                                    {
                                        int idUprawnienia = context.Uprawnienia.FirstOrDefault(p => (p.Nazwa_tabeli+"_"+p.Instrukcja) == Roles).ID_Uprawnienia;
                                        if (rolesPermissions.All(rp => rp.ID_Uprawnienia != idUprawnienia))
                                        {
                                            actionContext.Response =
                                                actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                                        }
                                    }
                                    else
                                    {
                                        actionContext.Response =
                                            actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                                    }
                                }
                                else
                                {
                                    actionContext.Response =
                                        actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                                }
                            }
                            else
                            {
                                actionContext.Response =
                                    actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                            }
                        }
                        else
                        {
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                        }
                    }
                }
            }
            catch (Exception)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return;

            }
        }
    }
}
