using Spark.DatabaseAccessor.SqlSugar.WebAPI.Sample.Extensions;

var builder = WebApplication.CreateBuilder(args);
//注册AspNetCore控制器容器服务
builder.Services.AddControllers();
// 注册SqlSugar数据库驱动容器服务
builder.Services.AddSparkDatabaseAccessor(builder.Configuration);
var app = builder.Build();
//配置SqlSugar数据库驱动服务
app.UseSqlSugarDatabaseAccessor();
//配置数据库表初始化服务
app.UseSparkApplicationInitTables();
app.MapControllers();
app.Run();
