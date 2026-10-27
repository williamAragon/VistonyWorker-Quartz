using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.CostoEstandar.Interfaces;

namespace Vistony.Worker.Application.CostoEstandar.UseCases
{
    public class ObtenerCostoComercialUseCase
    {
        private readonly ICostoComercialRepository _repository;
        private readonly ILogger<ObtenerCostoComercialUseCase> _logger;

        public ObtenerCostoComercialUseCase(
                   ICostoComercialRepository repository,
                   ILogger<ObtenerCostoComercialUseCase> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            const string database = "B1H_VIST_PE";
            _logger.LogInformation("Iniciando flujo COSTO_COMERCIAL\"");
            try
            { 
                await _repository.EjecutarAsync(database);
                _logger.LogInformation(
                    "Procedimiento COSTO_COMERCIAL\" ejecutado correctamente para {Database}",
                    database);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error ejecutando COSTO_COMERCIAL\" para {Database}",
                    database);

                throw;
            }
            _logger.LogInformation("Finalizó flujo COSTO_COMERCIAL");
        }


    }
}
