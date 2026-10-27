using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.CostoEstandar.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/costo-comercial")]
    public sealed class CostoComercialController : ControllerBase
    {
        private readonly ObtenerCostoComercialUseCase _useCase;

        public CostoComercialController(ObtenerCostoComercialUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();

            return Ok("COSTO_COMERCIAL ejecutado correctamente.");
        }
    }
}
