using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using ToDoList.Enums;

namespace ToDoList.Data.Entities
{
    public class Usuario : IdentityUser
    {
        public string FirstName { get; set; } // Nombre del usuario
        public string LastName { get; set; }  // Apellido del usuario
        public string Address { get; set; }   // Dirección del usuario
        public string Document { get; set; }  // Documento de identificación
        public UserType UserType { get; set; } // Tipo de usuario
        public ICollection<Tarea> Tareas { get; set; } // Relación con tareas
    }
}
