using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using server.Controllers;
using server.Repository;
using Microsoft.AspNetCore.Mvc;

namespace server.Tests.Controllers
{
    public class UsersControllerTests
    {
        [Fact]
        public async Task Post_NullRequest_ReturnsBadRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("users_post_null").Options;
            using var ctx = new AppDbContext(options);
            var controller = new UsersController(ctx);

            var result = await controller.Post(null);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User information is required", bad.Value);
        }

        [Fact]
        public async Task Post_MissingUser_ReturnsBadRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("users_post_missing_user").Options;
            using var ctx = new AppDbContext(options);
            var controller = new UsersController(ctx);

            var request = new UsuarioRequest { User = "", Pass = "p" };
            var result = await controller.Post(request);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User is required", bad.Value);
        }

        [Fact]
        public async Task Post_MissingPass_ReturnsBadRequest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("users_post_missing_pass").Options;
            using var ctx = new AppDbContext(options);
            var controller = new UsersController(ctx);

            var request = new UsuarioRequest { User = "u", Pass = "" };
            var result = await controller.Post(request);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Password field is required", bad.Value);
        }

        [Fact]
        public async Task Post_ValidRequest_InsertsAndReturnsOk()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("users_post_valid").Options;
            using var ctx = new AppDbContext(options);

            var controller = new UsersController(ctx);

            var request = new UsuarioRequest { User = "testuser", Pass = "pwd" };
            var result = await controller.Post(request);

            Assert.IsType<OkResult>(result);
            var saved = await ctx.Usuarios.FirstOrDefaultAsync(u => u.User == "testuser");
            Assert.NotNull(saved);
            Assert.Equal("testuser", saved.User);
        }

        private class ThrowingDbContext : AppDbContext
        {
            public ThrowingDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            {
                throw new Exception("save error");
            }
        }

        [Fact]
        public async Task Post_SaveChangesFailure_Returns500WithMessage()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("users_post_error").Options;
            using var ctx = new ThrowingDbContext(options);

            var controller = new UsersController(ctx);

            var request = new UsuarioRequest { User = "u", Pass = "p" };
            var result = await controller.Post(request);

            var status = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, status.StatusCode);
            Assert.Equal("save error", status.Value);
        }
    }

    // Helper query provider which strips FromSqlRaw method calls from expression trees
    internal class FromSqlStrippingQueryProvider<TEntity> : IQueryProvider
    {
        private readonly IQueryProvider _inner;
        private readonly IQueryable<TEntity> _data;

        public FromSqlStrippingQueryProvider(IQueryProvider inner, IQueryable<TEntity> data)
        {
            _inner = inner;
            _data = data;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            var cleaned = new FromSqlRemovingVisitor().Visit(expression);
            return _inner.CreateQuery(cleaned);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            var cleaned = new FromSqlRemovingVisitor().Visit(expression);
            return _inner.CreateQuery<TElement>(cleaned);
        }

        public object? Execute(Expression expression)
        {
            var cleaned = new FromSqlRemovingVisitor().Visit(expression);
            return _inner.Execute(cleaned);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var cleaned = new FromSqlRemovingVisitor().Visit(expression);
            return _inner.Execute<TResult>(cleaned);
        }
    }

    // Async enumerable & enumerator used by tests
    internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
        public TestAsyncEnumerable(Expression expression) : base(expression) { }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;
        public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;
        public T Current => _inner.Current;
        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return default;
        }
        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());
    }

    // Removes the FromSqlRaw/FromSqlInterpolated method call wrapper so LINQ-to-Objects provider can execute the query
    internal class FromSqlRemovingVisitor : ExpressionVisitor
    {
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == "FromSqlRaw" || node.Method.Name == "FromSqlInterpolated")
            {
                // assume first argument is the source IQueryable (the DbSet)
                if (node.Arguments != null && node.Arguments.Count > 0)
                {
                    return Visit(node.Arguments[0]);
                }
            }

            return base.VisitMethodCall(node);
        }
    }
}

