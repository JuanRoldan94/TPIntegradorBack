using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TPIntegradorBack.Filters
{
    public class AuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var controller = context.RouteData.Values["controller"]?.ToString();

            // Permitir acceso al Login sin sesión
            if (controller == "Login")
                return;

            var usuarioId = context.HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                context.Result = new RedirectToActionResult(
                    "Index",
                    "Login",
                    null
                );
            }
        }
    }
}