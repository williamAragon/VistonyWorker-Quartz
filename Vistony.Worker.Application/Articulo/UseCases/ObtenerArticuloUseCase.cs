using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.Articulo.Interfaces;

namespace Vistony.Worker.Application.Articulo.UseCases
{
    public sealed class ObtenerArticuloUseCase
    {
        private readonly IArticuloRepository _repository;
        private readonly IFirestoreArticuloClient _firestoreClient;
        private readonly ILogger<ObtenerArticuloUseCase> _logger;

        private static readonly string[] _locations =
        {
        "PE",
        "CL",
        "EC",
        "PY",
        "BO"
    };

        public ObtenerArticuloUseCase(
            IArticuloRepository repository,
            IFirestoreArticuloClient firestoreClient,
            ILogger<ObtenerArticuloUseCase> logger)
        {
            _repository = repository;
            _firestoreClient = firestoreClient;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            foreach (var location in _locations)
            {
                try
                {
                    var companyId = await _firestoreClient.ObtenerCompanyIdAsync(location);
                    if (string.IsNullOrWhiteSpace(companyId))
                    {
                        _logger.LogWarning("No existe company en Firestore para {Location}", location);
                        continue;
                    }

                    var itemCodes = await _repository.ObtenerCodigosAsync(location);
                    var existentes = await _firestoreClient.ObtenerItemCodesExistentesAsync(companyId);

                    var pendientes = itemCodes.Where(c => !existentes.Contains(c)).ToList();
                    _logger.LogInformation("{Location}: {Total} en HANA, {Existentes} ya en Firestore, {Pendientes} por migrar",
                        location, itemCodes.Count, existentes.Count, pendientes.Count);

                    foreach (var itemCode in pendientes)
                    {
                        try
                        {
                            var articulo = await _repository.ObtenerDetalleAsync(location, companyId, itemCode);
                            if (articulo == null)
                            {
                                _logger.LogWarning("{Location} {ItemCode}: sin detalle en HANA", location, itemCode);
                                continue;
                            }

                            await _firestoreClient.GuardarAsync(articulo);
                            _logger.LogInformation("Artículo migrado {Location} {ItemCode}", location, itemCode);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error {Location} ItemCode {ItemCode}", location, itemCode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error Location {Location}", location);
                }
            }
        }
    }
}
