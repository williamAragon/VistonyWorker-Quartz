using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.Articulo.Interfaces
{
    public interface IFirestoreArticuloClient
    {
        Task<string?> ObtenerCompanyIdAsync(string location);

        Task<bool> ExisteAsync(string companyId, string itemCode);

        Task GuardarAsync(Domain.Articulo.Articulo articulo);

        Task<HashSet<string>> ObtenerItemCodesExistentesAsync(string companyId);
    }
}
