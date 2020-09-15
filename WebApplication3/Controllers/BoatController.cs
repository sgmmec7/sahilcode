using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Interfaces;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class BoatController : Controller
    {
        private IDataRepository _iDR;

        public BoatController(IDataRepository iDR)
        {
            this._iDR = iDR;
        }

        [HttpGet]
        public ActionResult Index(string val)
        {
            TempData["Val"] = val;
            return View();
        }

        [HttpGet]
        public JsonResult getReq()
        {
            int Id = 0;
            try
            {
                Id = _iDR.getReq();
                return Json(new { Result = Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = Id }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult InsertBoat(clsBoat objBoat)
        {
            try
            {
                var msg = _iDR.InsertBoatInformation(objBoat);
                return Json(new { Result = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult getStatusBoat(clsBoat objBoat)
        {
            try
            {
                var msg = _iDR.retrievBoatStatus(objBoat);
                return Json(new { Result = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "error" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}