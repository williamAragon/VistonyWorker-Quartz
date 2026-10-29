using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.PrecioMayorista.Interfaces;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class PrecioMayoristaHanaRepository : IPrecioMayorista
    {
        private readonly IConfiguration _configuration;

        public PrecioMayoristaHanaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task CalcularPrecioMayoristaAsync(string database)
        {
            var connectionString = _configuration.GetConnectionString("HanaConnectionSEIDOR_PERU")
                ?? throw new InvalidOperationException("Connection string 'HanaConnectionSEIDOR_PERU' no encontrada en configuración");

            await using var connection = new Sap.Data.Hana.HanaConnection(connectionString);
            await connection.OpenAsync();

            string query = $"CALL \"{database}\".\"SP_VIS_CARGA_PRECIO_REFERENCIAL\"()";
            Console.WriteLine(query);

            await using var command = new Sap.Data.Hana.HanaCommand(query, connection);
            command.CommandTimeout = 600;

            await command.ExecuteNonQueryAsync();
        }
    }
}
