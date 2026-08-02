using TPIntegradorBack.Data;
using TPIntegradorBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class UsuariosController : Controller
{
    private readonly ApplicationDbContext _context;

    public UsuariosController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var usuarios = await _context.Usuarios
            .Where(u => u.Activo == true)
            .Include(u => u.Pedidos)
            .ToListAsync();
        return View(usuarios);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios
            .Include(u => u.Pedidos)
            .FirstOrDefaultAsync(m => m.UsuarioId == id);
        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }


    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("UsuarioId,NombreUsuario,Contrasenia,Nombre,Apellido,Role")] Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(usuario.Contrasenia))
            {
                usuario.Contrasenia = BCrypt.Net.BCrypt.HashPassword(usuario.Contrasenia);
            }
            _context.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(usuario);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }
        return View(usuario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("UsuarioId,NombreUsuario,Contrasenia,Nombre,Apellido,Role")] Usuario usuario)
    {
        if (id != usuario.UsuarioId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.UsuarioId))
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
        return View(usuario);
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(m => m.UsuarioId == id);
        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario != null)
        {
            usuario.Activo = false;
            _context.Usuarios.Update(usuario);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UsuarioExists(int? id)
    {
        return _context.Usuarios.Any(e => e.UsuarioId == id);
    }
}

