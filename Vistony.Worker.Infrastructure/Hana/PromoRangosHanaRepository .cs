using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.PromoRangos.Interfaces;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class PromoRangosHanaRepository : IPromoRangosRepository
    {
        private readonly IConfiguration _configuration;

        public PromoRangosHanaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task CalcularPromoRangosAsync(string database)
        {
            var connectionString = _configuration.GetConnectionString("HanaConnectionSEIDOR_PERU")
                ?? throw new InvalidOperationException("Connection string 'HanaConnectionSEIDOR_PERU' no encontrada en configuración");

            await using var connection = new HanaConnection(connectionString);
            await connection.OpenAsync();

            string query = $"CALL \"{database}\".\"SP_CALCULAR_PROMO_RANGOS\"()";
            Console.WriteLine(query);

            await using var command = new HanaCommand(query, connection);
            command.CommandTimeout = 600;

            await command.ExecuteNonQueryAsync();
        }
    }
}
