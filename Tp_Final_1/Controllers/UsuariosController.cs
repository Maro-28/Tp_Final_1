using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tp_Final_1.Data;
using Tp_Final_1.Models;

namespace Tp_Final_1.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly MyContext _context;

        public UsuariosController(MyContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return _context.usuarios != null ?
                        View(await _context.usuarios.ToListAsync()) :
                        Problem("Entity set 'MyContext.usuarios'  is null.");
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuarios
                .FirstOrDefaultAsync(m => m.id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,dni,nombre,apellido,email,password,intentosFallidos,bloqueado,isAdm")] Usuario usuario)
        {
            int mailExiste = _context.usuarios.Where(x => x.email == usuario.email).Count();
            Usuario user = usuario.crearUsuario(usuario,mailExiste);

            if (user.email == "")
            {
                TempData["MessagemailExiste"] = "Mail ya registrado, intente con otro";
                return RedirectToAction(nameof(Create));
            }
            else
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["MessageLoger"] = "Usuario registrado";
                return RedirectToAction(nameof(Index), "Login");
            }
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuarios.FindAsync(id);
            usuario.password = "";
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("id,dni,nombre,apellido,email,password,intentosFallidos,bloqueado,isAdm")] Usuario usuario, string current, string newP)
        {            
            Usuario[] userList = _context.usuarios.AsNoTracking().Where(x => x.password == current).ToArray();
            Usuario user = usuario.editarUsuario(usuario, newP, userList);
            if(user.password == "@*@")
            {
                TempData["Message"] = "La nueva password no coincide";
                return View() ; 
            }
            else if(user.password == "")
            {
                TempData["Message"] = "Password actual incorrecta";
                return View();
            }
            else
            {                                
                _context.Update(user);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Usuario modificado correctamente";
            }
            return RedirectToAction(nameof(Index), "Home");
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuarios
                .FirstOrDefaultAsync(m => m.id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.usuarios == null)
            {
                return Problem("Entity set 'MyContext.usuarios'  is null.");
            }
            var usuario = await _context.usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.usuarios.Remove(usuario);
            }

            
            await _context.SaveChangesAsync();
            if (HttpContext.Session.GetString("_admin").ToString() == "True")
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                HttpContext.Session.Remove("_id");
                HttpContext.Session.Remove("_nombre");
                HttpContext.Session.Remove("_admin");
                return RedirectToAction("Index", "Login");
            }
                
        }

        private bool UsuarioExists(int id)
        {
            return (_context.usuarios?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
