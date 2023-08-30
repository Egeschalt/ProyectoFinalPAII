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
    public class EditorialController : Controller
    {
        private readonly BibliotecaContext _context;

        public EditorialController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: Editorial
        public async Task<IActionResult> Index(string buscaNombre)
        {
            ViewData["Filtrar"] = buscaNombre;
            var clientes = from lista in _context.Editorial select lista;

            //método de busqueda de nombre
            if (!string.IsNullOrEmpty(buscaNombre))
            {

                clientes = clientes.Where(campo => campo.EditorialName.Contains(buscaNombre));
            }

            return View(await clientes.ToListAsync());
        }

        // GET: Editorial/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Editorial == null)
            {
                return NotFound();
            }

            var editorial = await _context.Editorial
                .FirstOrDefaultAsync(m => m.EditorialId == id);
            if (editorial == null)
            {
                return NotFound();
            }

            return View(editorial);
        }

        // GET: Editorial/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Editorial/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EditorialId,EditorialName")] Editorial editorial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(editorial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(editorial);
        }

        // GET: Editorial/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Editorial == null)
            {
                return NotFound();
            }

            var editorial = await _context.Editorial.FindAsync(id);
            if (editorial == null)
            {
                return NotFound();
            }
            return View(editorial);
        }

        // POST: Editorial/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EditorialId,EditorialName")] Editorial editorial)
        {
            if (id != editorial.EditorialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editorial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditorialExists(editorial.EditorialId))
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
            return View(editorial);
        }

        // GET: Editorial/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Editorial == null)
            {
                return NotFound();
            }

            var editorial = await _context.Editorial
                .FirstOrDefaultAsync(m => m.EditorialId == id);
            if (editorial == null)
            {
                return NotFound();
            }

            return View(editorial);
        }

        // POST: Editorial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Editorial == null)
            {
                return Problem("Entity set 'BibliotecaContext.Editorial'  is null.");
            }
            var editorial = await _context.Editorial.FindAsync(id);
            if (editorial != null)
            {
                _context.Editorial.Remove(editorial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EditorialExists(int id)
        {
          return _context.Editorial.Any(e => e.EditorialId == id);
        }
    }
}
