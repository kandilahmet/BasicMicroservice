using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.Abstractions.Repositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StockService.Core.Domain.Entities
{
    public class BaseEntity: IBaseEntity
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.String)]
        public Guid ID { get; set; }
    }
}
