using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Logistics.Data.Context
{
    public class NoSqlContext : INoSqlContext
    {
        private IMongoDatabase Database { get; set; }
        private readonly List<Func<Task>> _commands;
        public NoSqlContext(IConfiguration configuration)
        {
            // Set Guid to CSharp style (with dash -)
            BsonDefaults.GuidRepresentation = GuidRepresentation.CSharpLegacy;

            // Every command will be stored and it'll be processed at SaveChanges
            _commands = new List<Func<Task>>();

            RegisterConventions();

            // Configure mongo (You can inject the config, just to simplify)
            var t = configuration.GetSection("DBSettings").GetSection("Connection").Value;
            var mongoClient = new MongoClient(configuration.GetSection("DBSettings").GetSection("Connection").Value);
            Database = mongoClient.GetDatabase(configuration.GetSection("DBSettings").GetSection("DatabaseName").Value);
        }

        private void RegisterConventions()
        {
            var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfDefaultConvention(false)
            };
            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
        }

        public int SaveChanges()
        {
            try
            {
                var qtd = _commands.Count;
                foreach (var command in _commands)
                {
                    command();
                }

                _commands.Clear();
                return qtd;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return Database.GetCollection<T>(name);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task AddCommand(Func<Task> func)
        {
            _commands.Add(func);
            return Task.CompletedTask;
        }
    }
}
