using GestorDespacho.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPIntegradorBack.Data;
using TPIntegradorBack.Models;

namespace GestorDespacho.Controllers
{
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Usuario)
                .Include(p => p.DetallePedido)
                .ToListAsync();
            return View(pedidos);
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
                int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

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
                    Confirmado = true
                };

                _context.Pedidos.Add(nuevoPedido);
                await _context.SaveChangesAsync();

                foreach (var item in datosDelFrente.Detalles)
                {
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
        public async Task<IActionResult> Index(string buscarUsuario)
        {
            var query = _context.Pedidos
                .AsNoTracking()
                .Include(p => p.Cliente)
                .Include(p => p.Usuario)
                .Include(p => p.DetallePedido)
                .AsQueryable();

            if (!string.IsNullOrEmpty(buscarUsuario))
            {
                query = query.Where(p => p.Usuario.NombreUsuario.Contains(buscarUsuario));
            }

            var pedidos = await query.ToListAsync();

            ViewBag.FiltroUsuario = buscarUsuario;

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
            return Json(direcciones);

        }
    }
}