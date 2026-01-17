using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Repository;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public UsersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet(Name = "Usuarios")]
        public async Task<ActionResult<IEnumerable<UsuarioResponse>>> Get()
        {
            var usuarios = await _dbContext.Usuarios.FromSqlRaw("EXEC [dbo].[GetAllUsers]").ToListAsync();
            return Ok(usuarios.Select(u => new UsuarioResponse() { Id = u.Id, User = u.User, FechaCreacion = u.FechaCreacion}));
        }

        [HttpPost(Name = "Usuarios")]
        public async Task<ActionResult> Post(UsuarioRequest usuarioRequest)
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

            try
            {
                var insertedUser = await _dbContext.Usuarios.AddAsync(new Usuario()
                {
                    User = usuarioRequest.User,
                    Pass = usuarioRequest.Pass,
                    FechaCreacion = DateTime.Now
                });

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok();
        }

    }
}
