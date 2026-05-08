using System.Text.Json.Serialization;

namespace StudentCrudAppWithEFCoreCodeFirst.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int StudentId { get; set; }
        [JsonIgnore]
        public Student? Student { get; set; }//navigation property
    }
}
