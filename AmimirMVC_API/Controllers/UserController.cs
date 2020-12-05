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
    public class UserController : Controller
    {
        private string baseURL = "https://localhost:44300";
        private string basePath = "/";

        private bool UsuarioAutenticado()
        {
            return HttpContext.Session["token"] != null;
        }


        // GET: User
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

            HttpResponseMessage response = httpClient.GetAsync(basePath + "api/Usuario").Result;

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Authentication");
            }

            string data = response.Content.ReadAsStringAsync().Result;
            List<UserCLS> usuarios = JsonConvert.DeserializeObject<List<UserCLS>>(data);

            return Json(
                new {
                    success = true,
                    data = usuarios,
                    message = "done"
                },
                JsonRequestBehavior.AllowGet
                ); 
        }

        public ActionResult Guardar( int ID, string Nombre, DateTime? FechaNacimiento, string Username, string CorreoElectronico, string Contrasena)
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
                UserCLS usuario = new UserCLS();
                usuario.ID = ID;
                usuario.Nombre = Nombre;
                usuario.FechaNacimiento = FechaNacimiento;
                usuario.Username = Username;
                usuario.CorreoElectronico = CorreoElectronico;
                usuario.Contrasena = sha256(Contrasena);

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

                string usuarioJson = JsonConvert.SerializeObject(usuario);
                HttpContent body = new StringContent(usuarioJson, Encoding.UTF8, "application/json");

                if (ID == 0)
                {
                    HttpResponseMessage response = httpClient.PostAsync(basePath +"api/Usuario", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                                new {
                                    success = true,
                                    message = "Usuario creado satisfactoriamente"
                                }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Authentication"); 
                    }
                }
                else
                {
                    HttpResponseMessage response = httpClient.PutAsync($"{basePath}api/Usuario/{ID}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                                new
                                {
                                    success = true,
                                    message = "Usuario modificado satisfactoriamente"
                                }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Authentication");
                    }
                }
                throw new Exception("Error desconocido al guardar usuario");
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

        public ActionResult Eliminar ( int ID )
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


                HttpResponseMessage response = httpClient.DeleteAsync($"{basePath}api/Usuario/{ID}").Result;
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
                throw new Exception("Error desconocido al borrar usuario");
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