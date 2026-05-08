using System.Text.Json.Serialization;

namespace StudentCrudAppWithEFCoreCodeFirst.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        [JsonIgnore]
        public List<Student>? Students { get; set; }
    }
}
