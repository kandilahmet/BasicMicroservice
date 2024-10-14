using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Core.Domain.Entities
{
    public class Stock:BaseEntity
    {
         
        [BsonElement("ProductId")]
        [BsonRepresentation(BsonType.String)]
        public Guid ProductId { get; set; }
        [BsonElement("Count")]
        public int Count { get; set; }
    }
    
}
