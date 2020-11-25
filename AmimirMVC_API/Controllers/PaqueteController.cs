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
    public class PaqueteController : Controller
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

            HttpResponseMessage response = httpClient.GetAsync("/api/Paquetes").Result;

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Authentication");
            }

            string data = response.Content.ReadAsStringAsync().Result;
            List<PaqueteCLS> paquetes = JsonConvert.DeserializeObject<List<PaqueteCLS>>(data);

            return Json(
                new
                {
                    success = true,
                    data = paquetes,
                    message = "done"
                },
                JsonRequestBehavior.AllowGet
                );
        }

        public ActionResult Guardar(int ID, string Nombre, int Duracion, decimal Precio)
        {
            try
            {
                PaqueteCLS paquete = new PaqueteCLS();
                paquete.ID = ID;
                paquete.Nombre = Nombre;
                paquete.Duracion = Duracion;
                paquete.Precio = Precio;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());


                string paqueteJson = JsonConvert.SerializeObject(paquete);
                HttpContent body = new StringContent(paqueteJson, Encoding.UTF8, "application/json");

                if (ID == 0)
                {
                    HttpResponseMessage response = httpClient.PostAsync("/api/Paquetes", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                                new
                                {
                                    success = true,
                                    message = "Paquete creado satisfactoriamente"
                                }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Authentication");
                    }
                }
                else
                {
                    HttpResponseMessage response = httpClient.PutAsync($"/api/Paquetes/{ID}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                                new
                                {
                                    success = true,
                                    message = "Paquete modificado satisfactoriamente"
                                }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Authentication");
                    }
                }
                throw new Exception("Error desconocido al guardar paquete");
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


                HttpResponseMessage response = httpClient.DeleteAsync($"/api/Paquetes/{ID}").Result;
                if (response.IsSuccessStatusCode)
                {
                    return Json(
                            new
                            {
                                success = true,
                                message = "Paquete eliminado satisfactoriamente"
                            }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return RedirectToAction("Index", "Authentication");

                }
                throw new Exception("Error desconocido al borrar paquete");
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