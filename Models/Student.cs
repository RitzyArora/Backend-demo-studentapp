using System.Security.Cryptography.X509Certificates;

namespace StudentCrudAppWithEFCoreCodeFirst.Models
{
    public class Student
    {
         
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public int StudentAge { get; set; }
        public int StudentMarks { get; set; }

        public List<Course>? Courses { get; set; } //One to many
        public Address? Address{ get; set; } //One to One
        public List<Subject>? Subjects{ get; set; } //Many to many
    }
}

