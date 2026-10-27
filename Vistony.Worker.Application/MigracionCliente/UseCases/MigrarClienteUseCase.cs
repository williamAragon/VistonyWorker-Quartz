using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.MigracionCliente.Interfaces;

namespace Vistony.Worker.Application.MigracionCliente.UseCases
{
    public sealed class MigrarClienteUseCase
    {
        private readonly IMigracionClienteRepository _repository;
        private readonly IFirestoreMigrar _firestoreClient;
        private readonly ILogger<MigrarClienteUseCase> _logger;

        private static readonly string[] _databases =
        {
            "B1H_VIST_PE",
            "B1H_VIST_BO"
        };

        public MigrarClienteUseCase(
            IMigracionClienteRepository repository,
            IFirestoreMigrar firestoreClient,
            ILogger<MigrarClienteUseCase> logger)
        {
            _repository = repository;
            _firestoreClient = firestoreClient;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo MIGRACION_CLIENTE");

            var tasks = new List<Task>();

            foreach (var database in _databases)
            {
                tasks.Add(EjecutarPorDatabaseAsync(database));
            }

            await Task.WhenAll(tasks);

            _logger.LogInformation("Finalizó flujo MIGRACION_CLIENTE");
        }

        private async Task EjecutarPorDatabaseAsync(string database)
        {
            int migrados = 0;
            int errores = 0;

            try
            {
                var clientes = await _repository.ObtenerPendientesAsync(database);

                _logger.LogInformation(
                    "{Database}: {Total} clientes pendientes de migrar",
                    database, clientes.Count);

                foreach (var cliente in clientes)
                {
                    try
                    {
                        await _firestoreClient.RegistrarAsync(cliente);
                        await _repository.MarcarMigradoAsync(database, cliente.CodeSap);
                        migrados++;
                    }
                    catch (Exception ex)
                    {
                        errores++;
                        _logger.LogError(ex,
                            "{Database}: error migrando cliente {CardCode}",
                            database, cliente.CodeSap);
                    }
                }

                _logger.LogInformation(
                    "{Database}: migrados {Migrados}, con error {Errores}",
                    database, migrados, errores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error ejecutando Migración de Clientes para {Database}",
                    database);
            }
        }
    }
}
