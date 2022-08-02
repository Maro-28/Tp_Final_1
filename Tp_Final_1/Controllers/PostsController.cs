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
    public class PostsController : Controller
    {
        private readonly MyContext _context;

        public PostsController(MyContext context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var myContext = _context.post.Include(p => p.user);
            return View(await myContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.post == null)
            {
                return NotFound();
            }

            var post = await _context.post
                .Include(p => p.user)
                .FirstOrDefaultAsync(m => m.id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["idUser"] = new SelectList(_context.usuarios, "id", "id");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,idUser,contenido,fecha")] Post post, string tags)
        {
            int idI = (int)HttpContext.Session.GetInt32("_id");
            post.postear(post, idI);
            _context.Add(post);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Posteo realizado";

            if (tags == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                post.agregarTags(post.id, tags);
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.post == null)
            {
                return NotFound();
            }

            var post = await _context.post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["idUser"] = new SelectList(_context.usuarios, "id", "id", post.idUser);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,idUser,contenido,fecha")] Post post)
        {

            _context.Update(post);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Post modificado";

            var hola = HttpContext.Session.GetString("_admin");

            if (HttpContext.Session.GetString("_admin") == "True")
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Home");

        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.post == null)
            {
                return NotFound();
            }

            var post = await _context.post
                .Include(p => p.user)
                .FirstOrDefaultAsync(m => m.id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.post.FindAsync(id);
            _context.post.Remove(post);

            await _context.SaveChangesAsync();

            TempData["Message"] = "Post eliminado";

            if (HttpContext.Session.GetString("_admin") == "True")
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Home");
        }

        private bool PostExists(int id)
        {
            return (_context.post?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
