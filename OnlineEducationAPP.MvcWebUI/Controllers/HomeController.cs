﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationAPP.MvcWebUI.Models;

namespace OnlineEducationAPP.MvcWebUI.Controllers
{
    
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
        
       
    }
}