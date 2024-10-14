using CrossCutting.Abstractions.Repositories.MongoDB; 
using MongoDB.Driver;
using StockService.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Infrastructure.Persistence.MongoDB
{
    public class MongoDBReadRepository<T> : MongoDBBaseRepository<T>, IMongoDBReadRepository<T>
        where T : BaseEntity
    {
        public MongoDBReadRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {

        }
        public async Task<List<T>> GetAll()
        {
            return await _mongoCollection.Find(_ => true).ToListAsync();
            
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression)
        {
            return await _mongoCollection.Find(expression).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetWhere(Expression<Func<T, bool>> expression)
        {
            return await _mongoCollection.Find(expression).ToListAsync();

        }
    }
}
