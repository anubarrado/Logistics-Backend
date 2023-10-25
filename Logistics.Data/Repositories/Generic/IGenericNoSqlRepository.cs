using Logistics.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Data.Repositories.Generic
{
    public interface IGenericNoSqlRepository<TEntity> : IDisposable where TEntity : BaseDocumentDTO
    {
        TEntity AddSync(TEntity obj);
        bool UpdateSync(TEntity obj, string id);

        Task<TEntity> Add(TEntity obj);
        Task AddAsync(TEntity obj);
        Task UpdateAsync(TEntity obj, string id);
        Task<TEntity> GetByIdAsync(string id);
        Task DeleteAsync(string id);

        TEntity GetById(string id);
        List<TEntity> ListById(List<string> listIds);
        public List<TEntity> List();

        bool Exist(string id);
    }
}
