using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.DTOs.Supplier
{
    public class SupplierCreateDTO
    {
        public string RUC { get; set; }
        public string FormalName { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string ContacEmail { get; set; }

        public string UserName { get; set; }
    }
}
