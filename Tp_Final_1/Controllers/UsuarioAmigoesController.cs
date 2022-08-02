﻿using System;
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

        // GET: UsuarioAmigoes
        public async Task<IActionResult> Index()
        {
            var myContext = _context.UsuarioAmigo.Include(u => u.amigo).Include(u => u.user);
            return View(await myContext.ToListAsync());
        }

        // GET: UsuarioAmigoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UsuarioAmigo == null)
            {
                return NotFound();
            }

            var usuarioAmigo = await _context.UsuarioAmigo
                .Include(u => u.amigo)
                .Include(u => u.user)
                .FirstOrDefaultAsync(m => m.idAmigo == id);
            if (usuarioAmigo == null)
            {
                return NotFound();
            }

            return View(usuarioAmigo);
        }

        // GET: UsuarioAmigoes/Create
        public IActionResult Create()
        {
            ViewData["idUser"] = new SelectList(_context.usuarios, "id", "id");
            ViewData["idAmigo"] = new SelectList(_context.usuarios, "id", "id");
            return View();
        }

        // POST: UsuarioAmigoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idUser,idAmigo")] UsuarioAmigo usuarioAmigo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuarioAmigo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["idUser"] = new SelectList(_context.usuarios, "id", "id", usuarioAmigo.idUser);
            ViewData["idAmigo"] = new SelectList(_context.usuarios, "id", "id", usuarioAmigo.idAmigo);
            return View(usuarioAmigo);
        }

        // GET: UsuarioAmigoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UsuarioAmigo == null)
            {
                return NotFound();
            }

            var usuarioAmigo = await _context.UsuarioAmigo.FindAsync(id);
            if (usuarioAmigo == null)
            {
                return NotFound();
            }
            ViewData["idUser"] = new SelectList(_context.usuarios, "id", "id", usuarioAmigo.idUser);
            ViewData["idAmigo"] = new SelectList(_context.usuarios, "id", "id", usuarioAmigo.idAmigo);
            return View(usuarioAmigo);
        }

        // POST: UsuarioAmigoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idUser,idAmigo")] UsuarioAmigo usuarioAmigo)
        {
            if (id != usuarioAmigo.idAmigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarioAmigo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioAmigoExists(usuarioAmigo.idAmigo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["idUser"] = new SelectList(_context.usuarios, "id", "id", usuarioAmigo.idUser);
            ViewData["idAmigo"] = new SelectList(_context.usuarios, "id", "id", usuarioAmigo.idAmigo);
            return View(usuarioAmigo);
        }

        // GET: UsuarioAmigoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UsuarioAmigo == null)
            {
                return NotFound();
            }

            var usuarioAmigo = await _context.UsuarioAmigo
                .Include(u => u.amigo)
                .Include(u => u.user)
                .FirstOrDefaultAsync(m => m.idAmigo == id);
            if (usuarioAmigo == null)
            {
                return NotFound();
            }

            return View(usuarioAmigo);
        }

        // POST: UsuarioAmigoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UsuarioAmigo == null)
            {
                return Problem("Entity set 'MyContext.UsuarioAmigo'  is null.");
            }
            var usuarioAmigo = await _context.UsuarioAmigo.FindAsync(id);
            if (usuarioAmigo != null)
            {
                _context.UsuarioAmigo.Remove(usuarioAmigo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioAmigoExists(int id)
        {
            return (_context.UsuarioAmigo?.Any(e => e.idAmigo == id)).GetValueOrDefault();
        }
    }
}