using StudentCrudAppWithEFCoreCodeFirst.Models;

namespace StudentCrudAppWithEFCoreCodeFirst.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAll();
    }
}
