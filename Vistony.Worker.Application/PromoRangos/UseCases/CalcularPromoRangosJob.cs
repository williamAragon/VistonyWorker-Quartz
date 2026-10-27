using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.PromoRangos.UseCases
{
    public class CalcularPromoRangosJob
    {
        private readonly CalcularPromoRangosUseCase _useCase;
        private readonly ILogger<CalcularPromoRangosJob> _logger;

        public CalcularPromoRangosJob(
            CalcularPromoRangosUseCase useCase,
            ILogger<CalcularPromoRangosJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB PROMO_RANGOS");
            Console.WriteLine("INICIO JOB PROMO_RANGOS");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB PROMO_RANGOS");
            _logger.LogInformation("FIN JOB PROMO_RANGOS");
        }
    }
}
