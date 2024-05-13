using Licenta.DataAbstractionLayer;
using Licenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Licenta.Controllers
{
    public class ClassTypeController : Controller
    {
        private DAL dal = new DAL(); // Create an instance of your DAL

        public ActionResult Index()
        {
            List<ClassType> classTypes = dal.GetAllClassTypes();
            return View(classTypes);
        }

        public ActionResult ClassTypes()
        {
            List<ClassType> classType = dal.GetAllClassTypes();
            return View(classType);

        }

        public ActionResult AllClassTypes()
        {
            List<ClassType> classType = dal.GetAllClassTypes();
            return View(classType);
        }

        public ActionResult AddClassType()
        {
            return View("AddClassType");
        }


        [HttpPost]

        public ActionResult DeleteClassType(int id)
        {
            DAL dal = new DAL();
            dal.DeleteClassType(id);
            return RedirectToAction("ClassTypes");
        }


        public ActionResult DeleteClassTypeView(int id)
        {
            return View("DeleteClassType", id);
        }


        public ActionResult UpdateClassTypeView(int id)
        {
            DAL dal = new DAL();
            ClassType classType = dal.GetClassType(id);
            return View("UpdateClassType", classType);
        }

        public ActionResult UpdateClassType(ClassType classType)
        {
            DAL dal = new DAL();
            classType.type = Request.Params["type"];
            classType.difficulty = int.Parse(Request.Params["difficulty"]);
            dal.UpdateClassType(classType);
            return RedirectToAction("ClassTypes");
        }

        public ActionResult SaveAddedClassType()
        {
            ClassType classType = new ClassType();
            //doc.Id = int.Parse(Request.Params["docID"]);
            classType.type = Request.Params["type"];
            classType.difficulty = int.Parse(Request.Params["difficulty"]);
            DAL dal = new DAL();
            dal.AddNewClassType(classType);
            return RedirectToAction("ClassTypes");
        }
    }
}