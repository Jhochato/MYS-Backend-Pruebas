using AdmSeriesAnimadasAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdmSeriesAnimadasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AutenticacionService _authService;

        public UsuariosController(AutenticacionService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var token = _authService.Authenticate(request.Usuario, request.Password);

            if (token == null)
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });

            return Ok(new { token });
        }
    }

    public class LoginRequest
    {
        public string Usuario { get; set; }
        public string Password { get; set; }

    }
}
