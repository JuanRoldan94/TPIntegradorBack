using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TPIntegradorBack.Filters
{
    public class AuthFilter : IAuthorizationFilter
    {
        /*
         Valida que el usuario este registrado: 
            - si esta registrado: lo deja pasar
            - si no esta registrado: le vuelve a mostrar la pantalla de Login
         */
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Identifica a que seccion de la pagina quiere ir el usuario
            var controller = context.RouteData.Values["controller"]?.ToString();

            // Permitir acceso al Login sin sesión
            if (controller == "Login")
                return;
 
            // Se obtiene el Id del usuario
            var usuarioId = context.HttpContext.Session.GetInt32("UsuarioId");
            
            // Si el usuario no esta registrado lo redirige de nuevo al Login
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