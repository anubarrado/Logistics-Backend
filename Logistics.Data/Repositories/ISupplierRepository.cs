using Logistics.Data.Repositories.Generic;
using Logistics.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Data.Repositories
{
    public interface ISupplierRepository : IGenericNoSqlRepository<SupplierEntity>
    {
    }
}
