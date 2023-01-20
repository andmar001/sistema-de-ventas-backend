using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Model;

namespace SistemaVenta.DAL.Repositorios
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly DbventaContext _dbcontext;

        public VentaRepository(DbventaContext dbcontext) : base(dbcontext) //colocarlo ya que en el GenericRepository ya se esta accediendo a la base de datos
        {
            _dbcontext = dbcontext;
        }
        //funcionalida para poder registrar una venta
        public async Task<Venta> Registrar(Venta modelo)
        {
            Venta ventaGenerada = new Venta();
            //transacciones por si ocurre algun error - varios insert
            //si ocurre un error todo se restablece a 0
            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    #region productos dentro de una venta
                    //Restar el stock de cada producto que estan dentro del detalle venta
                    foreach (DetalleVenta dv in modelo.DetalleVenta)
                    {
                        Producto producto_encontrado = _dbcontext.Productos.Where(x => x.IdProducto == dv.IdProducto).First();
                        //restar el stock
                        producto_encontrado.Stock = producto_encontrado.Stock - dv.Cantidad;
                        //modificación el la base de datos
                        _dbcontext.Productos.Update(producto_encontrado);
                    }
                    await _dbcontext.SaveChangesAsync();
                    # endregion productos dentro de una venta

                    //generar numero de documento
                    NumeroDocumento correlativo = _dbcontext.NumeroDocumentos.First();
                    //aumentar el ultimo numero
                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaRegistro = DateTime.Now;

                    //actualizar la información
                    _dbcontext.NumeroDocumentos.Update(correlativo);
                    await _dbcontext.SaveChangesAsync();

                    //generar el formato de numero de documento de venta
                    int CantidadDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", CantidadDigitos));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();
                    //00001 -- a 4 digitos
                    //Desde donde va a cortar -siguiente- cuantos digitos va a tener
                    numeroVenta.Substring(numeroVenta.Length - CantidadDigitos, CantidadDigitos);

                    modelo.NumeroDocumento = numeroVenta;

                    await _dbcontext.Venta.AddAsync(modelo);  //agregar modelo
                    await _dbcontext.SaveChangesAsync();      //guardar cambios

                    ventaGenerada = modelo;

                    transaction.Commit(); //finalizar transaction -  comnfirma la operación
                }
                catch (Exception)
                {
                    //Devolver todo como estaba antes
                    transaction.Rollback();
                    throw;
                }
                return ventaGenerada;
            }
        }
    }
}
