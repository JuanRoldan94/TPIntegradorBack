using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using TPIntegradorBack.Data;
using TPIntegradorBack.Models;

public class DireccionController : Controller
{
    private readonly ApplicationDbContext _context;

    public DireccionController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: DIRECCIONS
    public async Task<IActionResult> Index(int? clienteId)    
    {
        var query = _context.Direcciones
            .Include(d => d.Cliente)
            .AsQueryable();
        
        if (clienteId.HasValue)
        {
            query = query.Where(d => d.ClienteId == clienteId.Value);
        }

        return View(await query.ToListAsync());
    }

    // GET: DIRECCIONS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var direccion = await _context.Direcciones
            .FirstOrDefaultAsync(m => m.DireccionId == id);
        if (direccion == null)
        {
            return NotFound();
        }

        return View(direccion);
    }

    // GET: DIRECCIONS/Create
    public async Task<IActionResult> Create()
    {
        var clientes = await _context.Clientes.Where(c => c.Activo).ToListAsync();
        ViewBag.Clientes = new SelectList(clientes, "ClienteId", "RazonSocial");
        return View();
    }

    // POST: DIRECCIONS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DireccionId,Calle,Numero,Localidad,ClienteId")] Direccion direccion)
    {
        ModelState.Remove("Cliente");

        var clienteExiste = await _context.Clientes.AnyAsync(c => c.ClienteId == direccion.ClienteId);
        if (!clienteExiste)
        {
            ModelState.AddModelError("ClienteId", "Debe seleccionar un cliente válido.");
        }

        if (ModelState.IsValid)
        {
            _context.Add(direccion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        var clientes = await _context.Clientes.Where(c => c.Activo).ToListAsync();
        ViewBag.Clientes = new SelectList(clientes, "ClienteId", "RazonSocial");
        return View(direccion);
    }

    // GET: DIRECCIONS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var direccion = await _context.Direcciones.FindAsync(id);
        if (direccion == null)
        {
            return NotFound();
        }
        return View(direccion);
    }

    // POST: DIRECCIONS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? direccionid, [Bind("DireccionId,Calle,Numero,Localidad")] Direccion direccion)
    {
        if (direccionid != direccion.DireccionId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(direccion);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DireccionExists(direccion.DireccionId))
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
        return View(direccion);
    }

    // GET: DIRECCIONS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var direccion = await _context.Direcciones
            .FirstOrDefaultAsync(m => m.DireccionId == id);
        if (direccion == null)
        {
            return NotFound();
        }

        return View(direccion);
    }

    // POST: DIRECCIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var direccion = await _context.Direcciones.FindAsync(id);
        if (direccion != null)
        {
            _context.Direcciones.Remove(direccion);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DireccionExists(int? id)
    {
        return _context.Direcciones.Any(e => e.DireccionId == id);
    }
}
