<div align="center">
	<h1 align="center" style="color:#4da7fd"><b>SparkSQL</b></h1>
</div>
<div align="center">
<span align="center" style="font-weight:bold" >⚡一款高度抽象的、开箱即用的、具备标准常用数据库访问 OPEN API 的项目</span>
<br>
<span align="center" style="letter-spacing:1.8px" >致力于功能抽象化,降低系统对开源组件方案的依赖性,让功能特性实现更加多样性、可扩展性。</span>
</div>
<br>
<p align="center">
<img alt="visual studio 2022" src="https://img.shields.io/badge/Visual Studio-2022-blue.svg">
<img alt="csharp" src="https://img.shields.io/badge/language-csharp-brightgreen.svg">
<img alt="license" src="https://img.shields.io/badge/license-MIT-blue.svg">
<img alt="release" src="https://img.shields.io/badge/release-0.1.0-green">
</p>

### 🚩 项目介绍

SparkSQL 一款高度抽象的、开箱即用的、具备标准常用数据库访问 OPEN API 的项目。目前数据库访问 OPEN API 的默认实现是基于开源ORM框架 <a href="https://gitee.com/dotnetchina/SqlSugar">SqlSugar<a/> 研发的。未来一直会致力于将常用标准API尽可能的抽象化,降低系统对开源组件方案的依赖性,让功能特性实现更加多样性、可扩展性。


### 🏅开源地址
[![Gitee](https://shields.io/badge/Gitee-https://gitee.com/weile0769/SparkSQL-green?logo=gitee&style=flat&logoColor=red)](https://gitee.com/weile0769/SparkSQL)<br>
[![GitHub](https://shields.io/badge/GitHub-https://github.com/weile0769/SparkSQL-green?logo=github&style=flat)](https://github.com/weile0769/SparkSQL)

### 📰 功能架构


### 🎯 安装
```powershell
dotnet add package Spark.DatabaseAccessor.SqlSugar
```
### 📖 更新日志

请查看 [Release Notes](docs/CHANGELOG.zh.md)

### 🎉 功能介绍
#### 1. 项目配置案例
```csharp
var builder = WebApplication.CreateBuilder(args);
//注册SparkSQL数据库访问提供器服务
builder.Services.AddSparkDatabaseAccessor(option =>
{
    option.DefaultConnectionName = "spark";
    option.CQRSEnabled = true;
    option.PrintSqlLogEnabled = true;
    option.PrintSqlLogWithConnectionEnabled = true;
    option.PrintSqlLogExecutionTimeLimit = 3;
    option.DatabaseConnections = new List<DatabaseConnections>
    {
        new DatabaseConnections
        {
            Enabled = true,
            ConnectionName = "spark",
            DatabaseType= DatabaseType.Sqlite,
            ConnectionString=@$"DataSource=C:\database\spark.db",
            IsAutoCloseConnection = true
         }
    }
});
var app = builder.Build();
//配置SparkSQL数据库访问提供器服务
app.UseSparkDatabaseAccessor();
app.Run();
```
**说明：** <br>
DatabaseAccessorOptions配置项：
- DefaultConnectionName：当前主库连接标识
- CQRSEnabled：是否支持读写分离
- PrintSqlLogEnabled：是否开启打印SQL日志
- PrintSqlLogWithConnectionEnabled：是否开启打印SQL日志过程携带连接字符串
- PrintSqlLogExecutionTimeLimit：SQL日志监控耗时上限值。1.SQL耗时到达上限值时才触发日志监控事件。2.需配合PrintSqlLogEnabled设置true才触发
- DatabaseConnections：数据库连接配置项
  
DatabaseConnections配置项：
- Enabled：配置项开关
- ConnectionName：数据库连接标识
- Schema：数据库架构
- ConnectionString：数据库连接字符串
- SlaveDatabaseConnections：数据库连接配置[从库]
- IsAutoCloseConnection：是否自动关闭数据库连接；说明：默认开启自动关闭
- DatabaseType：数据库类型

SlaveDatabaseConnections配置项
- Enabled：配置项开关
- ConnectionName：数据库连接标识
- HitRate：从库执行级别，说明：值越大越优先匹配执行
- ConnectionString：数据库连接字符串

#### 2. 构建数据库上下文使用案例
- IDatabaseContextProvider数据库上下文提供器中的GetDatabaseContext函数可获取数据库上下文
- IDatabaseContextProvider数据库上下文提供器返回值Object对象需要自行实现各自的上下文对象。

```csharp
[SqlTable(TableName = "user")]
public class User : RootEntity
{
    /// <summary>
    ///   用户名
    /// </summary>
    [SqlColumn(ColumnName = "user_name", ColumnDescription = "用户名")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    ///   密码
    /// </summary>
    [SqlColumn(ColumnName = "password", ColumnDescription = "密码", IsNullable = true)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    ///   性别
    /// </summary>
    [SqlColumn(ColumnName = "gender", ColumnDescription = "性别")]
    public GenderEnum Gender { get; set; } = GenderEnum.Boy;

    /// <summary>
    ///   生日
    /// </summary>
    [SqlColumn(ColumnName = "birthday", ColumnDescription = "生日日期", IsNullable = true)]
    public DateTime? Birthday { get; set; }
}

/// <summary>
///     数据库上下文提供器使用案例
/// </summary>
public class DatabaseContextProvider
{
    /// <summary>
    ///     数据库上下文提供器
    /// </summary>
    private readonly IDatabaseContextProvider _databaseContextProvider;

    /// <summary>
    ///     构造函数
    /// </summary>
    public DatabaseContextProvider(IDatabaseContextProvider databaseContextProvider)
    {
        _databaseContextProvider = databaseContextProvider;
    }

    /// <summary>
    ///     数据库上下文
    /// </summary>
    private ISqlSugarClient _context => (ISqlSugarClient)_databaseContextProvider.GetDatabaseContext();

    /// <summary>
    ///   数据库上下文提供器使用案例
    /// </summary>
    /// <returns></returns>
    public async Task DatabaseContextProvider()
    {
        var userModel = new User
        {
            Id = IDGen.SequentialInt64(),
            UserName = "小明",
            Password = "123456"
        };
        await _context.Insertable(userModel).ExecuteCommandAsync();
    }
}
```
#### 3. 数据查询使用案例
下面列举的案例是异步案例，同步案例用法一致。

**构造函数注入数据仓储IBaseRepository<T>**
```csharp
/// <summary>
///     仓储提供器
/// </summary>
private readonly IBaseRepository<User> _userRepository;

/// <summary>
///   构造函数
/// </summary>
public AsyncBaseRepository_Tests(IBaseRepository<User> userRepository)
{
    _userRepository = userRepository;
}
```
**查询全部使用案例**
```csharp
/// <summary>
///   QueryAllAsync_Sample 查询全部使用案例
/// </summary>
/// <returns></returns>
public async Task QueryAllAsync_Sample()
{
    //查询全部
    var userModels = await _userRepository.QueryAllAsync();
}

```

**SQL条件查询使用案例**
```csharp
/// <summary>
///   QueryBySqlWhereAsync_Sample SQL条件查询使用案例
/// </summary>
/// <returns></returns>
public async Task QueryBySqlWhereAsync_Sample()
{
    //SQL条件查询
    var userModels = await _userRepository.QueryAsync($"record_state={(int)RecordStateEnum.Normal}");
}
```

**Lambda条件表达式查询使用案例**
```csharp
/// <summary>
///   QueryByLinqWhereAsync_Sample Lambda条件表达式查询使用案例
/// </summary>
/// <returns></returns>
public async Task QueryByLinqWhereAsync_Sample()
{
    //Lambda条件表达式查询
    var userModels = await _userRepository.QueryAsync(s => s.RecordState == RecordStateEnum.Normal);
}
```

**Lambda条件表达式单条查询使用案例**
```csharp
/// <summary>
///   QuerySingleByLinqWhereThrowsExceptionAsync_Sample Lambda条件表达式单条查询使用案例
/// </summary>
/// <remark>
///   如果查出来的结果超过1条，则抛异常
/// </remark>
/// <returns></returns>
public async Task QuerySingleByLinqWhereThrowsExceptionAsync_Sample()
{
    //Lambda条件表达式单条查询
    var userModels = await _userRepository.QuerySingleAsync(s => s.RecordState == RecordStateEnum.Normal);
}
```

**Lambda条件表达式按指定列名单条查询使用案例**
```csharp
/// <summary>
///   QuerySingleWithFieldColumnsByLinqWhereThrowsExceptionAsync_Sample Lambda条件表达式按指定列名单条查询使用案例
/// </summary>
/// <remark>
///   如果查出来的结果超过1条，则抛异常
/// </remark>
/// <returns></returns>
public async Task QuerySingleWithFieldColumnsByLinqWhereThrowsExceptionAsync_Sample()
{
    //Lambda条件表达式单条查询
    var userModels = await _userRepository.QuerySingleAsync(s => s.RecordState == RecordStateEnum.Normal, "id,user_name");
}
```

**Lambda条件表达式按指定列名单条查询,多条只取其中一条使用案例**
```csharp
/// <summary>
///   QueryQueryFirstOrDefaultWithFieldColumnsByLinqWhereAsync_Sample Lambda条件表达式按指定列名单条查询,多条只取其中一条使用案例
/// </summary>
/// <remark>
///   查询结果为多条数据，则只取其中一条
/// </remark>
/// <returns></returns>
public async Task QueryQueryFirstOrDefaultWithFieldColumnsByLinqWhereAsync_Sample()
{
    //Lambda条件表达式单条查询
    var userModels = await _userRepository.QueryFirstOrDefaultAsync(s => s.RecordState == RecordStateEnum.Normal, "id,user_name");
}
```

**Lambda条件表达式多条查询支持SQL指定排序字段使用案例**
```csharp
/// <summary>
///   QueryByLinqWhereOrWithOrderByFiledsAsync_Sample Lambda条件表达式多条查询支持SQL指定排序字段
/// </summary>
/// <remark>
///   查询结果为多条数据，支持SQL指定排序字段
/// </remark>
/// <returns></returns>
public async Task QueryByLinqWhereOrWithOrderByFiledsAsync_Sample()
{
    //Lambda条件表达式多条查询
    var userModels = await _userRepository.QueryAsync(s => s.RecordState == RecordStateEnum.Normal, "id desc");
}
```

**Lambda条件表达式多条查询支持SQL指定排序字段使用案例**
```csharp
/// <summary>
///   QueryByLinqWhereWithLinqOrderAsync_Sample Lambda条件表达式多条查询支持Lambda条件表达式设置排序字段
/// </summary>
/// <remark>
///   查询结果为多条数据，支持设置排序字段
/// </remark>
/// <returns></returns>
public async Task QueryByLinqWhereWithLinqOrderAsync_Sample()
{
    //Lambda条件表达式多条查询
    var userModels = await _userRepository.QueryAsync(s => s.RecordState == RecordStateEnum.Normal, s => s.Id, false);
}
```

**SQL条件多条查询支持SQL条件设置排序字段使用案例**
```csharp
/// <summary>
///   QueryBySqlWhereOrWithOrderByFiledsAsync_Sample SQL条件多条查询支持SQL条件设置排序字段使用案例
/// </summary>
/// <remark>
///   查询结果为多条数据，支持设置排序字段
/// </remark>
/// <returns></returns>
public async Task QueryBySqlWhereOrWithOrderByFiledsAsync_Sample()
{
    //Lambda条件表达式多条查询
    var userModels = await _userRepository.QueryAsync($"record_state={(int)RecordStateEnum.Normal}", "id desc");
}
```

**Lambda条件表达式指定条数查询支持SQL条件设置排序字段使用案例**
```csharp
/// <summary>
///   QueryTopByLinqWhereOrWithOrderByFiledsAsync_Sample Lambda条件表达式指定条数查询支持SQL条件设置排序字段使用案例
/// </summary>
/// <remark>
///   查询结果为指定条数数据，支持设置排序字段
/// </remark>
/// <returns></returns>
public async Task QueryTopByLinqWhereOrWithOrderByFiledsAsync_Sample()
{
    var userModels = await _userRepository.QueryTopAsync(s => s.RecordState == RecordStateEnum.Normal, 1, "id desc");
}
```

**SQL条件指定条数查询支持SQL条件设置排序字段使用案例**
```csharp
/// <summary>
///   QueryTopBySQLWhereOrWithOrderByFiledsAsync_Sample SQL条件指定条数查询支持SQL条件设置排序字段使用案例
/// </summary>
/// <remark>
///   查询结果为指定条数数据，支持设置排序字段
/// </remark>
/// <returns></returns>
public async Task QueryTopBySQLWhereOrWithOrderByFiledsAsync_Sample()
{
    var userModels = await _userRepository.QueryTopAsync($"record_state={(int)RecordStateEnum.Normal}", 1, "id desc");
}
```

**SQL语句查询使用案例**
```csharp
/// <summary>
///   QuerySqlAsync_Sample SQL语句查询使用案例
/// </summary>
/// <returns></returns>
public async Task QuerySqlAsync_Sample()
{
    var userModels = await _userRepository.QuerySqlAsync($"select  * from user where record_state={(int)RecordStateEnum.Normal};");
}
```

**SQL语句表格查询使用案例**
```csharp
/// <summary>
///   QueryTableAsync_Sample SQL语句表格查询使用案例
/// </summary>
/// <returns></returns>
public async Task QueryTableAsync_Sample()
{
    var userModels = await _userRepository.QueryTableAsync($"select  * from user where record_state={(int)RecordStateEnum.Normal};");
}
```

**Lambda条件表达式多条指定容量分页查询支持SQL指定排序字段使用案例**
```csharp
/// <summary>
///   QueryPageByLinqWhereOrWithOrderByFiledsAsync_Sample Lambda条件表达式多条指定容量分页查询支持SQL指定排序字段使用案例
/// </summary>
/// <returns></returns>
public async Task QueryPageByLinqWhereOrWithOrderByFiledsAsync_Sample()
{
    var userModels = await _userRepository.QueryPageAsync(s => s.RecordState == RecordStateEnum.Normal, 1, 200, "id desc");
}
```

**Lambda条件表达式查询记录数使用案例**
```csharp
/// <summary>
///   QueryCountByLinqWhereAsync_Sample Lambda条件表达式查询记录数使用案例
/// </summary>
/// <returns></returns>
public async Task QueryCountByLinqWhereAsync_Sample()
{
    var userModels = await _userRepository.CountAsync(s => s.RecordState == RecordStateEnum.Normal);
}
```

**Lambda条件表达式查询是否存在使用案例**
```csharp
/// <summary>
///   QueryAnyByLinqWhereAsync_Sample Lambda条件表达式查询是否存在使用案例
/// </summary>
/// <returns></returns>
public async Task QueryAnyByLinqWhereAsync_Sample()
{
    var userModels = await _userRepository.AnyAsync(s => s.RecordState == RecordStateEnum.Normal);
}
```

**Lambda条件表达式查询是否存在使用案例**
```csharp
/// <summary>
///   QueryAnyByLinqWhereAsync_Sample Lambda条件表达式查询是否存在使用案例
/// </summary>
/// <returns></returns>
public async Task QueryAnyByLinqWhereAsync_Sample()
{
    var userModels = await _userRepository.AnyAsync(s => s.RecordState == RecordStateEnum.Normal);
}
```

#### 4. 数据插入使用案例
下面列举的案例是异步案例，同步案例用法一致。

**单条实体插入使用案例**
```csharp
/// <summary>
///   SingleInsertAsync_Sample 单条实体插入使用案例
/// </summary>
/// <returns></returns>
public async Task SingleInsertAsync_Sample()
{
    //插入数据
    var userModel = new User
    {
        Id = IDGen.SequentialInt64(),
        UserName = "小明",
        Password = "123456"
    };
    await _userRepository.InsertAsync(userModel);
}
```

**单条实体指定列插入使用案例**
```csharp
/// <summary>
///   AppointInsertColumnsSingleInsertAsync_Sample 单条实体指定列插入使用案例
/// </summary>
/// <returns></returns>
public async Task AppointInsertColumnsSingleInsertAsync_Sample()
{
    //插入数据
    var userModel = new User
    {
        Id = IDGen.SequentialInt64(),
        UserName = "小明",
        Password = "123456"
    };
    await _userRepository.InsertAsync(userModel, s => new { s.Id, s.UserName, s.CreationDate, s.LastModified, s.RecordState, s.Gender });
}
```

**多条实体插入使用案例**
```csharp
/// <summary>
///   ListInsertAsync_Sample 多条实体插入使用案例
/// </summary>
/// <returns></returns>
public async Task ListInsertAsync_Sample()
{
    //插入数据
    var userModels = new List<User>
    {
        new User
        {
        Id = IDGen.SequentialInt64(),
        UserName = "小明",
        Password = "123456"
        },
        new User
        {
        Id = IDGen.SequentialInt64(),
        UserName = "小化",
        Password = "123456"
        }
    };
    await _userRepository.InsertAsync(userModels);
}
```

**多条实体指定列插入使用案例**
```csharp
/// <summary>
///   AppointInsertColumnsListInsertAsync_Sample 多条实体指定列插入使用案例
/// </summary>
/// <returns></returns>
public async Task AppointInsertColumnsListInsertAsync_Sample()
{
    //插入数据
    var userModels = new List<User>
    {
        new User
        {
        Id = IDGen.SequentialInt64(),
        UserName = "小明",
        Password = "123456"
        },
        new User
        {
        Id = IDGen.SequentialInt64(),
        UserName = "小化",
        Password = "123456"
        }
    };
    await _userRepository.InsertAsync(userModels, s => new { s.Id, s.UserName, s.CreationDate, s.LastModified, s.RecordState, s.Gender });
}
```

#### 5. 数据更新使用案例
下面列举的案例是异步案例，同步案例用法一致。

**单条实体更新使用案例**
```csharp
/// <summary>
///   SingleUpdateAsync_Sample 单条实体更新使用案例
/// </summary>
/// <returns></returns>
public async Task SingleUpdateAsync_Sample()
{
    //更新数据
    var userModel = await _userRepository.QuerySingleAsync(s => s.Id=5757720142968560414); 
    if(userModel==null) throw new ArgumentNullException(nameof(userModel),"用户数据为空");
    userModel.Password = "654321";
    await _userRepository.UpdateAsync(userModel);
}
```

**单条实体指定列更新使用案例**
```csharp
/// <summary>
///   AppointUpdateColumnsSingleUpdateAsync_Sample 单条实体指定列更新使用案例
/// </summary>
/// <returns></returns>
public async Task AppointUpdateColumnsSingleUpdateAsync_Sample()
{
    //更新数据
    var userModel = await _userRepository.QuerySingleAsync(s => s.Id=5757720142968560414); 
    if(userModel==null) throw new ArgumentNullException(nameof(userModel),"用户数据为空");
    userModel.Password = "654321";
    await _userRepository.UpdateAsync(userModel, s => new { s.Password });
}
```

**单条实体指定列Lambda表达式条件更新使用案例**
```csharp
/// <summary>
///   AppointUpdateColumnsSingleUpdateByLinqWhereAsync_Sample 单条实体指定列Lambda表达式条件更新使用案例
/// </summary>
/// <returns></returns>
public async Task AppointUpdateColumnsSingleUpdateByLinqWhereAsync_Sample()
{
    //更新数据
    var userModel = await _userRepository.QuerySingleAsync(s => s.Id=5757720142968560414); 
    if(userModel==null) throw new ArgumentNullException(nameof(userModel),"用户数据为空");
    userModel.Password = "654321";
    await _userRepository.UpdateAsync(userModel, s => new { s.Password }, s => s.Id == 5757720142968560414);
}
```

**单条实体SQL条件更新使用案例**
```csharp
/// <summary>
///   SingleUpdateWithSQLWhereAsync_Sample 单条实体SQL条件更新使用案例
/// </summary>
/// <returns></returns>
public async Task SingleUpdateWithSQLWhereAsync_Sample()
{
    //更新数据
    var userModel = await _userRepository.QuerySingleAsync(s => s.Id=5757720142968560414); 
    if(userModel==null) throw new ArgumentNullException(nameof(userModel),"用户数据为空");
    userModel.Password = "654321";
    await _userRepository.UpdateAsync(userModel, s => s.Id == 5757720142968560414);
}
```

**Lambda表达式条件动态设置更新使用案例**
```csharp
/// <summary>
///   DynamicSetUpdateColumnsWithLinqWhereAsync_Sample Lambda表达式条件动态设置更新使用案例
/// </summary>
/// <returns></returns>
public async Task DynamicSetUpdateColumnsWithLinqWhereAsync_Sample()
{
    //更新数据
    await _userRepository.UpdateAsync(s => new User { Password = "654321" }, s => s.Id == 5757720142968560414);
}
```

**多条实体更新使用案例**
```csharp
/// <summary>
///   ListUpdateAsync_Sample 多条实体更新使用案例
/// </summary>
/// <returns></returns>
public async Task ListUpdateAsync_Sample()
{
    //更新数据
    var userModels = await _userRepository.QueryAsync(s => s.Id=5757720142968560414); 
    if(!userModels.Any()) throw new ArgumentNullException(nameof(userModels),"用户数据为空");
    userModels.ForEach(s => s.Password = "654321");
    await _userRepository.UpdateAsync(userModels);
}
```

**多条实体指定列更新使用案例**
```csharp
/// <summary>
///   AppointUpdateColumnsListUpdateAsync_Sample 多条实体指定列更新使用案例
/// </summary>
/// <returns></returns>
public async Task AppointUpdateColumnsListUpdateAsync_Sample()
{
    //更新数据
    var userModels = await _userRepository.QueryAsync(s => s.Id=5757720142968560414); 
    if(!userModels.Any()) throw new ArgumentNullException(nameof(userModels),"用户数据为空");
    userModels.ForEach(s => s.Password = "654321");
    await _userRepository.UpdateAsync(userModels, s => new { s.Password });
}
```

#### 6. 数据删除使用案例
下面列举的案例是异步案例，同步案例用法一致。

**单条实体删除使用案例**
```csharp
/// <summary>
///   SingleDeleteAsync_Sample 单条实体删除使用案例
/// </summary>
/// <returns></returns>
public async Task SingleDeleteAsync_Sample()
{
    var userModel = await _userRepository.QuerySingleAsync(s => s.Id=5757720142968560414); 
    if(userModel==null) throw new ArgumentNullException(nameof(userModel),"用户数据为空");
    await _userRepository.DeleteAsync(userModel);
}
```

**多条实体删除使用案例**
```csharp
/// <summary>
///   ListDeleteAsync_Sample 多条实体删除使用案例
/// </summary>
/// <returns></returns>
public async Task ListDeleteAsync_Sample()
{
    var userModels = await _userRepository.QueryAsync(s => s.Id=5757720142968560414); 
    if(!userModels.Any()) throw new ArgumentNullException(nameof(userModels),"用户数据为空");
    await _userRepository.DeleteAsync(userModels);
}
```


#### 7. 数据保存使用案例
下面列举的案例是异步案例，同步案例用法一致。

**单条实体保存使用案例**
```csharp
/// <summary>
///   SingleSaveableAsync_Sample 单条实体保存使用案例
/// </summary>
/// <returns></returns>
public async Task SingleSaveableAsync_Sample()
{
    var id = 5757720142968560414;
    var userModel = new User
    {
        Id = id,
        UserName = "小明",
        Password = "123456"
    };
    await _userRepository.SaveableAsync(userModel);
}
```

**单条实体指定列保存使用案例**
```csharp
/// <summary>
///   SingleSaveableWithUpdateColumnsAsync_Sample 单条实体指定列保存使用案例
/// </summary>
/// <returns></returns>
public async Task SingleSaveableWithUpdateColumnsAsync_Sample()
{
    var id = 5757720142968560414;
    var userModel = new User
    {
        Id = id,
        UserName = "小明",
        Password = "123456"
    };
    await _userRepository.SaveableAsync(userModel);
    userModel.Password = "654321";
    await _userRepository.SaveableAsync(userModel, s => new { s.Password });
}
```

**多条实体保存使用案例**
```csharp
/// <summary>
///   ListSaveableAsync_Sample 多条实体保存使用案例
/// </summary>
/// <returns></returns>
public async Task ListSaveableAsync_Sample()
{
    var id1 = 5757720142968560414;
    var id1 = 5757720142968560415;
    var userModels = new List<User>
    {
        new User
        {
        Id = id1,
        UserName = "小明",
        Password = "123456"
        },
        new User
        {
        Id = id1,
        UserName = "小明",
        Password = "123456"
        }
    };
    await _userRepository.SaveableAsync(userModels);
}
```

**多条实体指定列保存使用案例**
```csharp
/// <summary>
///   ListSaveableWithUpdateColumnsAsync_Sample 多条实体指定列保存使用案例
/// </summary>
/// <returns></returns>
public async Task ListSaveableWithUpdateColumnsAsync_Sample()
{
    var id1 = 5757720142968560414;
    var id1 = 5757720142968560415;
    var userModels = new List<User>
    {
        new User
        {
        Id = id1,
        UserName = "小明",
        Password = "123456"
        },
        new User
        {
        Id = id1,
        UserName = "小明",
        Password = "123456"
        }
    };
    await _userRepository.SaveableAsync(userModels);
    userModels.ForEach(s =>
    {
        s.Password = "654321";
        s.LastModified = modifiedTime;
    });
    await _userRepository.SaveableAsync(userModels, s => new { s.LastModified,s.Password });
}
```


#### 8. 大数据插入使用案例
下面列举的案例是异步案例，同步案例用法一致。

**大数据实体插入使用案例**
```csharp
/// <summary>
///   ListBulkCopyAsync_Sample 大数据实体插入使用案例
/// </summary>
/// <returns></returns>
public async Task ListBulkCopyAsync_Sample()
{
    var id1 = 5757720142968560414;
    var id1 = 5757720142968560415;
    var userModels = new List<User>
    {
        new User
        {
        Id = id1,
        UserName = "小明",
        Password = "123456"
        },
        new User
        {
        Id = id1,
        UserName = "小明",
        Password = "123456"
        }
    };
    await _userRepository.BulkCopyAsync(userModels);
}
```

**大数据DataTable插入使用案例**
```csharp
/// <summary>
///   DataTableBulkCopyAsync_Sample 大数据DataTable插入使用案例
/// </summary>
/// <returns></returns>
public async Task DataTableBulkCopyAsync_Sample()
{
    var id1 = 5757720142968560414;
    var id1 = 5757720142968560415;
    var dataTable = new DataTable();
    dataTable.Columns.Add("id", typeof(long));
    dataTable.Columns.Add("create_time", typeof(DateTime));
    dataTable.Columns.Add("modify_time", typeof(DateTime));
    dataTable.Columns.Add("record_state", typeof(int));
    dataTable.Columns.Add("user_name", typeof(string));
    dataTable.Columns.Add("password", typeof(string));
    dataTable.Columns.Add("gender", typeof(int));
    dataTable.Rows.Add(id1, DateTime.Now, DateTime.Now, 1, "小明", "123456", GenderEnum.Boy);
    dataTable.Rows.Add(id1, DateTime.Now, DateTime.Now, 1, "小花", "123456", GenderEnum.Girl);
    await _userRepository.BulkCopyAsync("user", dataTable);
}
```

#### 9. 大数据更新使用案例
下面列举的案例是异步案例，同步案例用法一致。

**大数据实体更新使用案例**
```csharp
/// <summary>
///   ListBulkUpdateAsync_Sample 大数据实体更新使用案例
/// </summary>
/// <returns></returns>
public async Task ListBulkUpdateAsync_Sample()
{
    var userModels = await _userRepository.QueryAsync(s => s.Id=5757720142968560414); 
    if(!userModels.Any()) throw new ArgumentNullException(nameof(userModels),"用户数据为空");
    userModels.ForEach(s => s.Password = "654321");
    await _userRepository.BulkUpdateAsync(userModels);
}
```



#### 10. 执行SQL使用案例
下面列举的案例是异步案例，同步案例用法一致。

**SQL执行使用案例**
```csharp
/// <summary>
///   SqlExecuteCommandAsync_Sample SQL执行使用案例
/// </summary>
/// <returns></returns>
public async Task SqlExecuteCommandAsync_Sample()
{
    var sql = "update user set password = '654321' where id = @id";
    await _userRepository.ExecuteCommandAsync(sql, new { id = id });
}
```


#### 11. 打包提交使用案例
下面列举的案例是异步案例，同步案例用法一致。

**单条实体插入提交使用案例**
```csharp
/// <summary>
///   SingleInsertQueueAsync_Sample 单条实体插入提交使用案例
/// </summary>
/// <returns></returns>
public async Task SingleInsertQueueAsync_Sample()
{
    var userModel = new User
    {
        Id = id,
        UserName = "小明",
        Password = "123456"
    };
    _userRepository.InsertQueue(userModel);
    await _userRepository.SaveQueuesAsync(true);
}
```

**多条实体插入提交使用案例**
```csharp
/// <summary>
///   ListInsertQueueAsync_Sample 多条实体插入提交使用案例
/// </summary>
/// <returns></returns>
public async Task ListInsertQueueAsync_Sample()
{
    var userModels = new List<User>
    {
        new User
        {
        Id = IDGen.SequentialInt64(),
        UserName = "小明",
        Password = "123456"
        },
        new User
        {
        Id = IDGen.SequentialInt64(),
        UserName = "小明",
        Password = "123456"
        }
    };
    _userRepository.InsertQueue(userModels);
    await _userRepository.SaveQueuesAsync(true);
}
```
**单条实体更新提交使用案例**
```csharp
/// <summary>
///   SingleUpdateQueueAsync_Sample 单条实体更新提交使用案例
/// </summary>
/// <returns></returns>
public async Task SingleUpdateQueueAsync_Sample()
{
    //更新数据
    var userModel = await _userRepository.QuerySingleAsync(s => s.Id=5757720142968560414); 
    if(userModel==null) throw new ArgumentNullException(nameof(userModel),"用户数据为空");
    userModel.Password = "654321";
    _userRepository.UpdateQueue(userModel);
    await _userRepository.SaveQueuesAsync(true);
}
```

**单条实体指定列更新提交使用案例**
```csharp
/// <summary>
///   AppointUpdateQueueColumnsSingleUpdateAsync_Sample 单条实体指定列更新提交使用案例
/// </summary>
/// <returns></returns>
public async Task AppointUpdateQueueColumnsSingleUpdateAsync_Sample()
{
    //更新数据
    var userModel = await _userRepository.QuerySingleAsync(s => s.Id=5757720142968560414); 
    if(userModel==null) throw new ArgumentNullException(nameof(userModel),"用户数据为空");
    userModel.Password = "654321";
    _userRepository.UpdateQueue(userModel, s => new { s.Password });
    await _userRepository.SaveQueuesAsync(true);
}
```

#### 12. 多线程使用案例
下面列举的案例是异步案例，同步案例用法一致。

**多线程使用案例**
```csharp
/// <summary>
///   ParallelTaskExecuteCommandAsync_Sample 多线程使用案例
/// </summary>
/// <returns></returns>
public async Task ParallelTaskExecuteCommandAsync_Sample()
{
    var userQueue=new ConcurrentQueue<User>();
    var tasks = new Task[]
    {
        Task.Run(async () =>
        {
                await SparkScope.CreateAsync(async (serviceScope) =>
                {
                    //构建数据库上下文
                    var repository = serviceScope.ServiceProvider.GetRequiredService<IBaseRepository<User>>();
                    var userModel = await repository.QueryFirstOrDefaultAsync(s=>s.UserName=="小明");
                    userQueue.Enqueue(userModel);
                });
        }),
        Task.Run(async () =>
        {
                await SparkScope.CreateAsync(async (serviceScope) =>
                {
                    //构建数据库上下文
                    var repository = serviceScope.ServiceProvider.GetRequiredService<IBaseRepository<User>>();
                    var userModel = await repository.QueryFirstOrDefaultAsync(s=>s.UserName=="小明");
                    userQueue.Enqueue(userModel);
                });
        }),
    };
    Task.WaitAll(tasks);
}
```

#### 13. 事务提交使用案例

**事务提交使用案例**
```csharp
/// <summary>
///   DatabaseUnitOfWork_Sample 事务提交使用案例
/// </summary>
/// <returns></returns>
public async Task DatabaseUnitOfWork_Sample()
{
    var userModel = new User
    {
        Id = IDGen.SequentialInt64(),
        UserName = "小明",
        Password = "123456"
    };
    var userModel1 = new User
    {
        Id = IDGen.SequentialInt64(),
        UserName = "小花",
        Password = "123456"
    };
    try
    {
        _databaseUnitOfWork.BeginTran();
        await _userRepository.InsertAsync(userModel);
        await _userRepository.InsertAsync(userModel1);
        _databaseUnitOfWork.CommitTran();
    }
    catch
    {
        _databaseUnitOfWork.RollbackTran();
    }
}
```