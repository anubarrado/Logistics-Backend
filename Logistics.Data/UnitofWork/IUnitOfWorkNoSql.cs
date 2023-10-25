using Logistics.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Data.UnitofWork
{
    public interface IUnitOfWorkNoSql
    {
        ISupplierRepository SupplierRepository { get; }
        IUserRepository UserRepository { get; }

    }
}
