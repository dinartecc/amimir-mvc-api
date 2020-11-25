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

namespace AmimirMVC_API.Controllers
{
    public class AnimeController : Controller
    {
        private string baseURL = "https://localhost:44300";

        private bool UsuarioAutenticado()
        {
            return HttpContext.Session["token"] != null;
        }



        // GET: Anime
        public ActionResult Index()
        {

            if (!UsuarioAutenticado())
            {
                return RedirectToAction("Index", "Authentication");
            }

            return View();
        }

        public ActionResult Lista()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.GetAsync("/api/Animes").Result;

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Authentication");
            }

            string data = response.Content.ReadAsStringAsync().Result;
            List<AnimeCLS> animes = JsonConvert.DeserializeObject<List<AnimeCLS>>(data);

            return Json(
                new
                {
                    success = true,
                    data = animes,
                    message = "done"
                },
                JsonRequestBehavior.AllowGet
                );
        }

        public ActionResult Guardar(int ID, string Nombre, DateTime? FechaEstreno, string Sinopsis, decimal Puntuacion, decimal Popularidad, int EstadoID)
        {
            if (!UsuarioAutenticado())
            {
                return Json(
                        new
                        {
                            success = false,
                            message = "Usuario no autenticado"
                        }, JsonRequestBehavior.AllowGet);
            }
                try
            {
                AnimeCLS anime = new AnimeCLS();
                anime.ID = ID;
                anime.Nombre = Nombre;
                anime.FechaEstreno = FechaEstreno;
                anime.Sinopsis = Sinopsis;
                anime.Puntuacion = Puntuacion;
                anime.Popularidad = Popularidad;
                anime.EstadoID = EstadoID;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

                string animeJson = JsonConvert.SerializeObject(anime);
                HttpContent body = new StringContent(animeJson, Encoding.UTF8, "application/json");

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

        public ActionResult Eliminar(int ID)
        {

            if (!UsuarioAutenticado())
            {
                return Json(
                        new
                        {
                            success = false,
                            message = "Usuario no autenticado"
                        }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());


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