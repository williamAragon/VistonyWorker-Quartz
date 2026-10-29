using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.PrecioMayorista.Interfaces
{
    public interface IPrecioMayorista
    {
        Task CalcularPrecioMayoristaAsync(string database);
    }
}
