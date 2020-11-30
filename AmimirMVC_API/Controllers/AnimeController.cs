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
    public class AnimeController : Controller
    {
        private string baseURL = "https://localhost:44300";
        HttpClient httpClient = new HttpClient();


        // GET: Anime
        public ActionResult Index()
        {
            Token token = HttpContext.Session["token"] as Token;
            if (token == null || token.ExpiresAt < DateTime.Now )
            {
                return RedirectToAction("Index", "Authentication");
            }
            ViewBag.IsAdmin = token.isAdmin;

            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            string responseGenero = httpClient.GetAsync("/api/Generos").Result.Content.ReadAsStringAsync().Result;
            ViewBag.Generos = JsonConvert.DeserializeObject<List<GeneroCLS>>(responseGenero);
            string responseEstudio = httpClient.GetAsync("/api/Estudios").Result.Content.ReadAsStringAsync().Result;
            ViewBag.Estudios = JsonConvert.DeserializeObject<List<EstudioCLS>>(responseEstudio);
            string responseEstado = httpClient.GetAsync("/api/Estados").Result.Content.ReadAsStringAsync().Result;
            ViewBag.Estados = JsonConvert.DeserializeObject<List<EstadoCLS>>(responseEstado);
            string responseActor = httpClient.GetAsync("/api/Actores").Result.Content.ReadAsStringAsync().Result;
            ViewBag.Actores = JsonConvert.DeserializeObject<List<ActorCLS>>(responseActor);

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

            HttpResponseMessage response = httpClient.GetAsync("/api/Animes").Result;

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Authentication");
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
        public ActionResult Guardar(AnimeCLS anime, List<int> generos, List<int> estudios, List<PersonajeCLS> personajes, List<string> nombresAlternativos)
        {
            Token token = HttpContext.Session["token"] as Token;
            if (token == null || token.ExpiresAt < DateTime.Now)
            {
                return RedirectToAction("Index", "Authentication");
            }

            try
            {

                int ID = anime.ID ?? 0;

                AnimeWrapper req = new AnimeWrapper();
                req.Anime = anime;
                req.Generos = generos ?? new List<int>();
                req.Estudios = estudios ?? new List<int>();
                req.Personajes = personajes ?? new List<PersonajeCLS>();
                req.NombresAlternativos = nombresAlternativos ?? new List<string>();

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                string reqJson = JsonConvert.SerializeObject(req);
                HttpContent body = new StringContent(reqJson, Encoding.UTF8, "application/json");

                if (ID == 0)
                {
                    HttpResponseMessage response = httpClient.PostAsync("/api/Animes", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                                new
                                {
                                    success = true,
                                    message = "Anime creado satisfactoriamente"
                                }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Authentication");
                    }
                }
                else
                {
                    HttpResponseMessage response = httpClient.PutAsync($"/api/Animes/{ID}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                                new
                                {
                                    success = true,
                                    message = "Anime modificado satisfactoriamente"
                                }, JsonRequestBehavior.AllowGet);
                    }
                }
                throw new Exception("Error desconocido al guardar anime");
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


                HttpResponseMessage response = httpClient.DeleteAsync($"/api/Animes/{ID}").Result;
                if (response.IsSuccessStatusCode)
                {
                    return Json(
                            new
                            {
                                success = true,
                                message = "Usuario eliminado satisfactoriamente"
                            }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return RedirectToAction("Index", "Authentication");

                }
                throw new Exception("Error desconocido al borrar anime");
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