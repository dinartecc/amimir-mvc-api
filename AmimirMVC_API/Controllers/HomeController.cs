using AmimirMVC_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmimirMVC_API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Token token = HttpContext.Session["token"] as Token;
            if (token == null || token.ExpiresAt > DateTime.Now)
            {
                return RedirectToAction("Index", "Authentication");
            }
            ViewBag.IsAdmin = token.isAdmin;
            return View();
        }
    }
}