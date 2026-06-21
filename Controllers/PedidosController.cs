using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPIntegradorBack.Data;
using GestorDespacho.ViewModels;
using TPIntegradorBack.Models;
using System.Security.Claims;

namespace GestorDespacho.Controllers
{
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Crear()
        {
            var clientes = await _context.Clientes.Where(c => c.Activo).ToListAsync();
            ViewBag.Clientes = new SelectList(clientes, "Id", "RazonSocial");
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
            if (datosDelFrente == null || !datosDelFrente.Detalles.Any())
            {
                return Json(new { exito = false, mensaje = "El pedido no tiene productos" });
            }
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int usuarioId = string.IsNullOrEmpty(usuarioIdClaim) ? 1 : int.Parse(usuarioIdClaim);

                var nuevoPedido = new Pedido
                {
                    ClienteId = datosDelFrente.ClienteId,
                    UsuarioId = usuarioId,
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
            }catch (Exception e)
            {
                await transaction.RollbackAsync();
                return Json(new { exito = false, mensaje = "Error al procesar el despacho", e.Message });
            }
        }
    }
}