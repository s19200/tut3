using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tut3.Models
{
       public class Student
        {
            public int IndexNumber { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string Studies { get; set; }
            public int Semester { get; set; }
        }
    }