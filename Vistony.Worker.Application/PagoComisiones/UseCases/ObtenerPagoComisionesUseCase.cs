using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.PagoComisiones.Interfaces;

namespace Vistony.Worker.Application.PagoComisiones.UseCases
{
    public sealed class ObtenerPagoComisionesUseCase
    {
        private readonly IPagoComisionesRepository _repository;
        private readonly ICorreoPagoComisionesClient _correoClient;
        private readonly ILogger<ObtenerPagoComisionesUseCase> _logger;

        private static readonly string[] _databases =
        {
            "B1H_VIST_EC",
            "B1H_VIST_BO",
            "B1H_VIST_PY",
            "B1H_VIST_CL",
            "B1H_VIST_PE"
        };

        public ObtenerPagoComisionesUseCase(
            IPagoComisionesRepository repository,
            ICorreoPagoComisionesClient correoClient,
            ILogger<ObtenerPagoComisionesUseCase> logger)
        {
            _repository = repository;
            _correoClient = correoClient;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo PAGO_COMISIONES");

            var tasks = new List<Task>();

            foreach (var database in _databases)
            {
                tasks.Add(EjecutarPorDatabaseAsync(database));
            }

            await Task.WhenAll(tasks);

            _logger.LogInformation("Finalizó flujo PAGO_COMISIONES");
        }

        private async Task EjecutarPorDatabaseAsync(string database)
        {
            string sp = $"CALL {database}.P_VIS_VEN_PAGO_COMISIONES_ACTU";

            try
            {
                await _repository.EjecutarAsync(database);

                await _correoClient.EnviarAsync(
                    $"CALL {database} P_VIS_VEN_PAGO_COMISIONES_ACTU",
                    "Se actualizó las Comisiones");

                _logger.LogInformation(
                    "Pago de comisiones ejecutado correctamente para {Database}",
                    database);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error ejecutando Pago de Comisiones para {Database}",
                    database);

                await _correoClient.EnviarAsync(
                    "Error Quartz Actualizacion de Pago de Comisiones Vistony",
                    ex.Message);
            }
        }
    }
}
