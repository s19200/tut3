using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tut3.Requests;
using tut3.Responses;

namespace tut3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {

        [HttpPost]
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

                    return Ok(201);

                }
            }
        }

       
    }
}
