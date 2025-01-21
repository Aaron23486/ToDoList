using Microsoft.AspNetCore.Identity;
using ToDoList.Data.Entities;
using ToDoList.Enums;
using ToDoList.Helpers;

namespace ToDoList.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {

            await _context.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1997", "Aaron", "Gutierrez", "aaron20770@gmail.com", "61727742", "Costa Rica", UserType.Admin);
        }

        private async Task<Usuario> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            // Buscar el usuario por email
            Usuario user = await _userHelper.GetUserAsync(email);

            // Si el usuario no existe, crearlo
            if (user == null)
            {
                user = new Usuario
                {
                    UserName = email,
                    Email = email,
                    PhoneNumber = phone,
                    FirstName = firstName,
                    LastName = lastName,
                    Address = address,
                    Document = document,
                    UserType = userType,
                };

                // Crear el usuario con una contraseña predeterminada
                IdentityResult result = await _userHelper.AddUserAsync(user, "123456");

                // Validar que el usuario se haya creado correctamente
                if (result.Succeeded)
                {
                    // Asignar el rol correspondiente al usuario
                    await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                }
                else
                {
                    // Manejar errores en la creación del usuario
                    throw new Exception($"No se pudo crear el usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            return user;
        }


        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }
    }
}
