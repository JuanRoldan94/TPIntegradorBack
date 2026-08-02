using GestorDespacho.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPIntegradorBack.Data;
using TPIntegradorBack.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GestorDespacho.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Usuario)
                .Include(p => p.DetallePedido)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.PedidoId == id);

            if (pedido == null) return NotFound();

            return View(pedido);
        }

        public async Task<IActionResult> Crear()
        {
            var clientes = await _context.Clientes.Where(c => c.Activo).ToListAsync();

            Console.WriteLine(clientes.Count);
            ViewBag.Clientes = new SelectList(clientes, "ClienteId", "RazonSocial");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuscarProducto(int idProducto)
        {
            var producto = await _context.Productos.FindAsync(idProducto);

            if (producto == null)
            {
                return Json(new { exito = false, mensaje = "Producto no encontrado" });
            }

            return Json(new
            {
                exito = true,
                id = producto.Id,
                descripcion = producto.Descripcion,
                precio = producto.PrecioUnitario
            });
        }

        [HttpPost]
        public async Task<IActionResult> Confirmar([FromBody] ConfirmarPedido datosDelFrente)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                int? usuarioId = null;
                var claimUsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if(int.TryParse(claimUsuarioId, out int idParseado))
                {
                    usuarioId = idParseado;
                }
                int? direccionFinalId = null;

                if (datosDelFrente.DireccionId > 0)
                {
                    direccionFinalId = datosDelFrente.DireccionId;
                }

                if (usuarioId == null)
                {
                    return Json(new { exito = false, mensaje = "Usuario no autenticaodo" });
                }

                var nuevoPedido = new Pedido
                {
                    ClienteId = datosDelFrente.ClienteId,
                    UsuarioId = usuarioId.Value,
                    Fecha = DateTime.Now,
                    MontoTotal = datosDelFrente.MontoTotal,
                    Confirmado = true,
                    DireccionId = direccionFinalId
                };

                _context.Pedidos.Add(nuevoPedido);
                await _context.SaveChangesAsync();

                foreach (var item in datosDelFrente.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(item.ProductoId);

                    if (producto == null)
                    {
                        await transaction.RollbackAsync();
                        return Json(new { exito = false, mensaje = $"El producto con ID {item.ProductoId} no existe." });
                    }

                    if (producto.Stock < item.Cantidad)
                    {
                        await transaction.RollbackAsync();
                        return Json(new {exito = false, mensaje = $"No hay stock suficiente para '{producto.Descripcion}'. Stock disponible: {producto.Stock}." });
                    }

                    producto.Stock -= item.Cantidad;

                    var detalle = new TPIntegradorBack.Models.DetallePedido
                    {
                        PedidoId = nuevoPedido.PedidoId,
                        ProductoId = item.ProductoId,
                        Cantidad = item.Cantidad,
                        CostoUnitarioHistorico = item.CostoUnitario,
                        Subtotal = item.Cantidad * item.CostoUnitario
                    };

                    _context.DetallePedidos.Add(detalle);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new { exito = true, mensaje = "Pedido despachado correctamente" });

            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return Json(new { exito = false, mensaje = "Error al procesar el despacho", e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(string buscarCliente, string buscarUsuario, int? clienteId)
        {
            var query = _context.Pedidos
                .AsNoTracking()
                .Include(p => p.Cliente)
                .Include(p => p.Usuario)
                .Include(p => p.DetallePedido)
                .AsQueryable();

            if (clienteId.HasValue && clienteId > 0)
            {
                query = query.Where(p => p.ClienteId == clienteId.Value);
            }
            else
            {
                if (!string.IsNullOrEmpty(buscarCliente))
                {
                    query = query.Where(p => p.Cliente.RazonSocial.Contains(buscarCliente));
                }
            }

            if (!string.IsNullOrEmpty(buscarUsuario))
            {
                query = query.Where(p => p.Usuario.NombreUsuario.Contains(buscarUsuario));
            }

            var pedidos = await query.ToListAsync();

            ViewBag.FiltroCliente = buscarCliente;
            ViewBag.FiltroUsuario = buscarUsuario;
            ViewBag.ClienteId = clienteId;

            return View(pedidos);
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerDireccionesCliente(int clienteId)
        {
            var direcciones = await _context.Direcciones
                .AsNoTracking()
                .Where(d => d.ClienteId == clienteId)
                .Select(d => new    
                {
                    id = d.DireccionId,
                    texto = $"{d.Calle} {d.Numero}, {d.Localidad}"
                })
                .ToListAsync();

            direcciones.Insert(0, new { id = 0, texto = "Retira en Local" });
            return Json(direcciones);

        }
    }
}