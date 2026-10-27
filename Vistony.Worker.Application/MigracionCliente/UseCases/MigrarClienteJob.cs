using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.MigracionCliente.UseCases
{
    public class MigrarClienteJob
    {
        private readonly MigrarClienteUseCase _useCase;
        private readonly ILogger<MigrarClienteJob> _logger;

        public MigrarClienteJob(
            MigrarClienteUseCase useCase,
            ILogger<MigrarClienteJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB MIGRACION_CLIENTE");
            Console.WriteLine("INICIO JOB MIGRACION_CLIENTE");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB MIGRACION_CLIENTE");
            _logger.LogInformation("FIN JOB MIGRACION_CLIENTE");
        }
    }
}
