using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.Comisiones.Interfaces;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class ComisionesHanaRepository : IComisionesRepository
    {
        private readonly string _connectionString;
        private readonly string _connectionStringInduvis;

        public ComisionesHanaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HanaConnectionSEIDOR_PERU");
            _connectionStringInduvis = configuration.GetConnectionString("HanaConnectionSEIDOR");
        }

        public async Task EjecutarAsync(string database)
        {
            string Conetion = string.Empty;
            if (database == "B1H_VIST_PE")
            {
                Conetion = _connectionString;
            }
            else
            {
                Conetion = _connectionStringInduvis;

            }

            await using var connection = new HanaConnection(Conetion);
            await connection.OpenAsync();

            string query =
                $"CALL \"{database}\".\"P_VIS_VEN_COMISIONES_ACTU\"()";

            await using var command = new HanaCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }

        public async Task EjecutarB2BAsync(string database)
        {
            string Conetion = string.Empty;
            if (database=="B1H_VIST_PE")
            {
                Conetion = _connectionString;
            }
            else
            {
                Conetion = _connectionStringInduvis;

            }
            if (database == "B1H_VIST_PE")
            {
                await using var connection = new HanaConnection(Conetion);
                await connection.OpenAsync();
                string query = $"CALL \"{database}\".\"P_VIS_VEN_PAGO_COMISIONES_B2B_ACTU\"()";
                await using var command = new HanaCommand(query, connection);
                await command.ExecuteNonQueryAsync();

            }
        }

    }
}
