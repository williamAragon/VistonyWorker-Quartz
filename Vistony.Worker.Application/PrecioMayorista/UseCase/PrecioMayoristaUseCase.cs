using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.PrecioMayorista.Interfaces;

namespace Vistony.Worker.Application.PrecioMayorista.UseCase
{
    public sealed class PrecioMayoristaUseCase
    {
        private readonly IPrecioMayorista _precioMayoristaRepository;
        private readonly ILogger<PrecioMayoristaUseCase> _logger;

        public PrecioMayoristaUseCase(IPrecioMayorista precioMayoristaRepository,
            ILogger<PrecioMayoristaUseCase> logger)
        {
            _precioMayoristaRepository = precioMayoristaRepository; _logger = logger;
        }
        public async Task ExecuteAsync()
        {
            try
            {
                _logger.LogInformation("INICIO JOB PRECIO_MAYORISTA");

                await _precioMayoristaRepository.CalcularPrecioMayoristaAsync("B1H_VIST_PE");

                _logger.LogInformation("FIN JOB PRECIO_MAYORISTA");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al calcular el precio mayorista");
                throw;
            }            
        }
    }
}
