using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.WMS.Interfaces;

namespace Vistony.Worker.Application.WMS.UseCases
{
    public sealed class WMSUseCase
    {
        private readonly IWMSRepository _repository;
        private readonly ILogger<WMSUseCase> _logger;

        public static readonly string[] _databases =
        {
            "B1H_VIST_EC",
            "B1H_VIST_BO",
            "B1H_VIST_PY",
            "B1H_VIST_CL",
            "B1H_VIST_PE"
        };

        public WMSUseCase(IWMSRepository repository, ILogger<WMSUseCase> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo WMS");

            var tasks = new List<Task>();
            /*
            foreach (var database in _databases)
            {
                tasks.Add(EjecutarPorDatabaseAsync(database));
            }*/

            await Task.WhenAll(tasks);

            _logger.LogInformation("Finalizó flujo PAGO_COMISIONES");
        }
    }
}
