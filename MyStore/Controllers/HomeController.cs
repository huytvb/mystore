using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyStore.Business;
using MyStore.Entities;
using MyStore.Framework;

namespace MyStore.Controllers
{
    public class HomeController : MyStoreController
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            string listUsername = "";
            IList<User> users = _userService.GetAllUser();
            foreach (var user in users)
            {
                listUsername += user.Account + " -";
            }
            ViewBag.Message = listUsername;
            SuccessNotification("Wellcome");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}
