using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.CostoEstandar.Interfaces;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class CostoComercialHanaRepository : ICostoComercialRepository
    {
        private readonly string _connStrPeru;
        private readonly string _connStrSeidor;

        public CostoComercialHanaRepository(IConfiguration configuration)
        {
            _connStrPeru = configuration.GetConnectionString("HanaConnectionSEIDOR_PERU")
                ?? throw new InvalidOperationException("No se encontró HanaConnectionSEIDOR_PERU.");

            _connStrSeidor = configuration.GetConnectionString("HanaConnectionSEIDOR")
                ?? throw new InvalidOperationException("No se encontró HanaConnectionSEIDOR.");

        }


        public async Task EjecutarAsync(string database)
        {
            string connStr = database.EndsWith("_PE")
                ? _connStrPeru
                : _connStrSeidor;

            await using var connection = new HanaConnection(connStr);
            await connection.OpenAsync();

            string query =
                $"CALL \"{database}\".\"SP_UPDATE_COSTO_COMERCIA\"()";

            await using var command = new HanaCommand(query, connection);
            await command.ExecuteNonQueryAsync();


        }
    }
}
