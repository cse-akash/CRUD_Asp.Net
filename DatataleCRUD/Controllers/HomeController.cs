using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatataleCRUD.Models;

namespace DatataleCRUD.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEmployees()
        {
            using (MyDatatableEntities dt = new MyDatatableEntities())
            {
                var employees = dt.Employees.OrderBy(a => a.FirstName).ToList();
                return Json(new { data = employees }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Save(int id)
        {
            using (MyDatatableEntities dt = new MyDatatableEntities())
            {
                var employee = dt.Employees.Where(a => a.EmployeeId == id).FirstOrDefault();
                return View(employee);
            }
        }

        [HttpPost]
        public ActionResult Save(Employee emp)
        {
            bool status = false;
            if(ModelState.IsValid)
            {
                using (MyDatatableEntities dt = new MyDatatableEntities())
                {
                    if(emp.EmployeeId > 0)
                    {
                        //Edit
                        var employee = dt.Employees.Where(a => a.EmployeeId == emp.EmployeeId).FirstOrDefault();
                        if(employee != null)
                        {
                            employee.FirstName = emp.FirstName;
                            employee.LastName = emp.LastName;
                            employee.EmailID = emp.EmailID;
                            employee.City = emp.City;
                            employee.Country = emp.Country;
                        }
                    }
                    else
                    {
                        //Save
                        dt.Employees.Add(emp);
                    }

                    dt.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (MyDatatableEntities dt = new MyDatatableEntities())
            {
                var v = dt.Employees.Where(a => a.EmployeeId == id).FirstOrDefault();
                if(v != null)
                {
                    return View(v);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteEmployee(int id)
        {
            bool status = false;
            using (MyDatatableEntities dt = new MyDatatableEntities())
            {
                var v = dt.Employees.Where(a => a.EmployeeId == id).FirstOrDefault();
                if(v != null)
                {
                    dt.Employees.Remove(v);
                    dt.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

    }
}