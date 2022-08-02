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
        private readonly MyContext _context;

        public LoginController(MyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Usuario usuario)
        {
            Usuario userPassCorrecto = null;
            Usuario[] userCorrecto = null;

            if (_context.usuarios != null)
            {
                userPassCorrecto = _context.usuarios.Where(x => x.email == usuario.email && x.password == usuario.password).SingleOrDefault();
                userCorrecto = _context.usuarios.Where(x => x.email == usuario.email).ToArray();

                Usuario user = usuario.usuarioExistente(usuario, userPassCorrecto, userCorrecto);

                if (user.email == "")
                {
                    TempData["Message"] = "Usuario no encontrado, favor de registrarse";
                    return View();
                }
                else if (user.bloqueado)
                {
                    _context.Update(user);
                    _context.SaveChanges();
                    TempData["Message"] = "Usuario bloqueado, contacte al administrador";
                    return View();
                }
                else if (user.intentosFallidos > 0)
                {
                    _context.Update(user);
                    _context.SaveChanges();
                    TempData["Message"] = "Password mal ingresada, intente de nuevo";
                    return View();
                }
                else if (user.isAdm == true)
                {

                    HttpContext.Session.SetInt32(SessionIdKey, user.id);
                    HttpContext.Session.SetString(SessionNyaKey, user.nombre + " " + user.apellido);
                    HttpContext.Session.SetString(SessionAdminKey, user.isAdm.ToString());
                    _context.Update(user);
                    _context.SaveChanges();
                    return RedirectToAction("IndexAdmin", "Home");
                }
                else
                {
                    HttpContext.Session.SetInt32(SessionIdKey, user.id);
                    HttpContext.Session.SetString(SessionNyaKey, user.nombre + " " + user.apellido);
                    HttpContext.Session.SetString(SessionAdminKey, user.isAdm.ToString());
                    _context.Update(user);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["Message"] = "Usuario no encontrado, favor de registrarse";
                return View();
            }
        }
    }
}
