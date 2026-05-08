using Microsoft.EntityFrameworkCore;
using StudentCrudAppWithEFCoreCodeFirst.Data;
using StudentCrudAppWithEFCoreCodeFirst.Dto;
using StudentCrudAppWithEFCoreCodeFirst.Models;

namespace StudentCrudAppWithEFCoreCodeFirst.Repository
{
    public class StudentRepository:IStudentRepository
    {
        private readonly AppDbContext _context;
        public StudentRepository(AppDbContext context)
        {
            _context=context;
        }
        public async Task<IEnumerable<Student>> GetAll()
        {
            return await _context.Students.Include(s => s.Courses)
              .Include(s => s.Address)
              .Include(s => s.Subjects)
              .ToListAsync();
        }
        public async Task<Student> GetById(int id)
        {
           return await _context.Students.
                Include(s => s.Courses)
                .Include(s => s.Address)
                .Include(s => s.Subjects)
                .FirstOrDefaultAsync(s => s.StudentId == id);
        }
        public async Task Add(Student student)
        {
            _context.Students.Add(student);
        }

        public async Task Update(Student student)
        {
            _context.Students.Update(student);
        }
        public async Task Delete(Student student)
        {
           
            _context.Students.Remove(student);
        }

        public async Task <List<Student>> GetStudentsByMarks(int minMarks) 
        {
            return await _context.Students.FromSqlRaw("select * from GET_STUDENTS_BY_MARKS({0})", minMarks).ToListAsync();
        }

        public async Task SaveAsync() 
        {
            await _context.SaveChangesAsync();
        }
    }
}
