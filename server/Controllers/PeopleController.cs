using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Repository;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public PeopleController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet(Name = "people")]
        public async Task<ActionResult<IEnumerable<Usuario>>> Get()
        {
            var people = await _dbContext.Personas.ToListAsync();
            return Ok(people);
        }

        [HttpPost(Name = "people")]
        public async Task<ActionResult> Post(PersonaRequest personaRequest)
        {
            if(personaRequest == null)
            {
                return BadRequest("La información de la persona es requerida");
            }
            if(String.IsNullOrEmpty(personaRequest.Nombres))
            {
                return BadRequest($"El campo {nameof(personaRequest.Nombres)} es requerido");
            }
            if(String.IsNullOrEmpty(personaRequest.Apellidos))
            {
                return BadRequest($"El campo {nameof(personaRequest.Apellidos)} es requerido");
            }
            if(String.IsNullOrEmpty(personaRequest.Email))
            {
                return BadRequest($"El campo {nameof(personaRequest.Email)} es requerido");
            }
            if(String.IsNullOrEmpty(personaRequest.NumeroIdentificacion))
            {
                return BadRequest($"El campo {nameof(personaRequest.NumeroIdentificacion)} es requerido");
            }
            if(String.IsNullOrEmpty(personaRequest.TipoIdentificacion))
            {
                return BadRequest($"El campo {nameof(personaRequest.TipoIdentificacion)} es requerido");
            }

            try
            {
                var insertedUser = await _dbContext.Personas.AddAsync(new Persona()
                {
                    Nombres = personaRequest.Nombres,
                    Apellidos = personaRequest.Apellidos,
                    Email = personaRequest.Email,
                    NumeroIdentificacion = personaRequest.NumeroIdentificacion,
                    TipoIdentificacion = personaRequest.TipoIdentificacion
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
