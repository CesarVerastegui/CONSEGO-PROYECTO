using System.Security.Claims;
using CONSEGO.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CONSEGO.Filters
{
    public class UsuarioActivoFilter : IAsyncAuthorizationFilter
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UsuarioActivoFilter(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
                return;

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return;

            var userId = int.Parse(userIdClaim.Value);

            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var usuario = await dbContext.Usuarios.FindAsync(userId);
            if (usuario == null || !usuario.Activo)
            {
                await context.HttpContext.SignOutAsync();
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }
    }
}