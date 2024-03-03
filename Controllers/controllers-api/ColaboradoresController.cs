using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackSistema.Controllers
{
    [ApiController]
    [Route("Colaboradores")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ColaboradoresController : ControllerBase
    {
        private const string FilePath = @"C:\Users\Alber\source\repos\GestionDeProyecto\Colaboradores.json";

        [HttpGet("Leer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetTareasList()
        {
            try
            {
                List<Colaborador> colaborador = ObtenerListaColaboradores();
                return Ok(colaborador);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = $"Error al obtener la lista de Colaboradores: {ex.Message}" });
            }
        }


        [HttpPost("Agregar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CrearColaborador(Colaborador colaboradorr)
        {
            try
            {
                List<Colaborador> colaborador = ObtenerListaColaboradores();

                if (string.IsNullOrEmpty(colaboradorr.Nombre) || string.IsNullOrEmpty(colaboradorr.Apellido) || string.IsNullOrEmpty(colaboradorr.Telefono) || string.IsNullOrEmpty(colaboradorr.Correo))
                {
                    return BadRequest(new { Success = false, Message = "Los campos obligatorios deben tener valores válidos." });
                }

                colaborador.Add(colaboradorr);

                string json = JsonConvert.SerializeObject(colaborador, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(FilePath, json);

                return Ok(new { Success = true, Message = "Colaborador creado con éxito", Colaboradores = colaboradorr });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = $"Error al crear el Colaborador: {ex.Message}" });
            }
        }


        [HttpPut("Actualizar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(string nombre, [FromBody] Colaborador updatedColaborador)
        {
            try
            {
                List<Colaborador> colaborador = ObtenerListaColaboradores();

                var existingColaborador = colaborador.FirstOrDefault(t => t.Nombre == nombre);

                if (existingColaborador != null)
                {
                    existingColaborador.Nombre = updatedColaborador.Nombre;
                    existingColaborador.Apellido = updatedColaborador.Apellido;
                    existingColaborador.Telefono = updatedColaborador.Telefono;
                    existingColaborador.Correo = updatedColaborador.Correo;
                   

                    string json = JsonConvert.SerializeObject(colaborador, Newtonsoft.Json.Formatting.Indented);
                    System.IO.File.WriteAllText(FilePath, json);

                    return Ok(new { Success = true, Message = "Colaborador actualizado con éxito", Colaborador = existingColaborador });
                }

                return NotFound(new { Success = false, Message = "Colaborador no encontrada." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = $"Error al actualizar el Colaborador: {ex.Message}" });
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
                List<Colaborador> colaborador = ObtenerListaColaboradores();

                var colaboradorToRemove = colaborador.FirstOrDefault(t => t.Nombre == nombre);

                if (colaboradorToRemove != null)
                {
                    colaborador.Remove(colaboradorToRemove);

                    string json = JsonConvert.SerializeObject(colaborador, Newtonsoft.Json.Formatting.Indented);
                    System.IO.File.WriteAllText(FilePath, json);

                    return Ok(new { Success = true, Message = "Colaborador eliminada con éxito" });
                }

                return NotFound(new { Success = false, Message = "Colaborador no encontrada." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = $"Error al eliminar el Colaborador: {ex.Message}" });
            }
        }


        private List<Colaborador> ObtenerListaColaboradores()
        {
            string jsonContent = System.IO.File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<Colaborador>>(jsonContent);
        }
    }
}
