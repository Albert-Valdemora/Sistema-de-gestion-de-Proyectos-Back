using Microsoft.AspNetCore.Mvc;

namespace BackSistema.Controllers.Facades
{
    [ApiController]
    [Route("ProyectoFacade")]
    public class FacadeController : ControllerBase
    {
        private ColaboradoresController colaboradoresController;
        private ProyectosController proyectosController;
        private TareasController tareasController;

        public FacadeController()
        {
            colaboradoresController = new ColaboradoresController();
            proyectosController = new ProyectosController();
            tareasController = new TareasController();
        }

        [HttpGet("ListarProyectos")]
        public IActionResult ListarProyectos()
        {
            return proyectosController.GetProyectosList();
        }

        [HttpPost("CrearTarea")]
        public IActionResult CrearTarea(Tarea tarea)
        {
            
            return tareasController.CrearTarea(tarea);
        }

        [HttpGet("ListarTareas")]
        public IActionResult ListarTareas()
        {
            return tareasController.GetTareasList();
        }
    }
}
