using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DailyMarker.Controllers
{
    public class TaskUsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}