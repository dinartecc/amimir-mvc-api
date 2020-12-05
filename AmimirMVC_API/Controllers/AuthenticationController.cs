using AmimirMVC_API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        private string basePath = "/";
        public ActionResult Index()
        {
            Token token = HttpContext.Session["token"] as Token;
            if ( token==null || token.ExpiresAt < DateTime.Now )
            {
                ViewBag.IsLoggedIn = false;
                ViewBag.IsAdmin = false;
            }
            else
            {
                ViewBag.IsLoggedIn = true;
                ViewBag.IsAdmin = token.isAdmin;
                ViewBag.Username = token.Username;
            }
            return View();
        }

        public ActionResult Login(UserCLS user)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseURL);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var request = new AuthReqCLS();

            request.user = user;
            request.ClientSecret = ConfigurationManager.AppSettings["CLIENT_SECRET"];

            string stringData = JsonConvert.SerializeObject(request);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(basePath + "api/Token", contentData).Result;

            string stringJWT = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.IsLoggedIn = false;
                ViewBag.Message = "Credenciales incorrectas";
                return View("Index");
            }

            Token token = JsonConvert.DeserializeObject<Token>(stringJWT);

            token.Username = user.Username;

            ViewBag.IsLoggedIn = true;
            ViewBag.Username = user.Username;
            ViewBag.IsAdmin = token.isAdmin;
            ViewBag.Message = "Has iniciado sesión correctamente";


            Session["token"] = token;

            return View("Index");
        }

        public ActionResult Logout()
        {
            ViewBag.IsLoggedIn = false;

            Token token = HttpContext.Session["token"] as Token;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseURL);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var req = new RefRequestCLS();
            req.RefreshToken = token.RefreshToken;
            req.ClientSecret = ConfigurationManager.AppSettings["CLIENT_SECRET"];

            string stringData = JsonConvert.SerializeObject(req);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            client.PostAsync(basePath + "api/DeleteToken", contentData);

            ViewBag.Message = "Has cerrado la sesión";
            HttpContext.Session.Abandon();
            return View("Index");
        }
    }
}