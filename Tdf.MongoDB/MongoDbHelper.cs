using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tdf.MongoDB
{
    /// <summary>
    /// MongoDB帮助类
    /// 基于MongoDB官方发布的C#驱动，封装对MongoDB数据库的增删改查访问方法
    /// </summary>
    public static class MongoDbHelper
    {
        #region 获取数据库实例对象
        /// <summary>
        /// 获取数据库实例对象
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <returns>数据库实例对象</returns>
        private static MongoDatabase GetDatabase(string connectionString, string dbName)
        {
            // 创建数据库链接
            var server = new MongoClient(connectionString).GetServer();
            // 获得数据库实例对象
            return server.GetDatabase(dbName);
        }

        #endregion

        #region 插入一条记录
        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="dbName"></param>
        /// <param name="collectionName"></param>
        /// <param name="model"></param>
        public static void Insert<T>(string connectionString, string dbName, string collectionName, T model) where T : EntityBase
        {
            if (model == null)
            {
                throw new ArgumentNullException("model", "待插入数据不能为空");
            }
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection<T>(collectionName);
            collection.Insert(model);
        }

        #endregion

        #region 更新数据
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="query">查询条件</param>
        /// <param name="dictUpdate">更新字段</param>
        public static void Update(string connectionString, string dbName, string collectionName, IMongoQuery query, Dictionary<string, BsonValue> dictUpdate)
        {
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection(collectionName);
            var update = new UpdateBuilder();
            if (dictUpdate != null && dictUpdate.Count > 0)
            {
                foreach (var item in dictUpdate)
                {
                    update.Set(item.Key, item.Value);
                }
            }
            var d = collection.Update(query, update, UpdateFlags.Multi);
        }
        #endregion

        #region 根据ID获取数据对象
        /// <summary>
        /// 根据ID获取数据对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="id">ID</param>
        /// <returns>数据对象</returns>
        public static T GetById<T>(string connectionString, string dbName, string collectionName, ObjectId id)
    where T : EntityBase
        {
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection<T>(collectionName);
            return collection.FindOneById(id);
        }
        #endregion

        #region 根据查询条件获取一条数据
        /// <summary>
        /// 根据查询条件获取一条数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="query">查询条件</param>
        /// <returns>数据对象</returns>
        public static T GetOneByCondition<T>(string connectionString, string dbName, string collectionName, IMongoQuery query)
          where T : EntityBase
        {
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection<T>(collectionName);
            return collection.FindOne(query);
        }
        #endregion

        #region 根据查询条件获取多条数据
        /// <summary>
        /// 根据查询条件获取多条数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public static List<T> GetManyByCondition<T>(string connectionString, string dbName, string collectionName, IMongoQuery query)
         where T : EntityBase
        {
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection<T>(collectionName);
            return collection.Find(query).ToList();
        }
        #endregion

        #region 获取集合中的所有数据
        /// <summary>
        /// 获取集合中的所有数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <returns>数据对象集合</returns>
        public static List<T> GetAll<T>(string connectionString, string dbName, string collectionName)
        where T : EntityBase
        {
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection<T>(collectionName);
            return collection.FindAll().ToList();
        }
        #endregion

        #region 删除集合中符合条件的数据
        /// <summary>
        /// 删除集合中符合条件的数据
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        /// <param name="query">查询条件</param>
        public static void DeleteByCondition(string connectionString, string dbName, string collectionName, IMongoQuery query)
        {
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection(collectionName);
            collection.Remove(query);
        }
        #endregion

        #region 删除集合中的所有数据
        /// <summary>
        /// 删除集合中的所有数据
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <param name="dbName">数据库名称</param>
        /// <param name="collectionName">集合名称</param>
        public static void DeleteAll(string connectionString, string dbName, string collectionName)
        {
            var db = GetDatabase(connectionString, dbName);
            var collection = db.GetCollection(collectionName);
            collection.RemoveAll();
        }
        #endregion

    }
}
