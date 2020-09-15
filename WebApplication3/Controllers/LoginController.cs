using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Interfaces;

namespace WebApplication3.Controllers
{
    public class LoginController : Controller
    {
        private IDataRepository _iDR;

        public LoginController(IDataRepository iDR)
        {
            this._iDR = iDR;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoginUser(FormCollection fc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string result = _iDR.LoginUser(fc["UserId"], fc["Password"]);
                    if (result == "login")
                    {
                        Session["UserId"] = fc["UserId"];
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["CR"] = "Please enter correct credentials";
                        TempData.Keep();
                        return RedirectToAction("Index", "Login");
                    }
                }
                else
                {
                    TempData["CR"] = "Please enter correct credentials";
                    TempData.Keep();
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}