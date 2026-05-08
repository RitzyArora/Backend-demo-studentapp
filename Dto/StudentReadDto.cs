using StudentCrudAppWithEFCoreCodeFirst.Models;

namespace StudentCrudAppWithEFCoreCodeFirst.Dto
{
    public class StudentReadDto
    {
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public int StudentAge { get; set; }
        public int StudentMarks { get; set; }

        public string? Grade { get; set; }   

        public List<CourseDto> Courses { get; set; } //One to many
        public AddressDto Address { get; set; } //One to One
        public List<SubjectDto> Subjects { get; set; } //Many to many
    }
}
