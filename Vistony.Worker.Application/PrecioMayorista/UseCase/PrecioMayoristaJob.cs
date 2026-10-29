using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.PrecioMayorista.UseCase
{
    public class PrecioMayoristaJob
    {
        private readonly PrecioMayoristaUseCase _useCase;
        private readonly ILogger<PrecioMayoristaJob> _logger;

        public PrecioMayoristaJob(
            PrecioMayoristaUseCase useCase,
            ILogger<PrecioMayoristaJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }
        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB PRECIO_MAYORISTA");
            Console.WriteLine("INICIO JOB PRECIO_MAYORISTA");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB PRECIO_MAYORISTA");
            _logger.LogInformation("FIN JOB PRECIO_MAYORISTA");
        }
    }
}
