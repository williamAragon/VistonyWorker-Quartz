using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.WMS.Interfaces
{
    public interface IWMSRepository
    {
        Task EjecutarUnitsOfMeasure(string database);
        Task EjecutarItems(string database);
        Task EjecutarOutbundOrders(string database);
        Task EjecutarReceipts(string database);
    }
}
