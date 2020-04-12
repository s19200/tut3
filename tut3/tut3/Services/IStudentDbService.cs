using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tut3.Models;

namespace tut3.Services
{
    public interface IStudentDbService
    {
        public IEnumerable<Student> GetStudents();
        public IStudentDbService GetStudent(string indexNumber);
        public IEnumerable<Enrollment> GetEnrollments(int indexNumber);

    }
}
