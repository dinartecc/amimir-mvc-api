using AmimirMVC_API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static AmimirMVC_API.Controllers.Utils;

namespace AmimirMVC_API.Controllers
{
    public class CapituloController : Controller
    {
        // GET: Capitulo
        private string baseURL = "https://localhost:44300";
        private string basePath = "/";
        HttpClient httpClient = new HttpClient();

        public ActionResult Index()
        {
            Token token = HttpContext.Session["token"] as Token;
            if (token == null || token.ExpiresAt < DateTime.Now)
            {
                return RedirectToAction("Index", "Authentication");
            }
            ViewBag.IsAdmin = token.isAdmin;

            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);


            string responseAnime = httpClient.GetAsync(basePath + "api/Animes").Result.Content.ReadAsStringAsync().Result;
            ViewBag.Animes = JsonConvert.DeserializeObject<List<AnimeCLS>>(responseAnime);

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

            HttpResponseMessage response = httpClient.GetAsync(basePath + "api/Capitulos").Result;

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
        public ActionResult Guardar(CapituloCLS capitulo,List<UrlAlternativo> urlAlternativo)
        {
            Token token = HttpContext.Session["token"] as Token;
            if (token == null || token.ExpiresAt < DateTime.Now)
            {
                return RedirectToAction("Index", "Authentication");
            }
            var req = new CapituloWrapper();
            req.Capitulo = capitulo;
            req.urlAlternativos = urlAlternativo;
            if (capitulo.ID == 0) {
             req.Capitulo.FechaPublicado = DateTime.Now;
            }

            try
            {

                int ID = capitulo.ID ?? 0;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                string reqJson = JsonConvert.SerializeObject(req);
                HttpContent body = new StringContent(reqJson, Encoding.UTF8, "application/json");

                if (ID == 0)
                {
                    HttpResponseMessage response = httpClient.PostAsync(basePath + "api/Capitulos", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                                new
                                {
                                    success = true,
                                    message = "Capitulo creado satisfactoriamente"
                                }, JsonRequestBehavior.AllowGet);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return RedirectToAction("Index", "Authentication");
                    }
                }
                else
                {
                    HttpResponseMessage response = httpClient.PutAsync($"{basePath}api/Capitulos/{ID}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                                new
                                {
                                    success = true,
                                    message = "Capitulo modificado satisfactoriamente"
                                }, JsonRequestBehavior.AllowGet);
                    }
                }
                throw new Exception("Error desconocido al guardar capitulo");
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


                HttpResponseMessage response = httpClient.DeleteAsync($"{basePath}api/Capitulos/{ID}").Result;
                if (response.IsSuccessStatusCode)
                {
                    return Json(
                            new
                            {
                                success = true,
                                message = "Capitulo eliminado satisfactoriamente"
                            }, JsonRequestBehavior.AllowGet);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Index", "Authentication");
                }
                throw new Exception("Error desconocido al borrar capitulo");
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