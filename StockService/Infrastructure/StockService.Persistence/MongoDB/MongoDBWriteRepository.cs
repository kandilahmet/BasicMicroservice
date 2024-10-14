using CrossCutting.Abstractions.Repositories.MongoDB;
using MongoDB.Driver;
using StockService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Infrastructure.Persistence.MongoDB
{
    public class MongoDBWriteRepository<T> : MongoDBBaseRepository<T>, IMongoDBWriteRepository<T>
        where T : BaseEntity
    {
        public MongoDBWriteRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {

        }
        public async Task<bool> AddAsync(T Entity)
        {
            await _mongoCollection.InsertOneAsync(Entity);
            return true;
        }

        public async Task AddRangeAsync(List<T> Entities)
        {
            await _mongoCollection.InsertManyAsync(Entities);
        }

        public async Task RemoveRange(List<T> Entities)
        {
            var filter = Builders<T>.Filter.In(x => x.ID, Entities.Select(y => y.ID));
            await _mongoCollection.DeleteManyAsync(filter);
        }

        public async Task Remove(T Entity)
        {
            var filter = Builders<T>.Filter.Eq(x => x.ID, Entity.ID);
            await _mongoCollection.DeleteOneAsync(filter);
        }

        public async Task UpdateRange(List<T> Entities)
        {

            foreach (var item in Entities)
            {
                var filter = Builders<T>.Filter.Eq(x => x.ID,item.ID);
                 await _mongoCollection.ReplaceOneAsync(x => x.ID == item.ID, item);

            }
        }

        public async Task Update(T Entity)
        { 
                var filter = Builders<T>.Filter.Eq(x => x.ID, Entity.ID);
                await _mongoCollection.ReplaceOneAsync(x => x.ID == Entity.ID, Entity);
 
        }
    }
}
