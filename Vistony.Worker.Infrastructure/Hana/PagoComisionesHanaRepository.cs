using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.PagoComisiones.Interfaces;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class PagoComisionesHanaRepository : IPagoComisionesRepository
    {
        private readonly IConfiguration _configuration;

        public PagoComisionesHanaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EjecutarAsync(string database)
        {
            string connectionKey = database switch
            {
                "B1H_VIST_PE" => "HanaConnectionSEIDOR_PERU",
                "B1H_VIST_EC" => "HanaConnectionSEIDOR",
                //"B1H_VIST_BO" => "HanaConnectionSEIDOR",
                "B1H_VIST_PY" => "HanaConnectionSEIDOR",
                "B1H_VIST_CL" => "HanaConnectionSEIDOR",
                _ => throw new InvalidOperationException($"No hay connection string configurada para {database}")
            };

            var connectionString = _configuration.GetConnectionString(connectionKey)
                ?? throw new InvalidOperationException($"Connection string '{connectionKey}' no encontrada en configuración");

            await using var connection = new HanaConnection(connectionString);
            await connection.OpenAsync();

            string query = $"CALL \"{database}\".\"P_VIS_VEN_PAGO_COMISIONES_ACTU\"()";

            await using var command = new HanaCommand(query, connection);
            await command.ExecuteReaderAsync();
        }
    }
}
