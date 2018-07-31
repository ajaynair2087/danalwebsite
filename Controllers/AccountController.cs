using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DanalJSONApp.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace DanalJSONApp.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }
    }
}