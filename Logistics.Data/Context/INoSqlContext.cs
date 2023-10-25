using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Data.Context
{
    public interface INoSqlContext : IDisposable
    {
        Task AddCommand(Func<Task> func);
        int SaveChanges();
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
