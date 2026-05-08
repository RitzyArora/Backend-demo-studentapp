using System.ComponentModel.DataAnnotations;

namespace StudentCrudAppWithEFCoreCodeFirst.Dto
{
    public class StudentCreateDto
    {
        [Required]
        public string? StudentName { get; set; }
        [Range(5,100)]
        public int StudentAge { get; set; }
        public int StudentMarks { get; set; }


        public List<CourseDto>? Courses { get; set; } //One to many
        public AddressDto? Address { get; set; } //One to One
        public List<SubjectDto>? Subjects { get; set; } //Many to many
    }
}
