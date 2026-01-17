using Microsoft.EntityFrameworkCore;
using server.Controllers;
using server.Repository;

namespace server.Tests.Controllers
{
    public class PeopleControllerTests
    {
        private static DbContextOptions<AppDbContext> CreateNewContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        }

        [Fact]
        public async Task GetActionResult_ReturnsAllPeople()
        {
            var options = CreateNewContextOptions("people_get_all");
            using (var ctx = new AppDbContext(options))
            {
                ctx.Personas.AddRange(
                new Persona { Nombres = "John", Apellidos = "Doe", Email = "john@example.com", NumeroIdentificacion = "1", TipoIdentificacion = "CC" },
                new Persona { Nombres = "Jane", Apellidos = "Smith", Email = "jane@example.com", NumeroIdentificacion = "2", TipoIdentificacion = "CC" }
                );
                await ctx.SaveChangesAsync();
            }

            using (var ctx = new AppDbContext(options))
            {
                var controller = new PeopleController(ctx);
                var result = await controller.Get();

                Assert.NotNull(result);
                var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);
                var list = Assert.IsAssignableFrom<IEnumerable<Persona>>(okResult.Value);
                Assert.Equal(2, list.Count());
            }
        }

        [Fact]
        public async Task Post_NullRequest_ReturnsBadRequest()
        {
            var options = CreateNewContextOptions("people_post_null");
            using var ctx = new AppDbContext(options);
            var controller = new PeopleController(ctx);

            var result = await controller.Post(null);

            var bad = Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
            Assert.Equal("La información de la persona es requerida", bad.Value);
        }

        [Theory]
        [InlineData("Nombres")]
        [InlineData("Apellidos")]
        [InlineData("Email")]
        [InlineData("NumeroIdentificacion")]
        [InlineData("TipoIdentificacion")]
        public async Task Post_MissingRequiredField_ReturnsBadRequest(string missingField)
        {
            var options = CreateNewContextOptions($"people_post_missing_{missingField}");
            using var ctx = new AppDbContext(options);
            var controller = new PeopleController(ctx);

            var request = new PersonaRequest
            {
                Nombres = "Name",
                Apellidos = "Last",
                Email = "a@b.com",
                NumeroIdentificacion = "123",
                TipoIdentificacion = "CC"
            };

            // make the specified field empty
            switch (missingField)
            {
                case "Nombres":
                    request.Nombres = string.Empty;
                    break;
                case "Apellidos":
                    request.Apellidos = string.Empty;
                    break;
                case "Email":
                    request.Email = string.Empty;
                    break;
                case "NumeroIdentificacion":
                    request.NumeroIdentificacion = string.Empty;
                    break;
                case "TipoIdentificacion":
                    request.TipoIdentificacion = string.Empty;
                    break;
            }

            var result = await controller.Post(request);

            var bad = Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
            Assert.Equal($"El campo {missingField} es requerido", bad.Value);
        }

        [Fact]
        public async Task Post_ValidRequest_InsertsAndReturnsOk()
        {
            var options = CreateNewContextOptions("people_post_valid");
            using var ctx = new AppDbContext(options)
            {
            };

            var controller = new PeopleController(ctx);

            var request = new PersonaRequest
            {
                Nombres = "Alice",
                Apellidos = "Wonder",
                Email = "alice@wonder.land",
                NumeroIdentificacion = "777",
                TipoIdentificacion = "CC"
            };

            var result = await controller.Post(request);

            Assert.IsType<Microsoft.AspNetCore.Mvc.OkResult>(result);

            // Verify inserted
            var saved = await ctx.Personas.FirstOrDefaultAsync(p => p.Email == "alice@wonder.land");
            Assert.NotNull(saved);
            Assert.Equal("Alice", saved.Nombres);
        }

        private class ThrowingDbContext : AppDbContext
        {
            public ThrowingDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public override Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default)
            {
                throw new Exception("simulated db error");
            }
        }

        [Fact]
        public async Task Post_SaveChangesFailure_Returns500WithMessage()
        {
            var options = CreateNewContextOptions("people_post_error");
            using var ctx = new ThrowingDbContext(options);

            var controller = new PeopleController(ctx);

            var request = new PersonaRequest
            {
                Nombres = "Err",
                Apellidos = "Case",
                Email = "err@case.test",
                NumeroIdentificacion = "999",
                TipoIdentificacion = "CC"
            };

            var result = await controller.Post(request);

            var status = Assert.IsType<Microsoft.AspNetCore.Mvc.ObjectResult>(result);
            Assert.Equal(500, status.StatusCode);
            Assert.Equal("simulated db error", status.Value);
        }
    }
}