using StudentCrudAppWithEFCoreCodeFirst.Dto;
using StudentCrudAppWithEFCoreCodeFirst.Models;

namespace StudentCrudAppWithEFCoreCodeFirst.Mapper
{
    public class StudentMapper
    {
        public static StudentReadDto ToDto(Student student,string grade)
        {
            return new StudentReadDto
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                StudentAge = student.StudentAge,
                StudentMarks = student.StudentMarks,
                Grade = grade,
                Address = student.Address == null ? null : new AddressDto
                {
                    City = student.Address.City,
                    State = student.Address.State
                },
                Courses = student.Courses?.Select(c => new CourseDto
                {
                    CourseName = c.CourseName

                }).ToList(),

                Subjects = student.Subjects?.Select(c => new SubjectDto
                {
                    SubjectName = c.SubjectName

                }).ToList(),

            };
        }

        public static Student ToEntity(StudentCreateDto dto) 
        {
            return new Student {
            StudentName=dto.StudentName,
            StudentAge=dto.StudentAge,
            StudentMarks=dto.StudentMarks,
            Address=dto.Address == null ? null :new Address 
            {
                City = dto.Address.City,
                State = dto.Address.State
            },
            Courses=dto.Courses?.Select(c=>new Course
            {
                CourseName=c.CourseName
            }).ToList(),

                Subjects = dto.Subjects?.Select(c => new Subject
                {
                    SubjectName = c.SubjectName
                }).ToList(),
            };
        }
    }
}
