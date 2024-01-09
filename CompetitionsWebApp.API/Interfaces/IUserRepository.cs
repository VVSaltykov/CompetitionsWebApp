using CompetitionsWebApp.Common.Models;
using CompetitionsWebApp.Common.ViewModels;

namespace CompetitionsWebApp.API.Interfaces
{
    public interface IUserRepository
    {
        Task AddNewUser(User user);
        Task<bool> UserIsInDatabase(RegisterViewModel registerViewModel);
        Task<User> GetUserByLoginModelAsync(LoginViewModel loginViewModel);
        Task<User> GetUserByIdAsync(int userId);
    }
}
