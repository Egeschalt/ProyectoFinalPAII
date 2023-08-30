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
    public class DevolucionesController : Controller
    {
        private readonly BibliotecaContext _context;

        public DevolucionesController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: Devoluciones
        public async Task<IActionResult> Index()
        {
            var bibliotecaContext = _context.Devoluciones.Include(d => d.Prestamo).Include(d => d.Prestamo.Cliente);
            return View(await bibliotecaContext.ToListAsync());
        }

        // GET: Devoluciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Devoluciones == null)
            {
                return NotFound();
            }

            var devoluciones = await _context.Devoluciones
                .Include(d => d.Prestamo)
                .Include(d => d.Prestamo.Cliente)
                .FirstOrDefaultAsync(m => m.DevolucionesId == id);
            if (devoluciones == null)
            {
                return NotFound();
            }
            return View(devoluciones);
            
        }

        // GET: Devoluciones/Create
        public IActionResult Create()
        {
            ViewData["PrestamoId"] = new SelectList(_context.Prestamo, "PrestamosId", "PrestamosId");
            return View();
        }

        // POST: Devoluciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DevolucionesId,PrestamoId,FechaDevolucion")] Devoluciones devoluciones)
        {
            
            if (ModelState.IsValid)
            {
                
                _context.Add(devoluciones);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PrestamoId"] = new SelectList(_context.Prestamo, "PrestamosId", "PrestamosId", devoluciones.PrestamoId);
            return View(devoluciones);
        }

        // GET: Devoluciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Devoluciones == null)
            {
                return NotFound();
            }

            var devoluciones = await _context.Devoluciones.FindAsync(id);
            if (devoluciones == null)
            {
                return NotFound();
            }
            ViewData["PrestamoId"] = new SelectList(_context.Prestamo, "PrestamosId", "PrestamosId", devoluciones.PrestamoId);
            return View(devoluciones);
        }

        // POST: Devoluciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DevolucionesId,PrestamoId,FechaDevolucion")] Devoluciones devoluciones)
        {
            if (id != devoluciones.DevolucionesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(devoluciones);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DevolucionesExists(devoluciones.DevolucionesId))
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
            ViewData["PrestamoId"] = new SelectList(_context.Prestamo, "PrestamosId", "PrestamosId", devoluciones.PrestamoId);
            return View(devoluciones);
        }

        // GET: Devoluciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Devoluciones == null)
            {
                return NotFound();
            }

            var devoluciones = await _context.Devoluciones
                .Include(d => d.Prestamo)
                .FirstOrDefaultAsync(m => m.DevolucionesId == id);
            if (devoluciones == null)
            {
                return NotFound();
            }

            return View(devoluciones);
        }

        // POST: Devoluciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Devoluciones == null)
            {
                return Problem("Entity set 'BibliotecaContext.Devoluciones'  is null.");
            }
            var devoluciones = await _context.Devoluciones.FindAsync(id);
            if (devoluciones != null)
            {
                _context.Devoluciones.Remove(devoluciones);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DevolucionesExists(int id)
        {
          return _context.Devoluciones.Any(e => e.DevolucionesId == id);
        }
    }
}
