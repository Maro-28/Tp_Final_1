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
    public class UsuarioAmigoesController : Controller
    {
        private readonly MyContext _context;

        public UsuarioAmigoesController(MyContext context)
        {
            _context = context;
        }

        // POST: UsuarioAmigoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idUser,idAmigo")] UsuarioAmigo usuarioAmigo)
        {
            _context.Add(usuarioAmigo);
            await _context.SaveChangesAsync();
            UsuarioAmigo ua = usuarioAmigo.agregarAmigoUsuario(usuarioAmigo);
            _context.Add(ua);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Amigo agregado";
            return RedirectToAction("Index", "Home");
        }

        // POST: UsuarioAmigoes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("idUser,idAmigo")] UsuarioAmigo usuarioAmigo)
        {
            
            _context.Remove(usuarioAmigo);
            await _context.SaveChangesAsync();
            UsuarioAmigo ua = usuarioAmigo.EliminarAmigo(usuarioAmigo);
            _context.Remove(ua);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Amigo eliminado";
            return RedirectToAction("Index", "Home");
        }
    }
}
