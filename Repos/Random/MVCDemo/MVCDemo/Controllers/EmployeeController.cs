﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCDemo.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Details()
        {
            EmployeeEntities employeeEntities = new EmployeeEntities();
            var employee = employeeEntities.tblEmployees.ToList();

            return View(employee);
        }
    }
}