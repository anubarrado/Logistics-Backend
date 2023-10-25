using Logistics.Data.Context;
using Logistics.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Data.UnitofWork
{
    public class UnitOfWorkMongo : IUnitOfWorkNoSql
    {
        private readonly INoSqlContext _context;

        public UnitOfWorkMongo(INoSqlContext context)
        {
            _context = context;
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }


        private ISupplierRepository _supplierRepository;
        private IUserRepository _userRepository;

        ISupplierRepository IUnitOfWorkNoSql.SupplierRepository
        {
            get
            {
                return _supplierRepository ?? new SupplierRepository(_context);
            }
        }
        IUserRepository IUnitOfWorkNoSql.UserRepository
        {
            get
            {
                return _userRepository ?? new UserRepository(_context);
            }
        }

    }
}
