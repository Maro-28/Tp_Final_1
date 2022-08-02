using Microsoft.AspNetCore.Mvc;
using Tp_Final_1.Data;
using Tp_Final_1.Models;

namespace Tp_Final_1.Controllers
{
    public class LoginController : Controller
    {
        public const string SessionIdKey = "_id";
        public const string SessionNyaKey = "_nombre";
        public const string SessionAdminKey = "_admin";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Usuario usuario)
        {
            Usuario user = usuario.usuarioExistente(usuario);

            HttpContext.Session.SetInt32(SessionIdKey, user.id);
            HttpContext.Session.SetString(SessionNyaKey, user.nombre + " " + user.apellido);
            HttpContext.Session.SetString(SessionAdminKey, user.isAdm.ToString());

            if (user.email == "")
            {
                TempData["Message"] = "Usuario no encontrado, favor de registrarse";
                return View();
            } 
            else if (user.bloqueado)
            {
                TempData["Message"] = "Usuario bloqueado, contacte al administrador";
                return View();
            }
            else if (user.intentosFallidos > 0)
            {
                TempData["Message"] = "Password mal ingresada, intente de nuevo";
                return View();
            }
            else if(user.isAdm == true)
            {
                return RedirectToAction("IndexAdmin", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
