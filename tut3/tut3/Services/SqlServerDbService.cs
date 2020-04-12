using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using tut3.Models;
using tut3.Requests;
using tut3.Responses;

namespace tut3.Services
{
    public class SqlServerDbService : ControllerBase, IStudentDbService
    {
        //public IEnumerable<Student> GetStudents()
        //{
        //    var students = new List<Student>();
        //    using (var connection = new SqlConnection(@"Data Source=db-mssql;Initial Catalog=s19200;Integrated Security=True"))
        //    {
        //        using (var command = new SqlCommand())
        //        {
        //            command.Connection = connection;
        //            command.CommandText = @"select s.IndexNumber, s.FirstName, s.LastName from Student s;";
        //            connection.Open();
        //            var rd = command.ExecuteReader();
        //            while (rd.Read())
        //            {
        //                var st = new Student
        //                {
        //                    IndexNumber = int.Parse(rd["IndexNumber"].ToString()),
        //                    FirstName = rd["FirstName"].ToString(),
        //                    LastName = rd["LastName"].ToString()
        //                };
        //                students.Add(st);
        //            }
        //        }
        //    }
        //    return students;
        //}
        public IActionResult AddEnrollment(EnrollStudent request)
        {
            using (var connection = new SqlConnection(@"Data Source=db-mssql;Initial Catalog=s19200;Integrated Security=True"))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    var transaction = connection.BeginTransaction();
                    connection.Open();
                    //checking if all the data is provided
                    if ((request.IndexNumber.ToString() == null) || (request.FirstName == null) || (request.LastName == null) || (request.Studies.ToString() == null))
                    {
                        return BadRequest(400);
                    }

                    //checking if Studies are valid
                    command.CommandText = "select 1 from Studies where idStudy = @idStudy";
                    command.Parameters.AddWithValue("idStudy", request.Studies);

                    var rd = command.ExecuteReader();
                    if (!rd.Read())
                    {
                        return BadRequest("Invalid Studies value");
                    }
                    int idStudy = (int)rd["IdStudy"];
                    rd.Close();


                    command.CommandText = "select MAX(idEnrollment) as maximalID from Enrollment where Semester=1 IdStudy=@idStudy";

                    command.Parameters.AddWithValue("IdStudy", request.Studies);
                    var rd1 = command.ExecuteReader();
                    var maxId = (int)rd1["maximalID"];

                    rd1.Close();
                    command.CommandText = "insert into Enrollment(idEnrollment, Semester, idStudy, StartDate) values (@idEnrollment, @Semester, @IDStudy, @StartDate)";
                    command.Parameters.AddWithValue("idEnrollment", maxId + 1);
                    command.Parameters.AddWithValue("Semester", 1);
                    command.Parameters.AddWithValue("IDStudy", request.Studies);
                    command.Parameters.AddWithValue("StartDate", DateTime.Now.ToString());


                    var rd2 = command.ExecuteReader();
                    rd2.Close();



                    command.CommandText = "select * from Student where IndexNumber = @IndexNumber";
                    command.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                    var rd3 = command.ExecuteReader();
                    if (!rd3.Read())
                    {
                        rd3.Close();
                        command.CommandText = "insert into Student(IndexNumber, FirstName, LastName, BirthDate, idEnrollment) values @IndexNumber, @FirstName, @LastName, @BirthDate, @idEnrollment";
                        command.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                        command.Parameters.AddWithValue("FirstName", request.FirstName);
                        command.Parameters.AddWithValue("LastName", request.LastName);
                        command.Parameters.AddWithValue("BirthDate", request.BirthDate.ToString());
                        command.Parameters.AddWithValue("idEnrollment", maxId + 1);

                        var rd4 = command.ExecuteReader();
                        rd4.Close();
                    }
                    else
                    {
                        return BadRequest("a student with given id already exists");
                    }


                    transaction.Commit();

                    var response = new EnrollStudentResponse
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        IndexNumber = request.IndexNumber
                    };

                    return Ok(response);

                }
            }
        }

        public IActionResult PromoteStudent(PromoteStudent request)
        {
            using (var connection = new SqlConnection(@"Data Source=db-mssql;Initial Catalog=s19200;Integrated Security=True"))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    var transaction = connection.BeginTransaction();
                    connection.Open();

                    command.CommandText = ("select * from Enrollment where idStudy=@Studies and Semester = @Semester");
                    var rd = command.ExecuteReader();
                    if (rd.Read())
                    {
                        command.Parameters.AddWithValue("Studies", request.Studies);
                        command.Parameters.AddWithValue("Semester", request.Semester);
                    }
                    else
                    {
                        return BadRequest(404);
                    }

                    rd.Close();

                    command.CommandText = " exec PromoteStudent @idStudies= @Studies, @Semester_ = @Semester";
                    var rd1 = command.ExecuteReader();

                    command.Parameters.AddWithValue("Studies", request.Studies);
                    command.Parameters.AddWithValue("Semester", request.Semester);
                }

            }
            var response = new PromoteStudentResponse
            {
                Semester = request.Semester,
                idStudies = request.Studies
            };

            return Ok(response);
        }
    }
}
