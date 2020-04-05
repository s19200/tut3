using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tut3.Models;

namespace tut3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {


        public IActionResult GetStudents(string orderBy)
        {
            var students = new List<Student>();
            using (var connection = new SqlConnection(@"Data Source = db - mssql; Initial Catalog = s19200; Integrated Security = True"))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @" s.FirstName, s.LastName, s.BirthDate, st.Name as Studies, e.Semester 
                                            from Student s  
                                            join Enrollment e on e.IdEnrollment = s.IdEnrollment  
                                            join Studies st on st.IdStudy = e.IdStudy;";
                    connection.Open();
                    var rd = command.ExecuteReader();
                    while (rd.Read())
                    {
                        var st = new Student
                        {
                            FirstName = rd["FirstName"].ToString(),
                            LastName = rd["LastName"].ToString(),
                            Studies = rd["Studies"].ToString(),
                            DateOfBirth = DateTime.Parse(rd["BirthDate"].ToString()),
                            Semester = int.Parse(rd["Semester"].ToString())
                        };

                        students.Add(st);
                    }
                }
            }
            return Ok();
        }


        [HttpGet("{id}")]
        public IActionResult GetSemester(string id)
        {
            string semester = null; ;

            using (var connection = new SqlConnection(@"Data Source = db - mssql; Initial Catalog = s19200; Integrated Security = True"))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"select Semester from Enrollment where IdEnrollment =
                                            (select idEnrollment from Student where IndexNumber = @id;); ";
                    connection.Open();
                    var rd = command.ExecuteReader();

                    if (rd.Read())
                    {
                         semester = rd["Semester"].ToString();
                    }

                    return Ok("student"+id +"is currently on semester"+ semester);

                }
            }
        }


        /*
                [HttpPost]
                public IActionResult CreateStudent(Student student)
                {
                    student.IndexNumber = $"s{new Random().Next(1, 20000)}";

                    return Ok(student);
                }

                [HttpPut("{id}")]
                public IActionResult put(int id)
                {
                    return Ok("Update complete");
                }

                [HttpDelete("{id}")]
                public IActionResult delete(int id)
                {
                    return Ok("Delete completed");
                } */
    }

    }


         