using Logistics.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Entity
{
    public class SupplierEntity : BaseDocumentDTO
    {
        public string IdEntidad { get; set; }
        public string RUC { get; set; }
        public string FormalName { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string ContacEmail { get; set; }

        public bool State { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}
