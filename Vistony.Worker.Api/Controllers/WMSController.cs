using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.WMS.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/wms")]
    public sealed class WMSController : ControllerBase
    {/*
        private readonly WMSUseCase _useCase;

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();
            return Ok("WMS ejecutado correctamente.");
        }*/
    }
}
