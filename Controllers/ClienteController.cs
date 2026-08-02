using TPIntegradorBack.Data;
using TPIntegradorBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPIntegradorBack.ViewModels;

public class ClienteController : Controller
{
    private readonly ApplicationDbContext _context;

    public ClienteController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var clientesActivos = await _context.Clientes
            .Include(c => c.Direcciones)
            .OrderByDescending(c => c.Activo)
            .ToListAsync();

        return View(clientesActivos);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cliente = await _context.Clientes
            .Include(c => c.Direcciones)
            .Include(c => c.Pedidos)
            .FirstOrDefaultAsync(m => m.ClienteId == id);
        if (cliente == null)
        {
            return NotFound();
        }

        return View(cliente);
    }


    public IActionResult Create()
    {
        return View(new ClienteYDireccionViewModel());
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClienteYDireccionViewModel model)
    {
        if (ModelState.IsValid)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var nuevoCliente = new Cliente
                {
                    RazonSocial = model.RazonSocial,
                    DNI = model.DNI,
                    Telefono = model.Telefono,
                    Activo = model.Activo
                };

                _context.Clientes.Add(nuevoCliente);
                await _context.SaveChangesAsync();

                if(!string.IsNullOrWhiteSpace(model.Calle) || model.Numero != null || !string.IsNullOrWhiteSpace(model.Localidad))
                {
                    var nuevaDireccion = new Direccion
                    {
                        ClienteId = nuevoCliente.ClienteId,
                        Calle = model.Calle,
                        Numero = model.Numero,
                        Localidad = model.Localidad
                    };
                    _context.Direcciones.Add(nuevaDireccion);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "Ocurrió un error grave al intentar guardar el cliente y su dirección.");
            }
        }
        return View(model);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("ClienteId,RazonSocial,DNI,Telefono,Activo")] Cliente cliente)
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


    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
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


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente != null)
        {
            cliente.Activo = false;
            _context.Clientes.Update(cliente);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ClienteExists(int? id)
    {
        return _context.Clientes.Any(e => e.ClienteId == id);
    }
}

