using Microsoft.AspNetCore.Identity;
using ToDoList.Data.Entities;

namespace ToDoList.Helpers
{
    public interface IUserHelper
    {
        Task<Usuario> GetUserAsync(string email);

        Task<IdentityResult> AddUserAsync(Usuario user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(Usuario user, string roleName);

        Task<bool> IsUserInRoleAsync(Usuario user, string roleName);
    }
}
