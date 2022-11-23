using ESi_Test.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ESi_Test.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly NorthwindContext _context;
        public EmployeesController(NorthwindContext context)
        {
            _context = context;
        }

        public IActionResult Index(string ReportsTo)
        {
            if(ReportsTo == null)
            {
                return View(new List<Employee>());
            }
            else
            {
                int reportsId;
                bool success = int.TryParse(ReportsTo, out reportsId);

                if (success)
                {
                    List<Employee> employees =  _context.Employees.Where(e => e.ReportsTo == reportsId).ToList();
                    List<Employee> subEmployees = new List<Employee>();
                    foreach (var em in employees)
                    {
                        subEmployees = subEmployees.Union(_context.Employees.Where(e => e.ReportsTo == em.EmployeeId)).ToList();
                    }
                    
                    var result = employees.Union(subEmployees).OrderBy(e=>e.EmployeeId).ToList();
                    return View(result);
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }

        }
    }
}
