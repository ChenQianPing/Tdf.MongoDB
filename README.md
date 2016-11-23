# Tdf.MongoDB

基于MongoDB官方发布的C#驱动，封装对MongoDB数据库的增删改查访问方法；

- **nuget Url**：[https://www.nuget.org/packages/Tdf.MongoDB/](https://www.nuget.org/packages/Tdf.MongoDB/)
- **详见**：[基于Mongodb进行分布式数据存储](http://www.jianshu.com/p/5289f4011f0d)

## Features
The full list of extension methods in Tdf.MongoDB right now are:

**编写实体类 User.cs**

```
public class User : EntityBase
{
	public string UserName { get; set; }
	public int Age { get; set; }
	public State State { get; set; }
}
```

**其中，State枚举类定义如下： State.cs**
```
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
```

**将集合名称写到C#代码中作为字符串常量**
```
public class CollectionNames
{
	public const string User = "User";
	public const string Role = "Role";
}
```

## Get methods
```
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
```

## Insert methods
```
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
```

## Update methods
```
var queryBuilder = new QueryBuilder<User>();
var query = queryBuilder.GTE(x => x.Age, 27);
var dictUpdate = new Dictionary<string, BsonValue>();
dictUpdate["State"] = State.Unused;
MongoDbHelper.Update(DbConfigParams.ConntionString, DbConfigParams.DbName, CollectionNames.User, query,
	dictUpdate);
```

## Delete methods
```
var queryBuilder = new QueryBuilder<User>();
var query = queryBuilder.GTE(x => x.Age, 28);
MongoDbHelper.DeleteByCondition(DbConfigParams.ConntionString, DbConfigParams.DbName, CollectionNames.User, query);
```
