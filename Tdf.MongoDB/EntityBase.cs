using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Tdf.MongoDB
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public class EntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
