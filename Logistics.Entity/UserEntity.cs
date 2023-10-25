using Logistics.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Entity
{
    public class UserEntity : BaseDocumentDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PassswordHash { get; set; }
    }
}
