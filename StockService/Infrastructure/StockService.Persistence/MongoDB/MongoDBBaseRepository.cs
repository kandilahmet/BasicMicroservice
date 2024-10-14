using CrossCutting.Abstractions.Repositories;
using MongoDB.Driver;
using StockService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Infrastructure.Persistence.MongoDB
{
    public class MongoDBBaseRepository<T>:IBaseRepository<T>
        where T:BaseEntity
    {
        protected readonly IMongoCollection<T> _mongoCollection;
        public MongoDBBaseRepository(IMongoDatabase database)
        {
            _mongoCollection = database.GetCollection<T>(typeof(T).Name);
        }
    }
}
