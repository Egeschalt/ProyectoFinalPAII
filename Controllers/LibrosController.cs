using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalPAII.Data;
using ProyectoFinalPAII.Models;

namespace ProyectoFinalPAII.Controllers
{
    public class LibrosController : Controller
    {
        private readonly BibliotecaContext _context;

        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }
     
        // GET: Libros
        public async Task<IActionResult> Index(string buscaNombre, string configOrdenar)
        {
            ViewData["OrdenarNombre"] = string.IsNullOrEmpty(configOrdenar) ? "nombre_desc" : "";
            ViewData["OrdenarFecha"] = configOrdenar == "fecha_asc" ? "fecha_desc" : "fecha_asc";
            ViewData["Filtrar"] = buscaNombre;
            var libros = from lista in _context.Libros.Include(m => m.Autor).Include(m => m.Editorial) select lista;

            //método de busqueda de nombre
            if (!string.IsNullOrEmpty(buscaNombre))
            {

                libros = libros.Where(campo => campo.LibrosName.Contains(buscaNombre));
            }

            //Método para ordenar fecha y nombres
            switch (configOrdenar)
            {
                case "nombre_desc":
                    libros = libros.OrderByDescending(campo => campo.LibrosName);
                    break;
                case "fecha_asc":
                    libros = libros.OrderBy(campo => campo.FechaLanzamiento);
                    break;
                case "fecha_desc":
                    libros = libros.OrderByDescending(campo => campo.FechaLanzamiento);
                    break;
                default:
                    libros = libros.OrderBy(campo => campo.LibrosName);
                    break;
            }

            return View(await libros.ToListAsync());
        }
       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .Include(m => m.Autor)
                .Include(m => m.Editorial)
                .FirstOrDefaultAsync(m => m.LibrosId == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            ViewData["AutorId"] = new SelectList(_context.Autores, "AutorId", "Nombre");
            ViewData["EditorialId"] = new SelectList(_context.Editorial, "EditorialId", "EditorialName");
            return View();
        }

        // POST: Libros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LibrosId,LibrosName,AutorId,EditorialId,FechaLanzamiento")] Libros libros)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libros);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AutorId"] = new SelectList(_context.Autores, "AutorId", "Nombre", libros.AutorId);
            ViewData["EditorialId"] = new SelectList(_context.Editorial, "EditorialId", "EditorialName", libros.EditorialId);
            return View(libros);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libros = await _context.Libros.FindAsync(id);
            if (libros == null)
            {
                return NotFound();
            }
            ViewData["AutorId"] = new SelectList(_context.Autores, "AutorId", "Nombre", libros.AutorId);
            ViewData["EditorialId"] = new SelectList(_context.Editorial, "EditorialId", "EditorialName", libros.EditorialId);
            return View(libros);
        }

        // POST: Libros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LibrosId,LibrosName,AutorId,EditorialId,FechaLanzamiento")] Libros libros)
        {
            if (id != libros.LibrosId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libros);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibrosExists(libros.LibrosId))
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
            ViewData["AutorId"] = new SelectList(_context.Autores, "AutorId", "AutorId", libros.AutorId);
            ViewData["EditorialId"] = new SelectList(_context.Editorial, "EditorialId", "EditorialId", libros.EditorialId);
            return View(libros);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libros = await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Editorial)
                .FirstOrDefaultAsync(m => m.LibrosId == id);
            if (libros == null)
            {
                return NotFound();
            }

            return View(libros);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Libros == null)
            {
                return Problem("Entity set 'BibliotecaContext.Libros'  is null.");
            }
            var libros = await _context.Libros.FindAsync(id);
            if (libros != null)
            {
                _context.Libros.Remove(libros);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibrosExists(int id)
        {
          return _context.Libros.Any(e => e.LibrosId == id);
        }
    }
}
