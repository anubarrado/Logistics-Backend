using Logistics.Data.Context;
using Logistics.Entity.Base;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Data.Repositories.Generic
{
    public class GenericNoSqlRepository<TEntity> : IGenericNoSqlRepository<TEntity> where TEntity : BaseDocumentDTO
    {
        protected readonly INoSqlContext _context;
        protected readonly IMongoCollection<TEntity> DbSet;
        
        protected GenericNoSqlRepository(INoSqlContext context)
        {
            _context = context;
            DbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public TEntity AddSync(TEntity obj)
        {
            DbSet.InsertOne(obj);
            
            return obj;
        }

        public async Task<TEntity> Add(TEntity obj)
        {
            await DbSet.InsertOneAsync(obj);
            return obj;
        }

        public Task AddAsync(TEntity obj)
        {
            _context.AddCommand(() => DbSet.InsertOneAsync(obj));
            return Task.CompletedTask;
        }

        public Task UpdateAsync(TEntity obj, string id)
        {
            _context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id)), obj));
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(string id)
        {
            _context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id))));
            return Task.CompletedTask;
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id)));
            return data.FirstOrDefault();
        }

        public TEntity GetById(string id)
        {
            var query = Builders<TEntity>.Filter.Eq("_id", new ObjectId(id));
            var data = DbSet.Find(query).FirstOrDefault();
            return data;
        }

        public bool UpdateSync(TEntity obj, string id)
        {
            var result = DbSet.ReplaceOne(Builders<TEntity>.Filter.Eq("_id", new ObjectId(id)), obj);
            return result.ModifiedCount > 0;
        }

        public List<TEntity> ListById(List<string> listadoIds)
        {
            List<ObjectId> listadoNoSqlId = new List<ObjectId>();
            foreach (var item in listadoIds)
            {
                listadoNoSqlId.Add(new ObjectId(item));
            }
            var query = Builders<TEntity>.Filter.In("_id", listadoNoSqlId);

            var data = DbSet.Find(query).ToList();
            return data;
        }
        public List<TEntity> List()
        {
            List<ObjectId> listadoNoSqlId = new List<ObjectId>();
            var query = Builders<TEntity>.Filter.Eq("State", true);

            var data = DbSet.Find(query).ToList();            
            return data;
        }

        public bool Exist(string id)
        {
            var query = Builders<TEntity>.Filter.Eq("_id", new ObjectId(id));
            var data = DbSet.Find(query).Count();
            return data > 0;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
