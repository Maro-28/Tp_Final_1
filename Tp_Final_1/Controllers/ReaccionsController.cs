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
    public class ReaccionsController : Controller
    {
        private readonly MyContext _context;

        public ReaccionsController(MyContext context)
        {
            _context = context;
        }

        // GET: Reaccions
        public async Task<IActionResult> Index()
        {
            var myContext = _context.reacciones.Include(r => r.post).Include(r => r.usuario);
            return View(await myContext.ToListAsync());
        }

        // POST: Reaccions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,tipoReaccion,idPost,idUser")] Reaccion reaccion)
        {
            int idI = (int)HttpContext.Session.GetInt32("_id");
            reaccion.agregarReaccion(reaccion, idI);

            Reaccion[] exist = _context.reacciones.AsNoTracking().Where(r => r.idUser == idI &&
                                                                        r.idPost == reaccion.idPost).ToArray();
           if (exist.Length > 0)
            {
                if (exist[0].tipoReaccion == reaccion.tipoReaccion)
                {
                    TempData["Message"] = "Usted ya reacciono de esta forma";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Message"] = "Su reaccion fue modificada";
                    await this.Edit(exist[0].id, reaccion);
                }
            }
            else
            {
                TempData["Message"] = "Su reaccion fue ingresada";
                _context.Add(reaccion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");

        }

        // POST: Reaccions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,tipoReaccion,idPost,idUser")] Reaccion reaccion)
        {
            reaccion.id = id;
            _context.Update(reaccion);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: Reaccions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.reacciones == null)
            {
                return NotFound();
            }

            var reaccion = await _context.reacciones
                .Include(r => r.post)
                .Include(r => r.usuario)
                .FirstOrDefaultAsync(m => m.id == id);
            if (reaccion == null)
            {
                return NotFound();
            }

            return View(reaccion);
        }

        // POST: Reaccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.reacciones == null)
            {
                return Problem("Entity set 'MyContext.reacciones'  is null.");
            }
            var reaccion = await _context.reacciones.FindAsync(id);
            if (reaccion != null)
            {
                _context.reacciones.Remove(reaccion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReaccionExists(int id)
        {
            return (_context.reacciones?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
