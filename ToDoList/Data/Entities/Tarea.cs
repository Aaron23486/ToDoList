using System.ComponentModel.DataAnnotations;

namespace ToDoList.Data.Entities
{
    public class Tarea
    {
        public int TareaId { get; set; }
        public string Title { get; set; } // Título de la tarea
        public string Description { get; set; } // Descripción opcional

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } // Fecha de vencimiento
        public bool IsCompleted { get; set; } // Estado de la tarea

        // Relación con el usuario
        public int UserId { get; set; } // Clave foránea
        public Usuario Usuario { get; set; } // Propiedad de navegación
    }

}
