using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using StudentCrudAppWithEFCoreCodeFirst.Data;
using StudentCrudAppWithEFCoreCodeFirst.Dto;
using StudentCrudAppWithEFCoreCodeFirst.Mapper;
using StudentCrudAppWithEFCoreCodeFirst.Models;
using StudentCrudAppWithEFCoreCodeFirst.Repository;
using StudentCrudAppWithEFCoreCodeFirst.Services;

namespace StudentCrudAppWithEFCoreCodeFirst.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //private readonly AppDbContext _appDbContext;
        private readonly IStudentService _studentService;
        private readonly IStudentRepository _appDbContext;
        private readonly IStudentGradeService _studentGradeService;
        private readonly IAppLogger _logger;

        public StudentsController(IStudentRepository appDbContext,IStudentService studentService,IStudentGradeService studentGradeService,IAppLogger logger)
        {
            _appDbContext = appDbContext;
            _studentService=studentService;
            _studentGradeService=studentGradeService;
            _logger = logger;
        }
        [EnableRateLimiting("fixed")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
        {
            _logger.Log("Fetching all Students");
            // var students = await _studentService.GetAll();
            var students = await _appDbContext.GetAll();
            var result = students.Select(s => StudentMapper.ToDto(s, _studentGradeService.GetGrade(s.StudentMarks)));

            return Ok(result);
        }
        [EnableRateLimiting("sliding")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student =await _appDbContext.GetById(id);
                if (student == null)
            {
                return NotFound();
            }
            var grade = _studentGradeService.GetGrade(student.StudentMarks);
            return Ok(StudentMapper.ToDto(student,_studentGradeService.GetGrade(student.StudentMarks)));
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(StudentCreateDto student)
        {
            var studentCreated = StudentMapper.ToEntity(student);

            await _appDbContext.Add(studentCreated);
            await _appDbContext.SaveAsync();
            _logger.Log($"Student Created : {student.StudentName}");
            return Ok(studentCreated);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> UpdateStudent(int id, StudentUpdateDto updatedStudent)
        {
            if (id != updatedStudent.StudentId)
            {
                return BadRequest();
            }
           var student=await _appDbContext.GetById(id);
            if (student == null)
                return NotFound();
            student.StudentName = updatedStudent.StudentName;
            student.StudentAge = updatedStudent.StudentAge;
            student.StudentMarks = updatedStudent.StudentMarks;

            await _appDbContext.Update(student);
            await _appDbContext.SaveAsync();

          
            return Ok(updatedStudent);

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {

            var student = await _appDbContext.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            _appDbContext.Delete(student);
            await _appDbContext.SaveAsync();
            return NoContent();

        }

        [HttpGet("filter/{marks}")]
        public async Task<IActionResult> GetStudentByMarks(int marks) 
        {
            var data= await _appDbContext.GetStudentsByMarks(marks);
            return Ok(data);
        }
        [HttpGet("error")]
        public IActionResult TestErrorMiddleware()
        {
            throw new Exception("Checking exception middleware from controller");
        }

    }
}
