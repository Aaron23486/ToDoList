using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Data.Entities;

namespace ToDoList.Controllers
{
    public class TareasController : Controller
    {
        private readonly DataContext _context;

        public TareasController(DataContext context)
        {
            _context = context;
        }

        // GET: Tareas
        public async Task<IActionResult> Index()
        {
            // Recupera todas las tareas y sus usuarios asociados
            var tareas = await _context.Tareas
                                        .Include(t => t.Usuario) // Incluye los datos del usuario relacionado
                                        .ToListAsync();

            // Puedes agregar algo a ViewData si necesitas pasar más información
            ViewData["Title"] = "Lista de Tareas";  // Por ejemplo, para personalizar el título de la página

            // Retorna la vista con las tareas
            return View(tareas);
        }


        // GET: Tareas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas
                .Include(t => t.Usuario) // Cambiar a 'Tareas'
                .FirstOrDefaultAsync(m => m.TareaId == id); // Cambiar a 'TareaId'
            if (tarea == null)
            {
                return NotFound();
            }

            return View(tarea);
        }

        // GET: Tareas/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TareaId,Title,Description,DueDate")] Tarea tarea) // Cambiar a 'TareaId'
        {
            ModelState.Remove("Usuario"); // Relación con Usuario no necesaria aquí
            if (ModelState.IsValid)
            {
                _context.Add(tarea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tarea);
        }

        // Método GET para cargar la vista de edición
        public async Task<IActionResult> Edit(int id)
        {
            // Buscar la tarea en la base de datos
            var task = await _context.Tareas.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task); // Pasar el modelo a la vista
        }

        // Método POST para guardar los cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tarea tarea) // Cambié 'Task' a 'Tarea'
        {
            if (id != tarea.TareaId)
            {
                return BadRequest();
            }

            ModelState.Remove("Usuario");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tarea); // Usar el modelo con el nombre correcto
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TareaExists(tarea.TareaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(tarea); // Si hay errores, volver a mostrar la vista
        }




        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(e => e.TareaId == id);
        }



        // GET: Tareas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas
                .Include(t => t.Usuario) // Cambiar a 'Tareas'
                .FirstOrDefaultAsync(m => m.TareaId == id); // Cambiar a 'TareaId'
            if (tarea == null)
            {
                return NotFound();
            }

            return View(tarea);
        }

        // POST: Tareas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id); // Cambiar a 'Tareas'
            if (tarea != null)
            {
                _context.Tareas.Remove(tarea); // Cambiar a 'Tareas'
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int TareaId, bool IsCompleted)
        {
            var tarea = await _context.Tareas.FindAsync(TareaId);
            if (tarea != null)
            {
                tarea.IsCompleted = IsCompleted;
                _context.Update(tarea);
                await _context.SaveChangesAsync();
            }

            // Devuelve una respuesta JSON vacía para AJAX
            return Json(new { success = true });
        }



    }
}
