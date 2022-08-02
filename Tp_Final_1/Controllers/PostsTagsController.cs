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
    public class PostsTagsController : Controller
    {
        private readonly MyContext _context;

        public PostsTagsController(MyContext context)
        {
            _context = context;
        }

        // GET: PostsTags
        public async Task<IActionResult> Index()
        {
            var myContext = _context.PostsTags.Include(p => p.Post).Include(p => p.Tag);
            return View(await myContext.ToListAsync());
        }

        // GET: PostsTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PostsTags == null)
            {
                return NotFound();
            }

            var postsTags = await _context.PostsTags
                .Include(p => p.Post)
                .Include(p => p.Tag)
                .FirstOrDefaultAsync(m => m.idPost == id);
            if (postsTags == null)
            {
                return NotFound();
            }

            return View(postsTags);
        }

        // GET: PostsTags/Create
        public IActionResult Create()
        {
            ViewData["idPost"] = new SelectList(_context.post, "id", "id");
            ViewData["idTag"] = new SelectList(_context.tags, "id", "id");
            return View();
        }

        // POST: PostsTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idPost,idTag")] PostsTags postsTags)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postsTags);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["idPost"] = new SelectList(_context.post, "id", "id", postsTags.idPost);
            ViewData["idTag"] = new SelectList(_context.tags, "id", "id", postsTags.idTag);
            return View(postsTags);
        }

        // GET: PostsTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PostsTags == null)
            {
                return NotFound();
            }

            var postsTags = await _context.PostsTags.FindAsync(id);
            if (postsTags == null)
            {
                return NotFound();
            }
            ViewData["idPost"] = new SelectList(_context.post, "id", "id", postsTags.idPost);
            ViewData["idTag"] = new SelectList(_context.tags, "id", "id", postsTags.idTag);
            return View(postsTags);
        }

        // POST: PostsTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idPost,idTag")] PostsTags postsTags)
        {
            if (id != postsTags.idPost)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postsTags);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostsTagsExists(postsTags.idPost))
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
            ViewData["idPost"] = new SelectList(_context.post, "id", "id", postsTags.idPost);
            ViewData["idTag"] = new SelectList(_context.tags, "id", "id", postsTags.idTag);
            return View(postsTags);
        }

        // GET: PostsTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PostsTags == null)
            {
                return NotFound();
            }

            var postsTags = await _context.PostsTags
                .Include(p => p.Post)
                .Include(p => p.Tag)
                .FirstOrDefaultAsync(m => m.idPost == id);
            if (postsTags == null)
            {
                return NotFound();
            }

            return View(postsTags);
        }

        // POST: PostsTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PostsTags == null)
            {
                return Problem("Entity set 'MyContext.PostsTags'  is null.");
            }
            var postsTags = await _context.PostsTags.FindAsync(id);
            if (postsTags != null)
            {
                _context.PostsTags.Remove(postsTags);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostsTagsExists(int id)
        {
            return (_context.PostsTags?.Any(e => e.idPost == id)).GetValueOrDefault();
        }
    }
}
