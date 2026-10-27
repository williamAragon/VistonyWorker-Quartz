using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.MigracionCliente.Interfaces;
using Vistony.Worker.Domain.MigracionCliente;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class MigracionClienteHanaRepository : IMigracionClienteRepository
    {
        private readonly IConfiguration _configuration;

        public MigracionClienteHanaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<ClienteMigracion>> ObtenerPendientesAsync(string database)
        {
            string connectionKey = database switch
            {
                "B1H_VIST_PE" => "HanaConnectionSEIDOR_PERU",
                "B1H_VIST_BO" => "HanaConnectionSEIDOR",
                _ => throw new InvalidOperationException($"No hay connection string configurada para {database}")
            };

            var connectionString = _configuration.GetConnectionString(connectionKey)
                ?? throw new InvalidOperationException($"Connection string '{connectionKey}' no encontrada en configuración");

            var lista = new List<ClienteMigracion>();

            await using var connection = new HanaConnection(connectionString);
            await connection.OpenAsync();

            string query = $"CALL \"{database}\".\"P_VIST_MIGRACION_CLIENTE\"()";
            Console.WriteLine(query);

            await using var command = new HanaCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new ClienteMigracion
                {
                    Block = reader["block"]?.ToString() ?? string.Empty,
                    CodeSap = reader["codeSap"]?.ToString() ?? string.Empty,
                    Country = reader["country"]?.ToString() ?? string.Empty,
                    Created = reader["created"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["created"]),
                    Department = reader["department"]?.ToString() ?? string.Empty,
                    IdentificationCode = reader["identificationCode"]?.ToString() ?? string.Empty,
                    Latitude = reader["latitude"] == DBNull.Value ? 0 : Convert.ToDouble(reader["latitude"]),
                    Longitude = reader["longitude"] == DBNull.Value ? 0 : Convert.ToDouble(reader["longitude"]),
                    Name = reader["name"]?.ToString() ?? string.Empty,
                    Street = reader["street"]?.ToString() ?? string.Empty
                });
            }

            return lista;
        }

        public async Task MarcarMigradoAsync(string database, string cardCode)
        {
            string connectionKey = database switch
            {
                "B1H_VIST_PE" => "HanaConnectionSEIDOR_PERU",
                "B1H_VIST_BO" => "HanaConnectionSEIDOR",
                _ => throw new InvalidOperationException($"No hay connection string configurada para {database}")
            };

            var connectionString = _configuration.GetConnectionString(connectionKey)
                ?? throw new InvalidOperationException($"Connection string '{connectionKey}' no encontrada en configuración");

            await using var connection = new HanaConnection(connectionString);
            await connection.OpenAsync();

            string query = $"CALL \"{database}\".\"P_VIST_MIGRACION_CLIENTE_ACTUALIZAR\"(?)";
            Console.WriteLine($"CALL \"{database}\".\"P_VIST_MIGRACION_CLIENTE_ACTUALIZAR\"('{cardCode}')");

            await using var command = new HanaCommand(query, connection);
            command.Parameters.Add(new HanaParameter { Value = cardCode });

            await command.ExecuteNonQueryAsync();
        }
    }
}
