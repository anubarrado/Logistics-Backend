using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.DTOs.Auth
{
    public class AutenticationConfigurationDTO
    {
        public string Secret { get; set; } = "";
        public string SecretPaciente { get; set; } = "";
        public double ExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
