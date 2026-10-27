using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.PromoRangos.Interfaces
{
    public interface IPromoRangosRepository
    {
        Task CalcularPromoRangosAsync(string database);
    }
}
