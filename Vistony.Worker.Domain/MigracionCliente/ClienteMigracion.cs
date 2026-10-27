using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Domain.MigracionCliente
{
    public class ClienteMigracion
    {
        public string Block { get; set; } = string.Empty;
        public string CodeSap { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public string Department { get; set; } = string.Empty;
        public string IdentificationCode { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
    }
}
