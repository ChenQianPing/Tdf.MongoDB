using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using Tdf.MongoDB;

namespace Tdf.MongoDbTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Mongo DB Test";
            //InsertTest();
            QueryTest();
            //UpdateTest();
            //DeleteTest();

            Console.WriteLine("Finish!");

            Console.ReadLine();
        }

        #region 插入数据测试
        /// <summary>
        /// 插入数据测试
        /// </summary>
        static void InsertTest()
        {
            var random = new Random();
            for (var i = 1; i <= 10; i++)
            {
                var item = new User()
                {
                    UserName = "我的名字" + i,
                    Age = random.Next(25, 30),
                    State = i % 2 == 0 ? State.Normal : State.Unused
                };
                MongoDbHelper.Insert(DbConfigParams.ConntionString, DbConfigParams.DbName, CollectionNames.User, item);
            }
        }
        #endregion

        #region 查询测试
        /// <summary>
        /// 查询测试
        /// </summary>
        static void QueryTest()
        {
            var queryBuilder = new QueryBuilder<User>();
            var query = queryBuilder.GTE(x => x.Age, 27);
            var ltModel = MongoDbHelper.GetManyByCondition<User>(DbConfigParams.ConntionString, DbConfigParams.DbName,
                CollectionNames.User, query);
            if (ltModel != null && ltModel.Count > 0)
            {
                foreach (var item in ltModel)
                {
                    Console.WriteLine("姓名：{0}，年龄：{1}，状态：{2}",
                        item.UserName, item.Age, GetStateDesc(item.State));
                }
            }
        }
        #endregion

        #region 更新测试
        /// <summary>
        /// 更新测试
        /// </summary>
        static void UpdateTest()
        {
            var queryBuilder = new QueryBuilder<User>();
            var query = queryBuilder.GTE(x => x.Age, 27);
            var dictUpdate = new Dictionary<string, BsonValue>();
            dictUpdate["State"] = State.Unused;
            MongoDbHelper.Update(DbConfigParams.ConntionString, DbConfigParams.DbName, CollectionNames.User, query,
                dictUpdate);
        }
        #endregion

        #region 删除测试
        /// <summary>
        /// 删除测试
        /// </summary>
        static void DeleteTest()
        {
            var queryBuilder = new QueryBuilder<User>();
            var query = queryBuilder.GTE(x => x.Age, 28);
            MongoDbHelper.DeleteByCondition(DbConfigParams.ConntionString, DbConfigParams.DbName, CollectionNames.User, query);
        }
        #endregion

        #region 获取状态描述
        /// <summary>
        /// 获取状态描述
        /// </summary>
        /// <param name="state">状态</param>
        /// <returns>状态描述</returns>
        static string GetStateDesc(State state)
        {
            string result = string.Empty;
            switch (state)
            {
                case State.Normal:
                    result = "正常";
                    break;
                case State.Unused:
                    result = "未使用";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state");
            }
            return result;
        }
        #endregion
    }
}
