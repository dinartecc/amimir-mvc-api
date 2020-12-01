﻿using AmimirMVC_API.Models;
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
    public class EstudioController : Controller
    {
        private string baseURL = "https://localhost:44300";
        HttpClient httpClient = new HttpClient();


        // GET: Anime
        public ActionResult Index()
        {
            Token token = HttpContext.Session["token"] as Token;
            if (token == null || token.ExpiresAt < DateTime.Now)
            {
                return RedirectToAction("Index", "Authentication");
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

            HttpResponseMessage response = httpClient.GetAsync("/api/Estudios").Result;

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
        public ActionResult Guardar(EstudioCLS estudio)
        {
            Token token = HttpContext.Session["token"] as Token;
            if (token == null || token.ExpiresAt < DateTime.Now)
            {
                return RedirectToAction("Index", "Authentication");
            }

            try
            {

                int ID = estudio.ID ?? 0;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                string reqJson = JsonConvert.SerializeObject(estudio);
                HttpContent body = new StringContent(reqJson, Encoding.UTF8, "application/json");

                if (ID == 0)
                {
                    HttpResponseMessage response = httpClient.PostAsync("/api/Estudios", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                                new
                                {
                                    success = true,
                                    message = "Estudio creado satisfactoriamente",
                                }, JsonRequestBehavior.AllowGet);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        return RedirectToAction("Index", "Authentication");
                    }
                }
                else
                {
                    HttpResponseMessage response = httpClient.PutAsync($"/api/Estudios/{ID}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                                new
                                {
                                    success = true,
                                    message = "Estudio modificado satisfactoriamente"
                                }, JsonRequestBehavior.AllowGet);
                    }
                }
                throw new Exception("Error desconocido al guardar el estudio");
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


                HttpResponseMessage response = httpClient.DeleteAsync($"/api/Estudios/{ID}").Result;
                if (response.IsSuccessStatusCode)
                {
                    return Json(
                            new
                            {
                                success = true,
                                message = "Usuario eliminado satisfactoriamente"
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
                           message = "Este estudio no puede ser borrado porque tiene animes asociados"
                       }, JsonRequestBehavior.AllowGet);
                }
                throw new Exception("Error desconocido al borrar estudio");
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