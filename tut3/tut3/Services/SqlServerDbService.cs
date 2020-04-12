using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using tut3.Models;

namespace tut3.Services
{
    public class SqlServerDbService : IStudentDbService
    {
        public IEnumerable<Student> GetStudents()
        {
            var students = new List<Student>();
            using (var connection = new SqlConnection(@"Data Source=db-mssql;Initial Catalog=s19200;Integrated Security=True"))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"select s.IndexNumber, s.FirstName, s.LastName from Student s;";
                    connection.Open();
                    var rd = command.ExecuteReader();
                    while (rd.Read())
                    {
                        var st = new Student
                        {
                            IndexNumber = int.Parse(rd["IndexNumber"].ToString()),
                            FirstName = rd["FirstName"].ToString(),
                            LastName = rd["LastName"].ToString()
                        };
                        students.Add(st);
                    }
                }
            }
            return students;
        }

        public IEnumerable<Enrollment> GetEnrollments(int indexNumber)
        {
            throw new NotImplementedException();
        }

        public IStudentDbService GetStudent(string indexNumber)
        {
            throw new NotImplementedException();
        }
    }
}
