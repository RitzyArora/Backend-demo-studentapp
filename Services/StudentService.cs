using Microsoft.EntityFrameworkCore;
using StudentCrudAppWithEFCoreCodeFirst.Data;
using StudentCrudAppWithEFCoreCodeFirst.Models;

namespace StudentCrudAppWithEFCoreCodeFirst.Services
{
    public class StudentService:IStudentService// scoped
    {
        private readonly AppDbContext _context;
        public StudentService(AppDbContext context)
        {
            _context=context;
        }
        public async Task<IEnumerable<Student>> GetAll() 
        {
            return await _context.Students.ToListAsync();
        }
    }
}
