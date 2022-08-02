using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tp_Final_1.Models;
using Tp_Final_1.Data;
using Microsoft.EntityFrameworkCore;

namespace Tp_Final_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyContext _context;
        public HomeController(MyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            _context.post.Include(p => p.Tag).Load();
            _context.tags.Include(t => t.Post).Load();
        
            var postContext = _context.post.Include(p => p.user);
            var comenContext = _context.comentarios;
            var usuariosContext = _context.usuarios;
            var userId = HttpContext.Session.GetInt32("_id");
            var amigosContext = _context.UsuarioAmigo;
            HttpContext.Session.GetString("_nombre");
            ViewData["Posts"] = _context.post.ToList();
            _ = usuariosContext != null ?
                ViewData["Usuario"] = usuariosContext.ToList() :
                ViewData["Usuario"] = Enumerable.Empty<string>();
            ViewData["Amigos"] = amigosContext.ToList();
            ViewData["Comentario"] = comenContext.ToList();
            return View();
        }

        public IActionResult IndexAdmin()
        {
            MyContext _context = new MyContext();
            var postContext = _context.post.Count();
            var usuariosContext = _context.usuarios.Count();
            var tagsContext = _context.tags.Count();
            ViewData["Usuarios"] = usuariosContext;
            ViewData["Post"] = postContext;
            ViewData["Tags"] = tagsContext;
            return View();
        }
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("_id");
            HttpContext.Session.Remove("_nombre");
            HttpContext.Session.Remove("_admin");
            return RedirectToAction("Index", "Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}