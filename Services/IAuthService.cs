using StudentCrudAppWithEFCoreCodeFirst.Models;

namespace StudentCrudAppWithEFCoreCodeFirst.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string username, string password);
        Task RegisterAsync(User user);
    }
}
