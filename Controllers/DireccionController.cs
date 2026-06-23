
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Direcciones.ToListAsync());
    }

    // GET: DIRECCIONS/Details/5
    public async Task<IActionResult> Details(int? direccionid)
    {
        if (direccionid == null)
        {
            return NotFound();
        }

        var direccion = await _context.Direcciones
            .FirstOrDefaultAsync(m => m.DireccionId == direccionid);
        if (direccion == null)
        {
            return NotFound();
        }

        return View(direccion);
    }

    // GET: DIRECCIONS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DIRECCIONS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("DireccionId,Calle,Numero,Localidad")] Direccion direccion)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.IdentityUserId == userId);
        if (cliente == null)
        {
            ModelState.AddModelError("", "No se encontro un perfil de cliente para tu usuario actual.");
            return View(direccion);
        }

        direccion.ClienteId = cliente.ClienteId;
        ModelState.Remove("Cliente");
        ModelState.Remove("ClienteId");
        
        if (ModelState.IsValid)
        {
            _context.Add(direccion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(direccion);
    }

    // GET: DIRECCIONS/Edit/5
    public async Task<IActionResult> Edit(int? direccionid)
    {
        if (direccionid == null)
        {
            return NotFound();
        }

        var direccion = await _context.Direcciones.FindAsync(direccionid);
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
    public async Task<IActionResult> Delete(int? direccionid)
    {
        if (direccionid == null)
        {
            return NotFound();
        }

        var direccion = await _context.Direcciones
            .FirstOrDefaultAsync(m => m.DireccionId == direccionid);
        if (direccion == null)
        {
            return NotFound();
        }

        return View(direccion);
    }

    // POST: DIRECCIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? direccionid)
    {
        var direccion = await _context.Direcciones.FindAsync(direccionid);
        if (direccion != null)
        {
            _context.Direcciones.Remove(direccion);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DireccionExists(int? direccionid)
    {
        return _context.Direcciones.Any(e => e.DireccionId == direccionid);
    }
}
