using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.WMS.UseCases
{
    public class WMSJob
    {
        private readonly WMSUseCase _useCase;
        private readonly ILogger<WMSJob> _logger;
        public WMSJob(
            WMSUseCase useCase,
            ILogger<WMSJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }
        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB WMS");
            Console.WriteLine("INICIO JOB WMS");
            await _useCase.ExecuteAsync();
            Console.WriteLine("FIN JOB WMS");
            _logger.LogInformation("FIN JOB WMS");
        }
    }
}
