using Spark.DatabaseAccessor.SqlSugar.WebAPI.Sample.Extensions;

var builder = WebApplication.CreateBuilder(args);
//ע��AspNetCore��������������
builder.Services.AddControllers();
// ע��SqlSugar���ݿ�������������
builder.Services.AddSparkDatabaseAccessor(builder.Configuration);
var app = builder.Build();
//����SqlSugar���ݿ���������
app.UseSqlSugarDatabaseAccessor();
//�������ݿ���ʼ������
app.UseSparkApplicationInitTables();
app.MapControllers();
app.Run();
