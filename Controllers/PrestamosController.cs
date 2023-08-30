using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalPAII.Data;
using ProyectoFinalPAII.Models;
using OfficeOpenXml;
using System.IO;

namespace ProyectoFinalPAII.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly BibliotecaContext _context;

        public PrestamosController(BibliotecaContext context)
        {
            _context = context;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        public IActionResult GenerateExcelReport()
        {
            var prestamos = _context.Prestamo.Include(p => p.Cliente).Include(p => p.Libro).ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Prestamos");

                worksheet.Cells.LoadFromCollection(prestamos, true);

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrestamosReport.xlsx");
            }
        }
        // GET: Prestamos
        public async Task<IActionResult> Index(string configOrdenar, string filtroCliente )
        {
            ViewData["OrdenarFecha"] = configOrdenar == "fecha_asc" ? "fecha_desc" : "fecha_asc";
            var prestamo = from lista in _context.Prestamo.Include(m => m.Cliente).Include(m => m.Libro) select lista;



            //Método para ordenar fecha y nombres
            switch (configOrdenar)
            {
                case "nombre_desc":
                    prestamo = prestamo.OrderByDescending(campo => campo.Cliente.NombreCliente);
                    break;
                case "fecha_asc":
                    prestamo = prestamo.OrderBy(campo => campo.FechaPrestamo);
                    break;
                case "fecha_desc":
                    prestamo = prestamo.OrderByDescending(campo => campo.FechaPrestamo);
                    break;
                default:
                    prestamo = prestamo.OrderBy(campo => campo.Cliente.NombreCliente);
                    break;
            }

            if (!string.IsNullOrEmpty(filtroCliente))
            {
                prestamo = prestamo.Where(p => p.Cliente.NombreCliente.Contains(filtroCliente));
                ViewData["FiltroCliente"] = filtroCliente;
            }

            return View(await prestamo.ToListAsync());
        }

        // GET: Prestamos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Prestamo == null)
            {
                return NotFound();
            }

            var prestamos = await _context.Prestamo
                .Include(p => p.Cliente)
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(m => m.PrestamosId == id);
            if (prestamos == null)
            {
                return NotFound();
            }

            return View(prestamos);
        }

        // GET: Prestamos/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "NombreCliente");
            ViewData["LibroId"] = new SelectList(_context.Libros, "LibrosId", "LibrosName");
            var prestamo = new Prestamos
            {
                Estado = "Activo"
            };
            return View(prestamo);
        }

        // POST: Prestamos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PrestamosId,ClienteId,LibroId,FechaPrestamo,Estado")] Prestamos prestamos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prestamos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "NombreCliente", prestamos.ClienteId);
            ViewData["LibroId"] = new SelectList(_context.Libros, "LibrosId", "LibrosName", prestamos.LibroId);
            return View(prestamos);
        }

        // GET: Prestamos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Prestamo == null)
            {
                return NotFound();
            }

            var prestamos = await _context.Prestamo.FindAsync(id);
            if (prestamos == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "ClienteId", prestamos.ClienteId);
            ViewData["LibroId"] = new SelectList(_context.Libros, "LibrosId", "LibrosId", prestamos.LibroId);
            return View(prestamos);
        }

        // POST: Prestamos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PrestamosId,ClienteId,LibroId,FechaPrestamo,Estado")] Prestamos prestamos)
        {
            if (id != prestamos.PrestamosId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestamos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamosExists(prestamos.PrestamosId))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "ClienteId", prestamos.ClienteId);
            ViewData["LibroId"] = new SelectList(_context.Libros, "LibrosId", "LibrosId", prestamos.LibroId);
            return View(prestamos);
        }

        // GET: Prestamos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Prestamo == null)
            {
                return NotFound();
            }

            var prestamos = await _context.Prestamo
                .Include(p => p.Cliente)
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(m => m.PrestamosId == id);
            if (prestamos == null)
            {
                return NotFound();
            }

            return View(prestamos);
        }

        // POST: Prestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Prestamo == null)
            {
                return Problem("Entity set 'BibliotecaContext.Prestamo'  is null.");
            }
            var prestamos = await _context.Prestamo.FindAsync(id);
            if (prestamos != null)
            {
                _context.Prestamo.Remove(prestamos);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrestamosExists(int id)
        {
          return _context.Prestamo.Any(e => e.PrestamosId == id);
        }
    }
}
