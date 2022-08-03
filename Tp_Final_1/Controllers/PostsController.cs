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
            _context.post.Include(p => p.user);
            _context.post.Include(p => p.Tag).Load();
            _context.tags.Include(t => t.Post).Load();
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


        //GET: Posts/Busqueda/5
        public ActionResult Busqueda(string contenido, string tags, DateTime fechai, DateTime fechaf, string nombre, string apellido)
        {
            var consultaPost = _context.post;
            var consultaTags = _context.tags;
            var consultaPostTags = _context.PostsTags;
            var consultaUser = _context.usuarios.ToList();
            string fDesde = fechai.Date.ToString("dd/MM/yyyy");
            string hDesde = fechaf.Date.ToString("dd/MM/yyyy");
            Tag tag = null;
            if(consultaTags != null && tags != null)
             tag = (Tag)consultaTags.Where(x => x.palabra == tags).First();

            IQueryable<Post> query = (DbSet<Post>)consultaPost;
            if(contenido != null)
                query = query.Where(x => x.contenido == contenido);
            if (fDesde != "01/01/0001")
                query = query.Where(x => x.fecha >= fechai);
            if (hDesde != "01/01/0001")
                query = query.Where(x => x.fecha <= fechaf);
            if (nombre != null)
                query = query.Where(x => x.user.nombre == nombre);
            if (apellido != null)
                query = query.Where(x => x.user.apellido == apellido);
            if(tag != null)
                query = query.Where(x => x.Tag.Contains(tag));
                           
            return View(query.ToList());
        }


        private bool PostExists(int id)
        {
            return (_context.post?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
