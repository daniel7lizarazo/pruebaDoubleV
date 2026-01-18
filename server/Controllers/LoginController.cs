using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Repository;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public LoginController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost(Name = "Login")]
        public async Task<ActionResult> Login(UsuarioRequest usuarioRequest)
        {
            if(usuarioRequest == null)
            {
                return BadRequest("User information is required");
            }
            if(String.IsNullOrEmpty(usuarioRequest.User))
            {
                return BadRequest("User is required");
            }
            if(String.IsNullOrEmpty(usuarioRequest.Pass))
            {
                return BadRequest("Password field is required");
            }

            var insertedUser = await _dbContext.Usuarios
                .SingleOrDefaultAsync<Usuario>(u => u.User == usuarioRequest.User && u.Pass == usuarioRequest.Pass);

            if(insertedUser is null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok();
        }

    }
}
