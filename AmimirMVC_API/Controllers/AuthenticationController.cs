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
    public class AuthenticationController : Controller
    {
        // GET: Authentication

        private string baseURL = "https://localhost:44300";
        public ActionResult Index()
        {
            if (HttpContext.Session["token"]==null)
            {
                ViewBag.Message = "Presione login para autenticarse";
            }
            else
            {
                ViewBag.Message = "Presione logout para cerrar sesión";
            }
            return View();
        }

        public ActionResult Login(UserCLS user)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseURL);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            string stringData = JsonConvert.SerializeObject(user);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync("/api/Token", contentData).Result;

            string stringJWT = response.Content.ReadAsStringAsync().Result;

            Token token = JsonConvert.DeserializeObject<Token>(stringJWT);

            HttpContext.Session.Add("token", token.AccessToken);

            ViewBag.Message = "Usuario Autenticado";

            return View("Index");
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            ViewBag.Message = "El usuario a cerrado la sesión";

            return View("Index");
        }
    }
}