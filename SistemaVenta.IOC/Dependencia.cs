using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DAL.Repositorios;

namespace SistemaVenta.IOC
{
    public static class Dependencia
    {
        //Recibir un servicio de conexiones - metodo de extension
        public static void InyectarDependencias( this IServiceCollection service, IConfiguration configuration)
        {
            //agregar contexto de nuestra referencia de la cadena de conexión 
            service.AddDbContext<DbventaContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("cadenaSQL"));
            });

            service.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));   //inyeccion de dependencia de manera generica, uso en cualquier modelo
            service.AddScoped<IVentaRepository,VentaRepository>();                             //inyeccion de dependencia . solo modelo de venta
        }
    }
}
