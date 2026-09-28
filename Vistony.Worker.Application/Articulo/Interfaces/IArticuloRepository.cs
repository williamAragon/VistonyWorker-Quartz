using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.Articulo.Interfaces
{
    public interface IArticuloRepository
    {
        Task<List<string>> ObtenerCodigosAsync(string location);

        Task<Domain.Articulo.Articulo?> ObtenerDetalleAsync(string location, string companyId, string itemCode);
    }
}
