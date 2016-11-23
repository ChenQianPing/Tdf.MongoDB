using Tdf.MongoDB;

namespace Tdf.MongoDbTest
{
    public class User : EntityBase
    {
        public string UserName { get; set; }
        public int Age { get; set; }
        public State State { get; set; }
    }

    /// <summary>
    /// 将集合名称写到C#代码中作为字符串常量
    /// </summary>
    public class CollectionNames
    {
        public const string User = "User";
        public const string Role = "Role";
    }

    public enum State
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 未使用
        /// </summary>
        Unused = 2,
    }
}
