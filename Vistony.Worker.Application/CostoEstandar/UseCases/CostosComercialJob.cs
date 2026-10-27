using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.CostoEstandar.UseCases
{
    public class CostosComercialJob
    {
        private readonly ObtenerCostoComercialUseCase _useCase;
        private readonly ILogger<CostosComercialJob> _logger;

        public CostosComercialJob(
            ObtenerCostoComercialUseCase useCase,
            ILogger<CostosComercialJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        } 

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB COSTO_COMERCIAL");
            Console.WriteLine("INICIO JOB COSTO_COMERCIAL");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB COSTO_COMERCIAL");
            _logger.LogInformation("FIN JOB COSTO_COMERCIAL");
        }


    }
}
