using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackSistema.Controllers
{
    [ApiController]
    [Route("Proyectos")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProyectosController : ControllerBase
    {
        private const string FilePath = @"C:\Users\Alber\source\repos\GestionDeProyecto\AdministrarProyecto.json";

        [HttpGet("Leer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetProyectosList()
        {
            try
            {
                List<Proyectos> proyectos = ObtenerListaProyectos();
                return Ok(proyectos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = $"Error al obtener la lista de Proyectos: {ex.Message}" });
            }

        }


        [HttpPost("Agregar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CrearProyecto(Proyectos proyectos)
        {
            try
            {
                List<Proyectos> proyecto = ObtenerListaProyectos();

                if (string.IsNullOrEmpty(proyectos.Nombre) || string.IsNullOrEmpty(proyectos.Descripcion) || proyectos.fechaInicio == default || proyectos.fechaFinalizacion == null)
                {
                    return BadRequest(new { Success = false, Message = "Los campos obligatorios deben tener valores válidos." });
                }

                proyecto.Add(proyectos);

                string json = JsonConvert.SerializeObject(proyecto, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(FilePath, json);

                return Ok(new { Success = true, Message = "Proyecto creado con éxito", Tarea = proyectos });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = $"Error al crear el Proyecto: {ex.Message}" });
            }
        }


        [HttpPut("Actualizar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(string nombre, [FromBody] Proyectos updatedProyecto)
        {
            try
            {
                List<Proyectos> proyectos = ObtenerListaProyectos();

                var existingProyecto = proyectos.FirstOrDefault(t => t.Nombre == nombre);

                if (existingProyecto != null)
                {
                    existingProyecto.Nombre = updatedProyecto.Nombre;
                    existingProyecto.Descripcion = updatedProyecto.Descripcion;
                    existingProyecto.fechaInicio = updatedProyecto.fechaInicio;
                    existingProyecto.fechaFinalizacion = updatedProyecto.fechaFinalizacion;
                 
                    string json = JsonConvert.SerializeObject(proyectos, Newtonsoft.Json.Formatting.Indented);
                    System.IO.File.WriteAllText(FilePath, json);

                    return Ok(new { Success = true, Message = "Proyecto actualizada con éxito", Tarea = existingProyecto });
                }

                return NotFound(new { Success = false, Message = "Proyecto no encontrada." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = $"Error al actualizar el Proyecto: {ex.Message}" });
            }
        }


        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(string nombre)
        {
            try
            {
                List<Proyectos> proyectos = ObtenerListaProyectos();

                var proyectoToRemove = proyectos.FirstOrDefault(t => t.Nombre == nombre);

                if (proyectoToRemove != null)
                {
                    proyectos.Remove(proyectoToRemove);

                    string json = JsonConvert.SerializeObject(proyectos, Newtonsoft.Json.Formatting.Indented);
                    System.IO.File.WriteAllText(FilePath, json);

                    return Ok(new { Success = true, Message = "Proyecto eliminado con éxito" });
                }

                return NotFound(new { Success = false, Message = "Proyecto no encontrada." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = $"Error al eliminar el Proyecto: {ex.Message}" });
            }
        }


        private List<Proyectos> ObtenerListaProyectos()
        {
            string jsonContent = System.IO.File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<Proyectos>>(jsonContent);
        }
    }
}
