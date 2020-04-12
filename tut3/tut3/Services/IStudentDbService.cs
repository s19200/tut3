using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tut3.Models;
using tut3.Requests;

namespace tut3.Services
{
    public interface IStudentDbService 
    {
        public IActionResult AddEnrollment(EnrollStudent request);
        public IActionResult PromoteStudent(PromoteStudent request);

    }
}
