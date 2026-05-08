using StudentCrudAppWithEFCoreCodeFirst.Dto;
using StudentCrudAppWithEFCoreCodeFirst.Models;

namespace StudentCrudAppWithEFCoreCodeFirst.Repository
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAll();
        Task <Student> GetById(int id);
     
        Task<List<Student>> GetStudentsByMarks(int minMarks);
        Task Add(Student student);
        Task Update(Student student);
        Task Delete(Student student);
        Task SaveAsync();
    }
}
