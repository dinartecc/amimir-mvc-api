using AmimirMVC_API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static AmimirMVC_API.Controllers.Utils;

namespace AmimirMVC_API.Controllers
{
    public class EstadoController : Controller
    {
        private string baseURL = "https://localhost:44300";
        private string basePath = "/";
        HttpClient httpClient = new HttpClient();


        // GET: Anime
        public ActionResult Index()
        {
            Token token = HttpContext.Session["token"] as Token;
            if (token == null)
            {
                return RedirectToAction("Index", "Authentication");
            }


            if (token.ExpiresAt < DateTime.Now)
            {
                var newToken = RefrescarToken(token);
                if (newToken.AccessToken == token.AccessToken)
                {
                    HttpContext.Session.Abandon();
                    return RedirectToAction("Index", "Authentication");
                }
                else
                {
                    HttpContext.Session["token"] = newToken;
                }

            }
            else if (token.ExpiresAt.AddMinutes(-10) < DateTime.Now)
            {
                HttpContext.Session["token"] = RefrescarToken(token);
            }

            ViewBag.IsAdmin = token.isAdmin;

            return View();
        }


        public ActionResult Lista()
        {
            Token token = HttpContext.Session["token"] as Token;
            if (token == null || token.ExpiresAt < DateTime.Now)
            {
                return RedirectToAction("Index", "Authentication");
            }

            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            HttpResponseMessage response = httpClient.GetAsync(basePath + "api/Estados").Result;

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Authentication");
            }

            if (!response.IsSuccessStatusCode)
            {
                return Json(
                        new
                        {
                            success = false,
                        }, JsonRequestBehavior.AllowGet);
            }

            string data = response.Content.ReadAsStringAsync().Result;

            return Json(
                new
                {
                    success = true,
                    data = data,
                    message = "done"
                },
                JsonRequestBehavior.AllowGet
                );
        }

        [HttpPost]
        public ActionResult Guardar(EstadoCLS estado)
        {
            Token token = HttpContext.Session["token"] as Token;
            if (token == null || token.ExpiresAt < DateTime.Now)
            {
                return RedirectToAction("Index", "Authentication");
            }

            try
            {

                int ID = estado.ID;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                string reqJson = JsonConvert.SerializeObject(estado);
                HttpContent body = new StringContent(reqJson, Encoding.UTF8, "application/json");

                if (ID == 0)
                {
                    HttpResponseMessage response = httpClient.PostAsync(basePath + "api/Estados", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                                new
                                {
                                    success = true,
                                    message = "Estado creado satisfactoriamente",
                                }, JsonRequestBehavior.AllowGet);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return RedirectToAction("Index", "Authentication");
                    }
                }
                else
                {
                    HttpResponseMessage response = httpClient.PutAsync($"{basePath}api/Estados/{ID}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                                new
                                {
                                    success = true,
                                    message = "Estado modificado satisfactoriamente"
                                }, JsonRequestBehavior.AllowGet);
                    }
                }
                throw new Exception("Error desconocido al guardar el estado");
            }
            catch (Exception e)
            {
                return Json(
                        new
                        {
                            success = false,
                            message = e.InnerException
                        }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Eliminar(int ID)
        {

            Token token = HttpContext.Session["token"] as Token;
            if (token == null || token.ExpiresAt < DateTime.Now)
            {
                return RedirectToAction("Index", "Authentication");
            }

            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);


                HttpResponseMessage response = httpClient.DeleteAsync($"{basePath}api/Estados/{ID}").Result;
                if (response.IsSuccessStatusCode)
                {
                    return Json(
                            new
                            {
                                success = true,
                                message = "Estado eliminado satisfactoriamente"
                            }, JsonRequestBehavior.AllowGet);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Index", "Authentication");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    return Json(
                       new
                       {
                           success = false,
                           message = "Este estado no puede ser borrado porque tiene animes asociados"
                       }, JsonRequestBehavior.AllowGet);
                }
                throw new Exception("Error desconocido al borrar estado");
            }
            catch (Exception e)
            {
                return Json(
                       new
                       {
                           success = false,
                           message = e.InnerException
                       }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}