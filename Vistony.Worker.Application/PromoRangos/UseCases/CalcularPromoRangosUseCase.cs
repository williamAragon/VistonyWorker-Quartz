using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.PromoRangos.Interfaces;

namespace Vistony.Worker.Application.PromoRangos.UseCases
{
    public sealed class CalcularPromoRangosUseCase
    {
        private readonly IPromoRangosRepository _repository;
        private readonly ILogger<CalcularPromoRangosUseCase> _logger;

        public CalcularPromoRangosUseCase(
            IPromoRangosRepository repository,
            ILogger<CalcularPromoRangosUseCase> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando SP_CALCULAR_PROMO_RANGOS");

                await _repository.CalcularPromoRangosAsync("B1H_VIST_PE");

                _logger.LogInformation("Finalizó SP_CALCULAR_PROMO_RANGOS");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando SP_CALCULAR_PROMO_RANGOS");
                throw;
            }
        }
    }
}
