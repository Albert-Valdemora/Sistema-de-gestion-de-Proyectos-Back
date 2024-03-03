using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BackSistema.Controllers
{
    [ApiController]
    [Route("Tareas")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TareasController : ControllerBase
    {
        private const string FilePath = @"C:\Users\Alber\source\repos\GestionDeProyecto\AdministrarTareas.json";

        [HttpGet("Leer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetTareasList()
        {
            try
            {
                List<Tarea> tareas = ObtenerListaTareas();
                return Ok(tareas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = $"Error al obtener la lista de Tareas: {ex.Message}" });
            }
        }


        [HttpPost("Agregar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CrearTarea(Tarea tarea)
        {
            try
            {
                List<Tarea> tareas = ObtenerListaTareas();

                if (string.IsNullOrEmpty(tarea.Proyecto) || string.IsNullOrEmpty(tarea.Descripcion) || string.IsNullOrEmpty(tarea.Estado) || tarea.FechaVencimiento == default)
                {
                    return BadRequest(new { Success = false, Message = "Los campos obligatorios deben tener valores válidos." });
                }

                tareas.Add(tarea);

                string json = JsonConvert.SerializeObject(tareas, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(FilePath, json);

                return Ok(new { Success = true, Message = "Tarea creada con éxito", Tarea = tarea });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = $"Error al crear la tarea: {ex.Message}" });
            }
        }


        [HttpPut("Actualizar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(string proyecto, [FromBody] Tarea updatedTarea)
        {
            try
            {
                List<Tarea> tareas = ObtenerListaTareas();

                var existingTarea = tareas.FirstOrDefault(t => t.Proyecto == proyecto);

                if (existingTarea != null)
                {
                    existingTarea.Proyecto = updatedTarea.Proyecto;
                    existingTarea.Descripcion = updatedTarea.Descripcion;
                    existingTarea.Estado = updatedTarea.Estado;
                    existingTarea.FechaVencimiento = updatedTarea.FechaVencimiento;
                    existingTarea.ColaboradorAsignado = updatedTarea.ColaboradorAsignado;

                    string json = JsonConvert.SerializeObject(tareas, Newtonsoft.Json.Formatting.Indented);
                    System.IO.File.WriteAllText(FilePath, json);

                    return Ok(new { Success = true, Message = "Tarea actualizada con éxito", Tarea = existingTarea });
                }

                return NotFound(new { Success = false, Message = "Tarea no encontrada." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = $"Error al actualizar la tarea: {ex.Message}" });
            }
        }


        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(string proyecto)
        {
            try
            {
                List<Tarea> tareas = ObtenerListaTareas();

                var tareaToRemove = tareas.FirstOrDefault(t => t.Proyecto == proyecto);

                if (tareaToRemove != null)
                {
                    tareas.Remove(tareaToRemove);

                    string json = JsonConvert.SerializeObject(tareas, Newtonsoft.Json.Formatting.Indented);
                    System.IO.File.WriteAllText(FilePath, json);

                    return Ok(new { Success = true, Message = "Tarea eliminada con éxito" });
                }

                return NotFound(new { Success = false, Message = "Tarea no encontrada." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = $"Error al eliminar la tarea: {ex.Message}" });
            }
        }


        private List<Tarea> ObtenerListaTareas()
        {
            string jsonContent = System.IO.File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<Tarea>>(jsonContent);
        }
    }
}
