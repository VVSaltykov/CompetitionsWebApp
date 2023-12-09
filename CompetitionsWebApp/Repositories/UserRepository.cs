using CompetitionsWebApp.Models;
using CompetitionsWebApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CompetitionsWebApp.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationContext appContext;

        public UserRepository(ApplicationContext appContext)
        {
            this.appContext = appContext;
        }
        public async Task AddNewUser(User user)
        {
            appContext.Users.Add(user);
            await appContext.SaveChangesAsync();
        }
        public async Task<bool> UserIsInDatabase(RegisterViewModel registerViewModel)
        {
            User user = await appContext.Users.FirstOrDefaultAsync(a => a.Login == registerViewModel.Login);
            if (user == null)
            {
                return false;
            }
            return true;
        }
        public async Task<User> GetUserByLoginModelAsync(LoginViewModel loginViewModel)
        {
            User user = await appContext.Users.FirstOrDefaultAsync(a => a.Login == loginViewModel.Login && a.Password == loginViewModel.Password);
            return user;
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await appContext.FindAsync<User>(userId);
            return user;
        }
    }
}
