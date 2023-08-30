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
    public class AutorController : Controller
    {
        private readonly BibliotecaContext _context;

        public AutorController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: Autor

        public async Task<IActionResult> Index(string buscaNombre, string configOrdenar, int? opcion)
        {
            ViewData["OrdenarNombre"] = string.IsNullOrEmpty(configOrdenar) ? "nombre_desc" : "";
            ViewData["Nombre"] = configOrdenar == "nac_asc" ? "nac_desc" : "nac_asc";
            ViewData["Filtrar"] = buscaNombre;
            var autor = from lista in _context.Autores select lista;

            //método de busqueda de nombre
            if (!string.IsNullOrEmpty(buscaNombre))
            {

                autor = autor.Where(campo => campo.Nombre.Contains(buscaNombre));
            }

            switch (configOrdenar)
            {
                case "nombre_desc":
                    autor = autor.OrderByDescending(campo => campo.Nombre);
                    break;
                case "nac_asc":
                    autor = autor.OrderBy(campo => campo.Nacionalidad);
                    break;
                case "nac_desc":
                    autor = autor.OrderByDescending(campo => campo.Nacionalidad);
                    break;
                default:
                    autor = autor.OrderBy(campo => campo.Nombre);
                    break;
            }
            IQueryable<Autor> datosFiltrados = _context.Autores;
            if (opcion.HasValue)
            {
                switch (opcion.Value)
                {
                    case 1:
                        datosFiltrados = datosFiltrados.Where(c => c.Nacionalidad.Contains("Colombia"));
                        break;
                    case 2:
                        datosFiltrados = datosFiltrados.Where(c => c.Nacionalidad.Contains("Peru"));
                        break;
                    case 3:
                        datosFiltrados = datosFiltrados.Where(c => c.Nacionalidad.Contains("Mexic"));
                        break;
                    case 4:
                        datosFiltrados = datosFiltrados.Where(c => c.Nacionalidad.Contains("Chile"));
                        break;
                    case 5:
                        datosFiltrados = datosFiltrados.Where(c => c.Nacionalidad.Contains("Argent"));
                        break;
                    default:; break;
                }
                return View(datosFiltrados.ToList());

            }

            return View(await autor.ToListAsync());
        }

        
        // GET: Autor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Autores == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores
                .FirstOrDefaultAsync(m => m.AutorId == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // GET: Autor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutorId,Nombre,Apellido,Nacionalidad")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        // GET: Autor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Autores == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // POST: Autor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AutorId,Nombre,Apellido,Nacionalidad")] Autor autor)
        {
            if (id != autor.AutorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.AutorId))
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
            return View(autor);
        }

        // GET: Autor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Autores == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores
                .FirstOrDefaultAsync(m => m.AutorId == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // POST: Autor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Autores == null)
            {
                return Problem("Entity set 'BibliotecaContext.Autores'  is null.");
            }
            var autor = await _context.Autores.FindAsync(id);
            if (autor != null)
            {
                _context.Autores.Remove(autor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id)
        {
            return _context.Autores.Any(e => e.AutorId == id);
        }
    }
}
