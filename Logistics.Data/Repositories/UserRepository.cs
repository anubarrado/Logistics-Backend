using Logistics.Data.Context;
using Logistics.Data.Repositories.Generic;
using Logistics.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Data.Repositories
{
    public class UserRepository : GenericNoSqlRepository<UserEntity>, IUserRepository
    {
        public UserRepository(INoSqlContext context) : base(context)
        {
        }

        public UserEntity GetByUserName(string userName)
        {
            try
            {
                var query = Builders<UserEntity>.Filter.Eq("UserName", userName);
                var data = DbSet.Find(query).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }           
        }
    }
}
