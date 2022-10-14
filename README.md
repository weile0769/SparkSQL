<div align="center">
	<h1 align="center" style="color:#4da7fd"><b>SparkSql</b></h1>
</div>
<div align="center">
<h4 align="center">⚡一款高度抽象的、开箱即用的、具备标准常用数据库访问 OPEN API 的项目</h4>
<span align="center" style="letter-spacing:1.8px" >致力于组件抽象化,降低组件功能之间的耦合度,让特性实现更加多样性、可代替性。</span>
</div>
<br>
<p align="center">
<img alt="visual studio 2022" src="https://img.shields.io/badge/Visual Studio-2022-blue.svg">
<img alt="csharp" src="https://img.shields.io/badge/language-csharp-brightgreen.svg">
<img alt="license" src="https://img.shields.io/badge/license-MIT-blue.svg">
<img alt="release" src="https://img.shields.io/badge/release-0.1.0-green">
</p>

### 🚩 项目介绍

SparkSql 一款高度抽象的、开箱即用的、具备标准常用数据库访问 OPEN API 的项目。目前默认实现是基于开源国产著名的ORM框架 <a href="https://gitee.com/dotnetchina/SqlSugar">SqlSugar<a/> 研发的。最大的想法是致力于帮助开发者们将常用标准API尽可能的抽象化，降低系统功能组件之间的耦合度,让系统特性实现更加多样性、让功能组件在未来发展具备可代替性。


##### 🏅开源地址
[![Gitee](https://shields.io/badge/Gitee-https://gitee.com/weile0796/sparksql-green?logo=gitee&style=flat&logoColor=red)](https://gitee.com/weile0796/sparksql)

### 📰 功能架构


### 🎉 安装
```csharp
var builder = WebApplication.CreateBuilder(args);
//注册数据库操作提供器容器服务
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
//配置数据库驱动服务
app.UseSparkDatabaseAccessor();
app.Run();
```
### 📖 更新日志

请查看 [Release Notes](docs)