using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.PagoComisiones.Interfaces;
using Vistony.Worker.Application.WMS.Interfaces;

namespace Vistony.Worker.Application.WMS.UseCases
{
    public sealed class WMSUseCase
    {
        private readonly IWMSRepository _repository;
        private readonly ICorreoPagoComisionesClient _correoClient;
        private readonly ILogger<WMSUseCase> _logger;

        public static readonly string[] _databases =
        {
            "B1H_VIST_PE",
            "B1H_VIST_PY"
        };

        public WMSUseCase(IWMSRepository repository, ICorreoPagoComisionesClient correoClient, ILogger<WMSUseCase> logger)
        {
            _repository = repository;
            _correoClient = correoClient;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo WMS");
            var tasks = new List<Task>();
<<<<<<< HEAD
            /*
            foreach (var database in _databases)
            {
                tasks.Add(EjecutarPorDatabaseAsync(database));
            }*/

=======
            foreach (var database in _databases)
            {
                tasks.Add(EjecutarDatabaseAsync(database));
            }
>>>>>>> WMS
            await Task.WhenAll(tasks);
            _logger.LogInformation("Finalizó flujo WMS");
        }

        private async Task EjecutarDatabaseAsync(string database)
        {
            try
            {
                await _repository.EjecutarUnitsOfMeasure(database);
                await _correoClient.EnviarAsync(
                    $"{database} Actualizacion de UnitsOfMeasure",
                    $"Se actualizo el UnitsOfMeasure a las Hora : {DateTime.Now}");

                await _repository.EjecutarItems(database);
                await _correoClient.EnviarAsync(
                    $"{database} Actualizacion de Items",
                    $"Se actualizo el Items a las Hora : {DateTime.Now}");

                await _repository.EjecutarOutbundOrders(database);
                await _correoClient.EnviarAsync(
                    $"{database} Actualizacion de OutbundOrders",
                    $"Se actualizo el OutbundOrders a las Hora : {DateTime.Now}");

                await _repository.EjecutarReceipts(database);
                await _correoClient.EnviarAsync(
                    $"{database} Actualizacion de Receipts",
                    $"Se actualizo el Receipts a las Hora : {DateTime.Now}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al ejecutar UnitsOfMeasure para la base de datos {database}");
                await _correoClient.EnviarAsync(
                    $"{database} Error en Actualizacion de UnitsOfMeasure",
                    $"Ocurrió un error al actualizar el UnitsOfMeasure a las Hora : {DateTime.Now}. Error: {ex.Message}");

                _correoClient.EnviarAsync(
                    $"{database} Error en Actualizacion de Items",
                    $"Ocurrió un error al actualizar el Items a las Hora : {DateTime.Now}. Error: {ex.Message}").Wait();

                _logger.LogError(ex, $"Error al ejecutar OutbundOrders para la base de datos {database}");
                await _correoClient.EnviarAsync(
                    $"{database} Error en Actualizacion de OutbundOrders",
                    $"Ocurrió un error al actualizar el OutbundOrders a las Hora : {DateTime.Now}. Error: {ex.Message}");

                _correoClient.EnviarAsync(
                    $"{database} Error en Actualizacion de Receipts",
                    $"Ocurrió un error al actualizar el Receipts a las Hora : {DateTime.Now}. Error: {ex.Message}").Wait();
            }
        }
    }
}
