using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Domain.MigracionCliente;

namespace Vistony.Worker.Application.MigracionCliente.Interfaces
{
    public interface IMigracionClienteRepository
    {
        Task<List<ClienteMigracion>> ObtenerPendientesAsync(string database);
        Task MarcarMigradoAsync(string database, string cardCode);
    }
}
