﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tut3.Models;
using tut3.Services;

namespace tut3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentDbService _dbService;

        public StudentsController(IStudentDbService dbService)
        {
            _dbService = dbService;
        }
        

        //[HttpGet("secured/{indexNumber}")]
        //public IActionResult GetStudent(string indexNumber)
        //{
        //    var student = _dbService.GetStudent(indexNumber);
        //    if (student == null)
        //    {
        //        return NotFound($"No studetn with provided index number ({indexNumber}");

        //    }
        //    else return Ok(student);
        //}



        ////public IActionResult GetStudents()
        ////{
        ////    return Ok(_dbService.GetStudents());
        ////}


        //[HttpGet("{id}")]
        //public IActionResult GetSemester(string id)
        //{
        //    string semester = null; ;

        //    using (var connection = new SqlConnection(@"Data Source=db-mssql;Initial Catalog=s19200;Integrated Security=True"))
        //    {
        //        using (var command = new SqlCommand())
        //        {
        //            command.Connection = connection;
        //            command.CommandText = @"select Semester from Enrollment where IdEnrollment =
        //                                    (select idEnrollment from Student where IndexNumber = @id;); ";
        //            connection.Open();
        //            var rd = command.ExecuteReader();

        //            if (rd.Read())
        //            {
        //                semester = rd["Semester"].ToString();
        //            }

        //            return Ok("student" + id + "is currently on semester" + semester);

        //        }
        //    }
        //}



        //        [HttpPost]
        //        public IActionResult CreateStudent(Student student)
        //        {
        //            student.IndexNumber = $"s{new Random().Next(1, 20000)}";

        //            return Ok(student);
        //        }

        //        [HttpPut("{id}")]
        //        public IActionResult put(int id)
        //        {
        //            return Ok("Update complete");
        //        }

        //        [HttpDelete("{id}")]
        //        public IActionResult delete(int id)
        //        {
        //            return Ok("Delete completed");
        //        } */
    }

}


