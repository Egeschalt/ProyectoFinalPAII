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
    public class ClientesController : Controller
    {
        private readonly BibliotecaContext _context;

        public ClientesController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index(string buscaNombre, string configOrdenar)
        {
            ViewData["OrdenarNombre"] = string.IsNullOrEmpty(configOrdenar) ? "nombre_desc" : "";
            ViewData["OrdenarFecha"] = configOrdenar == "fecha_asc" ? "fecha_desc" : "fecha_asc";
            ViewData["Filtrar"] = buscaNombre;
            var clientes = from lista in _context.Clientes select lista;

            //método de busqueda de nombre
            if (!string.IsNullOrEmpty(buscaNombre))
            {

                clientes = clientes.Where(campo => campo.NombreCliente.Contains(buscaNombre));
            }

            //Método para ordenar fecha y nombres
            switch (configOrdenar)
            {
                case "nombre_desc":
                    clientes = clientes.OrderByDescending(campo => campo.NombreCliente);
                    break;
                case "fecha_asc":
                    clientes = clientes.OrderBy(campo => campo.FechaSuscripcion);
                    break;
                case "fecha_desc":
                    clientes = clientes.OrderByDescending(campo => campo.FechaSuscripcion);
                    break;
                default:
                    clientes = clientes.OrderBy(campo => campo.NombreCliente);
                    break;
            }
            return View(await clientes.ToListAsync());
        }


        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteId,NombreCliente,DNI,FechaSuscripcion")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClienteId,NombreCliente,DNI,FechaSuscripcion")] Cliente cliente)
        {
            if (id != cliente.ClienteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ClienteId))
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
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'BibliotecaContext.Clientes'  is null.");
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
          return _context.Clientes.Any(e => e.ClienteId == id);
        }
    }
}
