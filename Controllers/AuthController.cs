using Microsoft.AspNetCore.Mvc;

namespace BackSistema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly List<User> users;

        public AuthController()
        {

            users = new List<User>
            {
                new User { Username = "admin", Password = "admin" }
            };
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginRequest request)
        {
            var user = users.Find(u => u.Username == request.Username && u.Password == request.Password);

            if (user != null)
            {
                return Ok(new { Message = "Acceso Permitido" });
            }

            return BadRequest(new { Message = "Nombre de usuario o contraseña incorrectos" });
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
