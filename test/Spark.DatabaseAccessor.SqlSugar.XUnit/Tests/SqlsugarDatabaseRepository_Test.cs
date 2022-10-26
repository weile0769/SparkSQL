using Microsoft.Extensions.DependencyInjection;
using Spark.DatabaseAccessor.Models;
using Spark.DatabaseAccessor.Repositories;
using Spark.DatabaseAccessor.SqlSugar.Utils;
using Spark.DatabaseAccessor.SqlSugar.XUnit.Entities;
using Spark.DatabaseAccessor.Utils;
using SqlSugar;
using System.Data;
using Xunit;

namespace Spark.DatabaseAccessor.SqlSugar.XUnit.Tests
{
    /// <summary>
    ///   数据仓储测试案例
    /// </summary>
    [Collection("数据库测试案例顺序执行组别")]
    public class SqlsugarDatabaseRepository_Test
    {
        /// <summary>
        ///     容器服务提供器
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        ///     用户仓储
        /// </summary>
        private readonly IDatabaseRepository<User> _userRepository;

        /// <summary>
        ///     其他用户仓储
        /// </summary>
        private readonly IDatabaseRepository<Other> _otherRepository;

        /// <summary>
        ///   构造函数
        /// </summary>
        public SqlsugarDatabaseRepository_Test(IServiceProvider serviceProvider,
            IDatabaseRepository<User> repository,
            IDatabaseRepository<Other> otherRepository)
        {
            _userRepository = repository;
            _otherRepository = otherRepository;
            _serviceProvider = serviceProvider;
        }

        #region 同步测试案例

        #region Query查询测试案例

        /// <summary>
        ///   QueryAll查询全部测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "查询全部测试案例")]
        public void QueryAll_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            //查询全部
            var userModels = _userRepository.QueryAll();
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }


        /// <summary>
        ///   QueryBySqlWhere_Test SQL条件查询测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "SQL条件查询测试案例")]
        public void QueryBySqlWhere_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            //SQL条件查询
            var userModels = _userRepository.Query($"record_state={(int)RecordStateEnum.Normal}");
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }

        /// <summary>
        ///   QueryByLinqWhere_Test Lambda条件表达式查询测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式查询测试案例")]
        public void QueryByLinqWhere_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            //Lambda条件表达式查询
            var userModels = _userRepository.Query(s => s.RecordState == RecordStateEnum.Normal);
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }

        /// <summary>
        ///   QuerySingleByLinqWhereThrowsException_Test Lambda条件表达式单条查询测试案例
        /// <remarks>
        ///  查询结果为多条数据，则抛出异常
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式单条查询测试案例")]
        public void QuerySingleByLinqWhereThrowsException_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id =id,
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
            _userRepository.Insert(userModels);
            Assert.Throws<SqlSugarException>(() =>
            {
                //Lambda条件表达式单条查询
                var userModel = _userRepository.QuerySingle(s => s.RecordState == RecordStateEnum.Normal);
            });
        }


        /// <summary>
        ///   QuerySingleWithFieldColumnsByLinqWhereThrowsException_Test Lambda条件表达式按指定列名单条查询测试案例
        /// <remarks>
        ///  查询结果为多条数据，则抛出异常
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式按指定列名单条查询测试案例")]
        public void QuerySingleWithFieldColumnsByLinqWhereThrowsException_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
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
            _userRepository.Insert(userModels);
            Assert.Throws<SqlSugarException>(() =>
            {
                //Lambda条件表达式单条查询
                var userModel = _userRepository.QuerySingle(s => s.RecordState == RecordStateEnum.Normal, "id,user_name");
            });
        }


        /// <summary>
        ///   QueryQueryFirstOrDefaultByLinqWhereThrowsException_Test Lambda条件表达式单条查询测试案例
        /// <remarks>
        ///  查询结果为多条数据，则只取其中一条
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式单条查询,多条只取其中一条测试案例")]
        public void QueryQueryFirstOrDefaultByLinqWhereThrowsException_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
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
            _userRepository.Insert(userModels);
            //Lambda条件表达式单条查询
            var userModel = _userRepository.QueryFirstOrDefault(s => s.RecordState == RecordStateEnum.Normal);
            Assert.Equal(userModel.Id, userModels[0].Id);
        }

        /// <summary>
        ///   QueryQueryFirstOrDefaultWithFieldColumnsByLinqWhereThrowsException_Test Lambda条件表达式按指定列名单条查询测试案例
        /// <remarks>
        ///  查询结果为多条数据，则只取其中一条
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式按指定列名单条查询,多条只取其中一条测试案例")]
        public void QueryQueryFirstOrDefaultWithFieldColumnsByLinqWhereThrowsException_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
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
            _userRepository.Insert(userModels);
            //Lambda条件表达式单条查询
            var userModel = _userRepository.QueryFirstOrDefault(s => s.RecordState == RecordStateEnum.Normal, "id,user_name");
            Assert.Equal(userModel.Id, userModels[0].Id);
            Assert.Empty(userModel.Password);
        }

        /// <summary>
        ///   QueryByLinqWhereOrWithrderByFileds_Test Lambda条件表达式多条查询支持SQL指定排序字段测试案例
        /// <remarks>
        ///  查询结果为多条数据，支持SQL指定排序字段
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式多条查询支持SQL指定排序字段测试案例")]
        public void QueryByLinqWhereOrWithOrderByFileds_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
    {
      new User
      {
        Id =id,
        UserName = "小明",
        Password = "123456"
      },
      new User
      {
         Id = id+1,
        UserName = "小明",
        Password = "123456"
      }
    };
            _userRepository.Insert(insertUserModels);
            //Lambda条件表达式多条查询
            var userModels = _userRepository.Query(s => s.RecordState == RecordStateEnum.Normal, "id desc");
            Assert.Equal(userModels[0].Id, insertUserModels[1].Id);
            userModels = _userRepository.Query(s => s.RecordState == RecordStateEnum.Normal, "id asc");
            Assert.Equal(userModels[0].Id, insertUserModels[0].Id);
        }

        /// <summary>
        ///   QueryByLinqWhereWithLinqOrder_Test Lambda条件表达式多条查询支持Lambda条件表达式设置排序字段测试案例
        /// <remarks>
        ///  查询结果为多条数据，支持设置排序字段
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式多条查询支持Lambda条件表达式设置排序字段测试案例")]
        public void QueryByLinqWhereWithLinqOrder_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
            {
                new User
                {
        Id =id,
        UserName = "小明",
        Password = "123456"
      },
      new User
      {
         Id = id+1,
        UserName = "小明",
        Password = "123456"
      }
    };
            _userRepository.Insert(insertUserModels);
            //Lambda条件表达式多条查询
            var userModels = _userRepository.Query(s => s.RecordState == RecordStateEnum.Normal, s => s.Id, false);
            Assert.Equal(userModels[0].Id, insertUserModels[1].Id);
            //Lambda条件表达式多条查询
            userModels = _userRepository.Query(s => s.RecordState == RecordStateEnum.Normal, s => s.Id, true);
            Assert.Equal(userModels[0].Id, insertUserModels[0].Id);
        }


        /// <summary>
        ///   QueryBySqlWhereOrWithOrderByFileds_Test SQL条件多条查询支持SQL条件设置排序字段测试案例
        /// <remarks>
        ///  查询结果为多条数据，支持设置排序字段
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "SQL条件多条查询支持SQL条件设置排序字段测试案例")]
        public void QueryBySqlWhereOrWithOrderByFileds_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
    {
      new User
      {
        Id =id,
        UserName = "小明",
        Password = "123456"
      },
      new User
      {
         Id = id+1,
        UserName = "小明",
        Password = "123456"
      }
    };
            _userRepository.Insert(insertUserModels);
            //SQL条件表达式多条查询
            var userModels = _userRepository.Query($"record_state={(int)RecordStateEnum.Normal}", "id desc");
            Assert.Equal(userModels[0].Id, insertUserModels[1].Id);
            userModels = _userRepository.Query($"record_state={(int)RecordStateEnum.Normal}", "id asc");
            Assert.Equal(userModels[0].Id, insertUserModels[0].Id);
        }

        /// <summary>
        ///   QueryTopByLinqWhereOrWithOrderByFileds_Test Lambda条件表达式指定条数查询支持SQL条件设置排序字段测试案例
        /// <remarks>
        ///  查询结果为指定条数数据，支持设置排序字段
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式指定条数查询支持SQL条件设置排序字段测试案例")]
        public void QueryTopByLinqWhereOrWithOrderByFileds_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
    {
      new User
      {
        Id =id,
        UserName = "小明",
        Password = "123456"
      },
      new User
      {
         Id = id+1,
        UserName = "小明",
        Password = "123456"
      }
    };
            _userRepository.Insert(insertUserModels);
            //Lambda条件表达式指定条数查询
            var userModels = _userRepository.QueryTop(s => s.RecordState == RecordStateEnum.Normal, 1, "id desc");
            Assert.Equal(userModels[0].Id, insertUserModels[1].Id);
            userModels = _userRepository.QueryTop(s => s.RecordState == RecordStateEnum.Normal, 1, "id asc");
            Assert.Equal(userModels[0].Id, insertUserModels[0].Id);
        }


        /// <summary>
        ///   QueryTopBySQLWhereOrWithOrderByFileds_Test SQL条件指定条数查询支持SQL条件设置排序字段测试案例
        /// <remarks>
        ///  查询结果为指定条数数据，支持设置排序字段
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "SQL条件指定条数查询支持SQL条件设置排序字段测试案例")]
        public void QueryTopBySQLWhereOrWithOrderByFileds_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
    {
      new User
      {
        Id =id,
        UserName = "小明",
        Password = "123456"
      },
      new User
      {
         Id = id+1,
        UserName = "小明",
        Password = "123456"
      }
    };
            _userRepository.Insert(insertUserModels);
            //SQL条件指定条数查询
            var userModels = _userRepository.QueryTop($"record_state={(int)RecordStateEnum.Normal}", 1, "id desc");
            Assert.Equal(userModels[0].Id, insertUserModels[1].Id);
            userModels = _userRepository.QueryTop($"record_state={(int)RecordStateEnum.Normal}", 1, "id asc");
            Assert.Equal(userModels[0].Id, insertUserModels[0].Id);
        }

        /// <summary>
        ///   QuerySql_Test SQL语句查询测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "SQL语句查询测试案例")]
        public void QuerySql_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
    {
      new User
      {
        Id =id,
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
            _userRepository.Insert(insertUserModels);
            //SQL语句查询
            var userModels = _userRepository.QuerySql($"select  * from user where record_state={(int)RecordStateEnum.Normal};");
            Assert.Equal(2, userModels.Count);
            userModels = _userRepository.QuerySql($"select  * from user where record_state=@recordState;", new { recordState = (int)RecordStateEnum.Normal });
            Assert.Equal(2, userModels.Count);
        }


        /// <summary>
        ///   QueryTable_Test SQL语句表格查询测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "SQL语句表格查询测试案例")]
        public void QueryTable_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
    {
      new User
      {
        Id =id,
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
            _userRepository.Insert(insertUserModels);
            //SQL语句表格查询
            var userTable = _userRepository.QueryTable($"select  * from user where record_state={(int)RecordStateEnum.Normal};");
            Assert.Equal(2, userTable.Rows.Count);
            userTable = _userRepository.QueryTable($"select  * from user where record_state=@recordState;", new { recordState = (int)RecordStateEnum.Normal });
            Assert.Equal(2, userTable.Rows.Count);
        }

        /// <summary>
        ///   QueryPageByLinqWhereOrWithOrderByFileds_Test Lambda条件表达式多条指定容量分页查询支持SQL指定排序字段测试案例
        /// <remarks>
        ///  查询结果为多条数据，支持SQL指定排序字段
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式多条指定容量分页查询支持SQL指定排序字段测试案例")]
        public void QueryPageByLinqWhereOrWithOrderByFileds_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
            {
              new User
              {
                Id =id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                 Id = id+1,
                UserName = "小明",
                Password = "123456"
              }
            };
            _userRepository.Insert(insertUserModels);
            //Lambda条件表达式多条分页查询
            var userPageModel = _userRepository.QueryPage(s => s.RecordState == RecordStateEnum.Normal, 1, 200, "id desc");
            Assert.Equal(2, userPageModel.TotalCount);
            Assert.Equal(userPageModel.Data?[0].Id ?? 0, insertUserModels[1].Id);
            userPageModel = _userRepository.QueryPage(s => s.RecordState == RecordStateEnum.Normal, 1, 200, "id asc");
            Assert.Equal(2, userPageModel.TotalCount);
            Assert.Equal(userPageModel.Data?[0].Id ?? 0, insertUserModels[0].Id);
        }

        /// <summary>
        ///   QueryCountByLinqWhere_Test Lambda条件表达式查询记录数测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式查询记录数测试案例")]
        public void QueryCountByLinqWhere_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
    {
      new User
      {
        Id =id,
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
            _userRepository.Insert(insertUserModels);
            //Lambda条件表达式多条查询记录数
            var userCount = _userRepository.Count(s => s.RecordState == RecordStateEnum.Normal);
            Assert.Equal(2, userCount);
        }

        /// <summary>
        ///   QueryAnyByLinqWhere_Test Lambda条件表达式查询是否存在测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式查询是否存在测试案例")]
        public void QueryAnyByLinqWhere_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
    {
      new User
      {
        Id =id,
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
            _userRepository.Insert(insertUserModels);
            //Lambda条件表达式多条查询是否存在
            var userAny = _userRepository.Any(s => s.RecordState == RecordStateEnum.Normal);
            Assert.True(userAny);
        }

        #endregion

        #region QueryForEach查询测试案例

        /// <summary>
        ///   QueryForEachByLinqWhere_Test Lambda条件表达式分批查询测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式分批查询测试案例")]
        public void QueryForEachByLinqWhere_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            //Lambda条件表达式查询
            var userModels = _userRepository.QueryForEach(s => s.RecordState == RecordStateEnum.Normal);
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }

        #endregion

        #region Insert插入测试案例

        /// <summary>
        ///   Insert_Test 单条实体插入测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体插入测试案例")]
        public void Insert_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            //查询全部
            var userModels = _userRepository.QueryAll();
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }

        /// <summary>
        ///   AppointInsert_Test 单条实体指定列插入测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体指定列插入测试案例")]
        public void AppointInsert_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456",
                Gender = GenderEnum.Boy
            };
            var insertSuccessedCount = _userRepository.Insert(userModel, s => new { s.Id, s.UserName, s.CreationDate, s.LastModified, s.RecordState, s.Gender });
            //指定插入列
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id, "password");
            Assert.NotNull(userModel);
            Assert.Empty(userModel.Password);
        }

        /// <summary>
        ///   ListInsert_Test 多条实体插入测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体插入测试案例")]
        public void ListInsert_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
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
            var insertSuccessedCount = _userRepository.Insert(userModels);
            //查询全部
            userModels = _userRepository.QueryAll();
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }


        /// <summary>
        ///   AppointListInsert_Test 多条实体指定列插入测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体指定列插入测试案例")]
        public void AppointListInsert_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
        UserName = "小明",
        Password = "123456",
        Gender=GenderEnum.Boy
      },
      new User
      {
         Id = id1,
        UserName = "小洪",
        Password = "123456",
        Gender=GenderEnum.Girl
      }
    };
            var insertSuccessedCount = _userRepository.Insert(userModels, s => new { s.Id, s.UserName, s.CreationDate, s.LastModified, s.RecordState, s.Gender });
            //查询全部
            userModels = _userRepository.QueryAll();
            var passwords = userModels.Select(s => s.Password).Where(s => s.IsNotNullOrWhiteSpace()).ToList();
            Assert.Empty(passwords);
        }

        #endregion

        #region Update更新测试案例

        /// <summary>
        ///   Update_Test 单条实体更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体更新测试案例")]
        public void Update_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            //更新数据
            userModel.Password = "654321";
            var updateSuccessedCount = _userRepository.Update(userModel);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   UpdateWithUpdateColumns_Test 单条实体指定列更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体指定列更新测试案例")]
        public void UpdateWithUpdateColumns_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            //更新数据
            userModel.Password = "654321";
            var updateSuccessedCount = _userRepository.Update(userModel, s => new { s.Password });
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   UpdateWithUpdateColumnsAndLinqWhere_Test 单条实体指定列Lambda表达式条件更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体指定列Lambda表达式条件更新测试案例")]
        public void UpdateWithUpdateColumnsAndLinqWhere_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            //更新数据
            userModel.Password = "654321";
            var updateSuccessedCount = _userRepository.Update(userModel, s => new { s.Password }, s => s.Id == id);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            updateSuccessedCount = _userRepository.Update(userModel, s => new { s.Password }, s => s.Id == id1);
            Assert.Equal(0, updateSuccessedCount);
        }

        /// <summary>
        ///   UpdateWithSQLWhere_Test 单条实体SQL条件更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体SQL条件更新测试案例")]
        public void UpdateWithSQLWhere_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            //更新数据
            userModel.Password = "654321";
            var updateSuccessedCount = _userRepository.Update(userModel, $"id={id}");
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   UpdateWithLinqWhere_Test 单条实体Lambda表达式条件更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体Lambda表达式条件更新测试案例")]
        public void UpdateWithLinqWhere_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            //更新数据
            userModel.Password = "654321";
            var updateSuccessedCount = _userRepository.Update(userModel, s => s.Id == id);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   UpdateWithSetColumnsAndLinqWhere_Test 单条实体Lambda表达式条件动态设置更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体Lambda表达式条件动态设置更新测试案例")]
        public void UpdateWithSetColumnsAndLinqWhere_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456",
                Birthday = DateTime.Now
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            var updateSuccessedCount = _userRepository.Update(s => new User { Password = "654321" }, s => s.Id == id);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   ListUpdate_Test 多条实体更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体更新测试案例")]
        public void ListUpdate_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
        UserName = "小明",
        Password = "123456",
                Birthday = DateTime.Now
      },
      new User
      {
         Id = id1,
        UserName = "小红",
        Password = "123456"
      }
    };
            var insertSuccessedCount = _userRepository.Insert(userModels);
            //更新数据
            userModels.ForEach(s => s.Password = "654321");
            var updateSuccessedCount = _userRepository.Update(userModels);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            var userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id1);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   ListUpdateWithUpdateColumns_Test 多条实体指定列更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体指定列更新测试案例")]
        public void ListUpdateWithUpdateColumns_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
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
            var insertSuccessedCount = _userRepository.Insert(userModels);
            //更新数据
            userModels.ForEach(s => s.Password = "654321");
            var updateSuccessedCount = _userRepository.Update(userModels, s => new { s.Password });
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            var userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id1);
            Assert.Equal("654321", userModel.Password);
        }


        #endregion

        #region Delete删除测试案例

        /// <summary>
        ///   Delete_Test 单条实体删除测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体删除测试案例")]
        public void Delete_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            var deleteSuccessedCount = _userRepository.Delete(userModel);
            Assert.Equal(insertSuccessedCount, deleteSuccessedCount);
            //查询全部
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Null(userModel);
        }


        /// <summary>
        ///   DeleteWithLinqWhere_Test Lambda表达式条件删除测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda表达式条件删除测试案例")]
        public void DeleteWithLinqWhere_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            var deleteSuccessedCount = _userRepository.Delete(s => s.Id == id);
            Assert.Equal(insertSuccessedCount, deleteSuccessedCount);
            //查询全部
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Null(userModel);
        }


        /// <summary>
        ///   ListDelete_Test 多条实体删除测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体删除测试案例")]
        public void ListDelete_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
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
            var insertSuccessedCount = _userRepository.Insert(userModels);
            var deleteSuccessedCount = _userRepository.Delete(userModels);
            Assert.Equal(insertSuccessedCount, deleteSuccessedCount);
            //查询全部
            userModels = _userRepository.QueryAll();
            Assert.Empty(userModels);
        }



        #endregion

        #region TruncateTable清空表测试案例

        /// <summary>
        ///   TruncateTable_Test 清空表测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "清空表测试案例")]
        public void TruncateTable_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            _userRepository.Insert(userModel);
            _userRepository.TruncateTable();
            var userModels = _userRepository.QueryAll();
            Assert.Empty(userModels);
        }

        #endregion

        #region  Saveable保存测试案例

        /// <summary>
        ///   Saveable_Test 单条实体保存测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体保存测试案例")]
        public void Saveable_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            _userRepository.Saveable(userModel);
            //查询全部
            var userModels = _userRepository.QueryAll();
            Assert.NotEmpty(userModels);
            userModel.Password = "654321";
            _userRepository.Saveable(userModel);
            //查询全部
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }


        /// <summary>
        ///   SaveableWithUpdateColumns_Test 单条实体指定列保存测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体指定列保存测试案例")]
        public void SaveableWithUpdateColumns_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            _userRepository.Saveable(userModel);
            //查询全部
            var userModels = _userRepository.QueryAll();
            Assert.NotEmpty(userModels);
            var modifiedTime = DateTime.Now;
            userModel.Password = "654321";
            userModel.LastModified = modifiedTime;
            _userRepository.Saveable(userModel, s => new { s.Password });
            //查询全部
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            Assert.NotEqual(userModel.LastModified, modifiedTime);
        }


        /// <summary>
        ///   ListSaveable_Test 多条实体指定列保存测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体保存测试案例")]
        public void ListSaveable_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
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
            _userRepository.Saveable(userModels);
            //查询全部
            userModels = _userRepository.QueryAll();
            Assert.Equal(2, userModels.Count);
            userModels.ForEach(s => s.Password = "654321");
            _userRepository.Saveable(userModels);
            //查询全部
            var userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id1);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   ListSaveableWithUpdateColumns_Test 多条实体指定列保存测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体指定列保存测试案例")]
        public void ListSaveableWithUpdateColumns_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
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
            _userRepository.Saveable(userModels);
            //查询全部
            userModels = _userRepository.QueryAll();
            Assert.Equal(2, userModels.Count);
            var modifiedTime = DateTime.Now;
            userModels.ForEach(s =>
            {
                s.Password = "654321";
                s.LastModified = modifiedTime;
            });
            _userRepository.Saveable(userModels, s => new { s.Password });
            //查询全部
            var userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            Assert.NotEqual(userModel.LastModified, modifiedTime);
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id1);
            Assert.Equal("654321", userModel.Password);
            Assert.NotEqual(userModel.LastModified, modifiedTime);
        }

        #endregion

        #region  BulkCopy大数据插入测试案例

        /// <summary>
        ///   ListBulkCopy_Test 大数据实体插入测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "大数据实体插入测试案例")]
        public void ListBulkCopy_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
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
            var insertSuccessedCount = _userRepository.BulkCopy(userModels);
            //查询全部
            userModels = _userRepository.QueryAll();
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }


        /// <summary>
        ///   DataTableBulkCopy_Test 大数据DataTable插入测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "大数据DataTable插入测试案例")]
        public void DataTableBulkCopy_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var dataTable = new DataTable();
            dataTable.Columns.Add("id", typeof(long));
            dataTable.Columns.Add("create_time", typeof(DateTime));
            dataTable.Columns.Add("modify_time", typeof(DateTime));
            dataTable.Columns.Add("record_state", typeof(int));
            dataTable.Columns.Add("user_name", typeof(string));
            dataTable.Columns.Add("password", typeof(string));
            dataTable.Columns.Add("gender", typeof(int));
            dataTable.Rows.Add(1, DateTime.Now, DateTime.Now, 1, "小明", "123456", GenderEnum.Boy);
            dataTable.Rows.Add(2, DateTime.Now, DateTime.Now, 1, "小花", "123456", GenderEnum.Girl);

            var insertSuccessedCount = _userRepository.BulkCopy("user", dataTable);
            //查询全部
            var userModels = _userRepository.QueryAll();
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }


        #endregion

        #region  BulkCopy大数据更新测试案例

        /// <summary>
        ///   ListBulkUpdate_Test 大数据实体更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "大数据实体更新测试案例")]
        public void ListBulkUpdate_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
        UserName = "小明",
        Password = "123456",
        Birthday = DateTime.Now
      },
      new User
      {
         Id = id1,
        UserName = "小明",
        Password = "123456",
        Birthday = DateTime.Now
      }
    };
            var insertSuccessedCount = _userRepository.BulkCopy(userModels);
            userModels.ForEach(s => s.Password = "654321");
            var updateSuccessedCount = _userRepository.BulkUpdate(userModels);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            var userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id1);
            Assert.Equal("654321", userModel.Password);
        }

        #endregion

        #region ExecuteCommandSQL执行测试案例

        /// <summary>
        ///   ExecuteCommand_Test SQL执行测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "SQL执行测试案例")]
        public void ExecuteCommand_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
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
            _userRepository.BulkCopy(userModels);
            //更新数据
            var sql = "update user set password = '654321' where id = @id";
            _userRepository.ExecuteCommand(sql, new { id = id });
            var userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            //更新数据
            sql = "update user set password = '654321' where id = @id";
            _userRepository.ExecuteCommand(sql, new { id = id1 });
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id1);
            Assert.Equal("654321", userModel.Password);
        }

        #endregion

        #region InsertQueue插入测试案例

        /// <summary>
        ///   InsertQueue_Test 单条实体插入提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体插入提交测试案例")]
        public void InsertQueue_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            _userRepository.InsertQueue(userModel);
            var insertSuccessedCount = _userRepository.SaveQueues(true);
            //查询全部
            var userModels = _userRepository.QueryAll();
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }

        /// <summary>
        ///   AppointInsertQueue_Test 单条实体指定列插入提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体指定列插入提交测试案例")]
        public void AppointInsertQueue_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456",
                Gender = GenderEnum.Boy
            };
            _userRepository.InsertQueue(userModel, s => new { s.Id, s.UserName, s.CreationDate, s.LastModified, s.RecordState, s.Gender });
            var insertSuccessedCount = _userRepository.SaveQueues(true);
            //指定插入列
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id, "password");
            Assert.NotNull(userModel);
            Assert.Empty(userModel.Password);
        }

        /// <summary>
        ///   ListInsertQueue_Test 多条实体插入提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体插入提交测试案例")]
        public void ListInsertQueue_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
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
            _userRepository.InsertQueue(userModels);
            var insertSuccessedCount = _userRepository.SaveQueues(true);
            //查询全部
            userModels = _userRepository.QueryAll();
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }


        /// <summary>
        ///   AppointListInsertQueue_Test 多条实体指定列插入提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体指定列插入提交测试案例")]
        public void AppointListInsertQueue_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
        UserName = "小明",
        Password = "123456",
        Gender=GenderEnum.Boy
      },
      new User
      {
         Id = id1,
        UserName = "小明",
        Password = "123456",
        Gender=GenderEnum.Boy
      }
    };
            _userRepository.InsertQueue(userModels, s => new { s.Id, s.UserName, s.CreationDate, s.LastModified, s.RecordState, s.Gender });
            var insertSuccessedCount = _userRepository.SaveQueues(true);
            //查询全部
            userModels = _userRepository.QueryAll();
            var passwords = userModels.Select(s => s.Password).Where(s => s.IsNotNullOrWhiteSpace()).ToList();
            Assert.Empty(passwords);
        }


        #endregion

        #region UpdateQueue更新测试案例

        /// <summary>
        ///   UpdateQueue_Test 单条实体更新提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体更新提交测试案例")]
        public void UpdateQueue_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            //更新数据
            userModel.Password = "654321";
            _userRepository.UpdateQueue(userModel);
            var updateSuccessedCount = _userRepository.SaveQueues(true);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }


        /// <summary>
        ///   UpdateQueueWithUpdateColumns_Test 单条实体指定列更新提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体指定列更新提交测试案例")]
        public void UpdateQueueWithUpdateColumns_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            //更新数据
            userModel.Password = "654321";
            _userRepository.UpdateQueue(userModel, s => new { s.Password });
            var updateSuccessedCount = _userRepository.SaveQueues(true);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }


        /// <summary>
        ///   UpdateQueueWithSetColumnsAndLinqWhere_Test 单条实体Lambda表达式条件动态设置更新提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体Lambda表达式条件动态设置更新提交测试案例")]
        public void UpdateQueueWithSetColumnsAndLinqWhere_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = _userRepository.Insert(userModel);
            _userRepository.UpdateQueue(s => new User { Password = "654321" }, s => s.Id == id);
            var updateSuccessedCount = _userRepository.SaveQueues(true);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   ListUpdateQueueWithUpdateColumns_Test 多条实体指定列更新提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体指定列更新提交测试案例")]
        public void ListUpdateQueueWithUpdateColumns_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
    {
      new User
      {
        Id =id,
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
            var insertSuccessedCount = _userRepository.Insert(userModels);
            //更新数据
            userModels.ForEach(s => s.Password = "654321");
            _userRepository.UpdateQueue(userModels, s => new { s.Password });
            var updateSuccessedCount = _userRepository.SaveQueues(true);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            var userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            userModel = _userRepository.QueryFirstOrDefault(s => s.Id == id1);
            Assert.Equal("654321", userModel.Password);
        }

        #endregion

        #region ChangeTransient将数据库上下文切换到瞬时状态

        ///// <summary>
        /////   CheckAfterChangeTransientDatabaseContextID_Tests 任务串行情况下ChangeTransient将数据库上下文切换到瞬时状态测试案例
        ///// </summary>
        ///// <returns></returns>
        //[Fact(DisplayName = "任务串行情况下ChangeTransient将数据库上下文切换到瞬时状态测试案例")]
        //public void CheckAfterChangeTransientDatabaseContextID_Tests()
        //{
        //    //初始化表
        //    _repository.TruncateTable();
        //    //插入数据
        //    var userModel = new User
        //    {
        //        Id =id,
        //        UserName = "小明",
        //        Password = "123456"
        //    };
        //    var insertSuccessedCount = _repository.Insert(userModel);
        //    var dbContext1 = _repository.ChangeTransient();
        //    var contextID1 = dbContext1.ContextID;
        //    var dbContext2 = _repository.ChangeTransient();
        //    var contextID2 = dbContext2.ContextID;
        //    Assert.NotEqual(contextID1, contextID2);
        //    var dbContext3 = _repository;
        //    var contextID3 = dbContext3.ContextID;
        //    var dbContext4 = _repository;
        //    var contextID4 = dbContext4.ContextID;
        //    Assert.Equal(contextID3, contextID4);
        //    Assert.NotEqual(contextID1, contextID3);
        //    Assert.NotEqual(contextID1, contextID4);
        //    Assert.NotEqual(contextID2, contextID3);
        //    Assert.NotEqual(contextID2, contextID4);
        //}


        ///// <summary>
        /////   ParallelTaskCheckAfterChangeTransientDatabaseContextID_Tests 任务并行情况下ChangeTransient将数据库上下文切换到瞬时状态测试案例
        ///// </summary>
        ///// <returns></returns>
        //[Fact(DisplayName = "任务并行情况下ChangeTransient将数据库上下文切换到瞬时状态测试案例")]
        //public void ParallelTaskCheckAfterChangeTransientDatabaseContextID_Tests()
        //{
        //    //初始化表
        //    _repository.TruncateTable();
        //    //插入数据
        //    var userModel = new User
        //    {
        //        Id =id,
        //        UserName = "小明",
        //        Password = "123456"
        //    };
        //    var insertSuccessedCount = _repository.Insert(userModel);
        //    var dbContext1 = _repository;
        //    var supplierModel1 = dbContext1.QueryFirstOrDefault(s => s.UserName == "小明");
        //    var contextID1 = dbContext1.ContextID;
        //    var tasks = new Task[]
        //    {
        //                Task.Run(() => {
        //                    var dbContext2 = _repository.ChangeTransient();
        //                    var supplierModel2 = dbContext2.QueryFirstOrDefault(s=>s.UserName=="小明");
        //                    var contextID2 = dbContext2.ContextID;
        //                    Assert.NotEqual(contextID1,contextID2);
        //                }),
        //                                        Task.Run(() => {
        //                    var dbContext3 = _repository.ChangeTransient();
        //                    var supplierModel3 = dbContext3.QueryFirstOrDefault(s=>s.UserName=="小明");
        //                            var contextID3 = dbContext3.ContextID;
        //                    Assert.NotEqual(contextID1,contextID3);
        //                }),
        //    };
        //    Task.WaitAll(tasks);
        //    var dbContext4 = _repository;
        //    var supplierModel4 = dbContext4.QueryFirstOrDefault(s => s.UserName == "小明");
        //    var contextID4 = dbContext4.ContextID;
        //    Assert.Equal(contextID1, contextID4);
        //}


        #endregion

        #endregion

        #region 异步测试案例

        #region QueryAsync查询测试案例

        /// <summary>
        ///   QueryAllAsync查询全部测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "查询全部测试案例")]
        public async Task QueryAllAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            //查询全部
            var userModels = await _userRepository.QueryAllAsync();
            Assert.Contains(userModel.Id, userModels.Select(s => s.Id));
        }


        /// <summary>
        ///   QueryBySqlWhereAsync_Test SQL条件查询测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "SQL条件查询测试案例")]
        public async Task QueryBySqlWhereAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            //SQL条件查询
            var userModels = await _userRepository.QueryAsync($"record_state={(int)RecordStateEnum.Normal}");
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }

        /// <summary>
        ///   QueryByLinqWhereAsync_Test Lambda条件表达式查询测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式查询测试案例")]
        public async Task QueryByLinqWhereAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            //Lambda条件表达式查询
            var userModels = await _userRepository.QueryAsync(s => s.RecordState == RecordStateEnum.Normal);
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }

        /// <summary>
        ///   QuerySingleByLinqWhereThrowsExceptionAsync_Test Lambda条件表达式单条查询测试案例
        /// <remarks>
        ///  查询结果为多条数据，则抛出异常
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式单条查询测试案例")]
        public async Task QuerySingleByLinqWhereThrowsExceptionAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = 2,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.InsertAsync(userModels);
            await Assert.ThrowsAsync<SqlSugarException>(async () =>
            {
                //Lambda条件表达式单条查询
                var userModel = await _userRepository.QuerySingleAsync(s => s.RecordState == RecordStateEnum.Normal);
            });
        }


        /// <summary>
        ///   QuerySingleWithFieldColumnsByLinqWhereThrowsExceptionAsync_Test Lambda条件表达式按指定列名单条查询测试案例
        /// <remarks>
        ///  查询结果为多条数据，则抛出异常
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式按指定列名单条查询测试案例")]
        public async Task QuerySingleWithFieldColumnsByLinqWhereThrowsExceptionAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = 2,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.InsertAsync(userModels);
            await Assert.ThrowsAsync<SqlSugarException>(async () =>
            {
                //Lambda条件表达式单条查询
                var userModel = await _userRepository.QuerySingleAsync(s => s.RecordState == RecordStateEnum.Normal, "id,user_name");
            });
        }


        /// <summary>
        ///   QueryQueryFirstOrDefaultByLinqWhereThrowsExceptionAsync_Test Lambda条件表达式单条查询测试案例
        /// <remarks>
        ///  查询结果为多条数据，则只取其中一条
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式单条查询,多条只取其中一条测试案例")]
        public async Task QueryQueryFirstOrDefaultByLinqWhereThrowsExceptionAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = 2,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.InsertAsync(userModels);
            //Lambda条件表达式单条查询
            var userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.RecordState == RecordStateEnum.Normal);
            Assert.Equal(userModel.Id, userModels[0].Id);
        }

        /// <summary>
        ///   QueryQueryFirstOrDefaultWithFieldColumnsByLinqWhereAsync_Test Lambda条件表达式按指定列名单条查询测试案例
        /// <remarks>
        ///  查询结果为多条数据，则只取其中一条
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式按指定列名单条查询,多条只取其中一条测试案例")]
        public async Task QueryQueryFirstOrDefaultWithFieldColumnsByLinqWhereAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = 2,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.InsertAsync(userModels);
            //Lambda条件表达式单条查询
            var userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.RecordState == RecordStateEnum.Normal, "id,user_name");
            Assert.Equal(userModel.Id, userModels[0].Id);
            Assert.Empty(userModel.Password);
        }

        /// <summary>
        ///   QueryByLinqWhereOrWithrderByFiledsAsync_Test Lambda条件表达式多条查询支持SQL指定排序字段测试案例
        /// <remarks>
        ///  查询结果为多条数据，支持SQL指定排序字段
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式多条查询支持SQL指定排序字段测试案例")]
        public async Task QueryByLinqWhereOrWithOrderByFiledsAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = id+1,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.InsertAsync(insertUserModels);
            //Lambda条件表达式多条查询
            var userModels = await _userRepository.QueryAsync(s => s.RecordState == RecordStateEnum.Normal, "id desc");
            Assert.Equal(userModels[0].Id, insertUserModels[1].Id);
            userModels = await _userRepository.QueryAsync(s => s.RecordState == RecordStateEnum.Normal, "id asc");
            Assert.Equal(userModels[0].Id, insertUserModels[0].Id);
        }

        /// <summary>
        ///   QueryByLinqWhereWithLinqOrderAsync_Test Lambda条件表达式多条查询支持Lambda条件表达式设置排序字段测试案例
        /// <remarks>
        ///  查询结果为多条数据，支持设置排序字段
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式多条查询支持Lambda条件表达式设置排序字段测试案例")]
        public async Task QueryByLinqWhereWithLinqOrderAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = id+1,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.InsertAsync(insertUserModels);
            //Lambda条件表达式多条查询
            var userModels = await _userRepository.QueryAsync(s => s.RecordState == RecordStateEnum.Normal, s => s.Id, false);
            Assert.Equal(userModels[0].Id, insertUserModels[1].Id);
            //Lambda条件表达式多条查询
            userModels = await _userRepository.QueryAsync(s => s.RecordState == RecordStateEnum.Normal, s => s.Id, true);
            Assert.Equal(userModels[0].Id, insertUserModels[0].Id);
        }


        /// <summary>
        ///   QueryBySqlWhereOrWithOrderByFiledsAsync_Test SQL条件多条查询支持SQL条件设置排序字段测试案例
        /// <remarks>
        ///  查询结果为多条数据，支持设置排序字段
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "SQL条件多条查询支持SQL条件设置排序字段测试案例")]
        public async Task QueryBySqlWhereOrWithOrderByFiledsAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = id+1,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.InsertAsync(insertUserModels);
            //SQL条件表达式多条查询
            var userModels = await _userRepository.QueryAsync($"record_state={(int)RecordStateEnum.Normal}", "id desc");
            Assert.Equal(userModels[0].Id, insertUserModels[1].Id);
            userModels = await _userRepository.QueryAsync($"record_state={(int)RecordStateEnum.Normal}", "id asc");
            Assert.Equal(userModels[0].Id, insertUserModels[0].Id);
        }

        /// <summary>
        ///   QueryTopByLinqWhereOrWithOrderByFiledsAsync_Test Lambda条件表达式指定条数查询支持SQL条件设置排序字段测试案例
        /// <remarks>
        ///  查询结果为指定条数数据，支持设置排序字段
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式指定条数查询支持SQL条件设置排序字段测试案例")]
        public async Task QueryTopByLinqWhereOrWithOrderByFiledsAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = id+1,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.InsertAsync(insertUserModels);
            //Lambda条件表达式指定条数查询
            var userModels = await _userRepository.QueryTopAsync(s => s.RecordState == RecordStateEnum.Normal, 1, "id desc");
            Assert.Equal(userModels[0].Id, insertUserModels[1].Id);
            userModels = await _userRepository.QueryTopAsync(s => s.RecordState == RecordStateEnum.Normal, 1, "id asc");
            Assert.Equal(userModels[0].Id, insertUserModels[0].Id);
        }


        /// <summary>
        ///   QueryTopBySQLWhereOrWithOrderByFiledsAsync_Test SQL条件指定条数查询支持SQL条件设置排序字段测试案例
        /// <remarks>
        ///  查询结果为指定条数数据，支持设置排序字段
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "SQL条件指定条数查询支持SQL条件设置排序字段测试案例")]
        public async Task QueryTopBySQLWhereOrWithOrderByFiledsAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = id+1,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.InsertAsync(insertUserModels);
            //SQL条件指定条数查询
            var userModels = await _userRepository.QueryTopAsync($"record_state={(int)RecordStateEnum.Normal}", 1, "id desc");
            Assert.Equal(userModels[0].Id, insertUserModels[1].Id);
            userModels = await _userRepository.QueryTopAsync($"record_state={(int)RecordStateEnum.Normal}", 1, "id asc");
            Assert.Equal(userModels[0].Id, insertUserModels[0].Id);
        }

        /// <summary>
        ///   QuerySqlAsync_Test SQL语句查询测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "SQL语句查询测试案例")]
        public async Task QuerySqlAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = 2,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.InsertAsync(insertUserModels);
            //SQL语句查询
            var userModels = await _userRepository.QuerySqlAsync($"select  * from user where record_state={(int)RecordStateEnum.Normal};");
            Assert.Equal(2, userModels.Count);
            userModels = await _userRepository.QuerySqlAsync($"select  * from user where record_state=@recordState;", new { recordState = (int)RecordStateEnum.Normal });
            Assert.Equal(2, userModels.Count);
        }


        /// <summary>
        ///   QueryTableAsync_Test SQL语句表格查询测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "SQL语句表格查询测试案例")]
        public async Task QueryTableAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = 2,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.InsertAsync(insertUserModels);
            //SQL语句表格查询
            var userTable = await _userRepository.QueryTableAsync($"select  * from user where record_state={(int)RecordStateEnum.Normal};");
            Assert.Equal(2, userTable.Rows.Count);
            userTable = await _userRepository.QueryTableAsync($"select  * from user where record_state=@recordState;", new { recordState = (int)RecordStateEnum.Normal });
            Assert.Equal(2, userTable.Rows.Count);
        }

        /// <summary>
        ///   QueryPageByLinqWhereOrWithOrderByFiledsAsync_Test Lambda条件表达式多条指定容量分页查询支持SQL指定排序字段测试案例
        /// <remarks>
        ///  查询结果为多条数据，支持SQL指定排序字段
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式多条指定容量分页查询支持SQL指定排序字段测试案例")]
        public async Task QueryPageByLinqWhereOrWithOrderByFiledsAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = id+1,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.InsertAsync(insertUserModels);
            //Lambda条件表达式多条分页查询
            var userPageModel = await _userRepository.QueryPageAsync(s => s.RecordState == RecordStateEnum.Normal, 1, 200, "id desc");
            Assert.Equal(2, userPageModel.TotalCount);
            Assert.Equal(userPageModel.Data?[0].Id ?? 0, insertUserModels[1].Id);
            userPageModel = await _userRepository.QueryPageAsync(s => s.RecordState == RecordStateEnum.Normal, 1, 200, "id asc");
            Assert.Equal(2, userPageModel.TotalCount);
            Assert.Equal(userPageModel.Data?[0].Id ?? 0, insertUserModels[0].Id);
        }

        /// <summary>
        ///   QueryCountByLinqWhereAsync_Test Lambda条件表达式查询记录数测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式查询记录数测试案例")]
        public async Task QueryCountByLinqWhereAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = 2,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.InsertAsync(insertUserModels);
            //Lambda条件表达式多条查询记录数
            var userCount = await _userRepository.CountAsync(s => s.RecordState == RecordStateEnum.Normal);
            Assert.Equal(2, userCount);
        }

        /// <summary>
        ///   QueryAnyByLinqWhereAsync_Test Lambda条件表达式查询是否存在测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式查询是否存在测试案例")]
        public async Task QueryAnyByLinqWhereAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var insertUserModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = 2,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.InsertAsync(insertUserModels);
            //Lambda条件表达式多条查询是否存在
            var userAny = await _userRepository.AnyAsync(s => s.RecordState == RecordStateEnum.Normal);
            Assert.True(userAny);
        }

        #endregion

        #region QueryForEachAsync查询测试案例

        /// <summary>
        ///   QueryForEachByLinqWhereAsync_Test Lambda条件表达式分批查询测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda条件表达式分批查询测试案例")]
        public async Task QueryForEachByLinqWhereAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            //Lambda条件表达式查询
            var userModels = await _userRepository.QueryForEachAsync(s => s.RecordState == RecordStateEnum.Normal);
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }

        #endregion

        #region InsertAsync插入测试案例

        /// <summary>
        ///   SingleInsertAsync_Test 单条实体插入测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体插入测试案例")]
        public async Task SingleInsertAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            //查询全部
            var userModels = await _userRepository.QueryAllAsync();
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }

        /// <summary>
        ///   AppointInsertColumnsSingleInsertAsync_Test 单条实体指定列插入测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体指定列插入测试案例")]
        public async Task AppointInsertColumnsSingleInsertAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456",
                Gender = GenderEnum.Boy
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel, s => new { s.Id, s.UserName, s.CreationDate, s.LastModified, s.RecordState, s.Gender });
            //指定插入列
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id, "password");
            Assert.NotNull(userModel);
            Assert.Empty(userModel.Password);
        }

        /// <summary>
        ///   ListInsertAsync_Test 多条实体插入测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体插入测试案例")]
        public async Task ListInsertAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = id1,
                UserName = "小化",
                Password = "123456"
              }
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModels);
            //查询全部
            userModels = await _userRepository.QueryAllAsync();
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }


        /// <summary>
        ///   AppointListInsertAsync_Test 多条实体指定列插入测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体指定列插入测试案例")]
        public async Task AppointListInsertAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456",
                Gender=GenderEnum.Boy
              },
              new User
              {
                Id = id1,
                UserName = "小明",
                Password = "123456",
                Gender=GenderEnum.Boy
              }
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModels, s => new { s.Id, s.UserName, s.CreationDate, s.LastModified, s.RecordState, s.Gender });
            //查询全部
            userModels = await _userRepository.QueryAllAsync();
            var passwords = userModels.Select(s => s.Password).Where(s => s.IsNotNullOrWhiteSpace()).ToList();
            Assert.Empty(passwords);
        }

        #endregion

        #region UpdateAsync更新测试案例

        /// <summary>
        ///   UpdateAsync_Test 单条实体更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体更新测试案例")]
        public async Task UpdateAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            //更新数据
            userModel.Password = "654321";
            var updateSuccessedCount = await _userRepository.UpdateAsync(userModel);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   UpdateWithUpdateColumnsAsync_Test 单条实体指定列更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体指定列更新测试案例")]
        public async Task UpdateWithUpdateColumnsAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            //更新数据
            userModel.Password = "654321";
            var updateSuccessedCount = await _userRepository.UpdateAsync(userModel, s => new { s.Password });
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   UpdateWithUpdateColumnsAndLinqWhereAsync_Test 单条实体指定列Lambda表达式条件更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体指定列Lambda表达式条件更新测试案例")]
        public async Task UpdateWithUpdateColumnsAndLinqWhereAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            //更新数据
            userModel.Password = "654321";
            var updateSuccessedCount = await _userRepository.UpdateAsync(userModel, s => new { s.Password }, s => s.Id == id);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            updateSuccessedCount = await _userRepository.UpdateAsync(userModel, s => new { s.Password }, s => s.Id == 2);
            Assert.Equal(0, updateSuccessedCount);
        }

        /// <summary>
        ///   UpdateWithSQLWhereAsync_Test 单条实体SQL条件更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体SQL条件更新测试案例")]
        public async Task SingleUpdateWithSQLWhereAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            //更新数据
            userModel.Password = "654321";
            var updateSuccessedCount = await _userRepository.UpdateAsync(userModel, $"id={id}");
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   UpdateWithLinqWhereAsync_Test 单条实体Lambda表达式条件更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体Lambda表达式条件更新测试案例")]
        public async Task SingleUpdateWithLinqWhereAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            //更新数据
            userModel.Password = "654321";
            var updateSuccessedCount = await _userRepository.UpdateAsync(userModel, s => s.Id == id);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   UpdateWithSetColumnsAndLinqWhereAsync_Test Lambda表达式条件动态设置更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda表达式条件动态设置更新测试案例")]
        public async Task UpdateWithSetColumnsAndLinqWhereAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456",
                Birthday = DateTime.Now
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            var updateSuccessedCount = await _userRepository.UpdateAsync(s => new User { Password = "654321" }, s => s.Id == id);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   ListUpdateAsync_Test 多条实体更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体更新测试案例")]
        public async Task ListUpdateAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456",
                        Birthday = DateTime.Now
              },
              new User
              {
                Id = 2,
                UserName = "小红",
                Password = "123456"
              }
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModels);
            //更新数据
            userModels.ForEach(s => s.Password = "654321");
            var updateSuccessedCount = await _userRepository.UpdateAsync(userModels);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            var userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == 2);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   AppointUpdateColumnsListUpdateAsync_Test 多条实体指定列更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体指定列更新测试案例")]
        public async Task AppointUpdateColumnsListUpdateAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = 2,
                UserName = "小明",
                Password = "123456"
              }
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModels);
            //更新数据
            userModels.ForEach(s => s.Password = "654321");
            var updateSuccessedCount = await _userRepository.UpdateAsync(userModels, s => new { s.Password });
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            var userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == 2);
            Assert.Equal("654321", userModel.Password);
        }


        #endregion

        #region DeleteAsync删除测试案例

        /// <summary>
        ///   DeleteAsync_Test 单条实体删除测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体删除测试案例")]
        public async Task DeleteAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            var deleteSuccessedCount = await _userRepository.DeleteAsync(userModel);
            Assert.Equal(insertSuccessedCount, deleteSuccessedCount);
            //查询全部
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Null(userModel);
        }


        /// <summary>
        ///   DeleteWithLinqWhereAsync_Test Lambda表达式条件删除测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "Lambda表达式条件删除测试案例")]
        public async Task DeleteWithLinqWhereAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            var deleteSuccessedCount = await _userRepository.DeleteAsync(s => s.Id == id);
            Assert.Equal(insertSuccessedCount, deleteSuccessedCount);
            //查询全部
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Null(userModel);
        }


        /// <summary>
        ///   ListDeleteAsync_Test 多条实体删除测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体删除测试案例")]
        public async Task ListDeleteAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
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
            var insertSuccessedCount = await _userRepository.InsertAsync(userModels);
            var deleteSuccessedCount = await _userRepository.DeleteAsync(userModels);
            Assert.Equal(insertSuccessedCount, deleteSuccessedCount);
            //查询全部
            userModels = await _userRepository.QueryAllAsync();
            Assert.Empty(userModels);
        }



        #endregion

        #region  SaveableAsync保存测试案例

        /// <summary>
        ///   SaveableAsync_Test 单条实体保存测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体保存测试案例")]
        public async Task SaveableAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            await _userRepository.SaveableAsync(userModel);
            //查询全部
            var userModels = await _userRepository.QueryAllAsync();
            Assert.NotEmpty(userModels);
            userModel.Password = "654321";
            await _userRepository.SaveableAsync(userModel);
            //查询全部
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }


        /// <summary>
        ///   SaveableWithUpdateColumnsAsync_Test 单条实体指定列保存测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体指定列保存测试案例")]
        public async Task SaveableWithUpdateColumnsAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            await _userRepository.SaveableAsync(userModel);
            //查询全部
            var userModels = await _userRepository.QueryAllAsync();
            Assert.NotEmpty(userModels);
            var modifiedTime = DateTime.Now;
            userModel.Password = "654321";
            userModel.LastModified = modifiedTime;
            await _userRepository.SaveableAsync(userModel, s => new { s.Password });
            //查询全部
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            Assert.NotEqual(userModel.LastModified, modifiedTime);
        }


        /// <summary>
        ///   ListSaveableAsync_Test 多条实体指定列保存测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体保存测试案例")]
        public async Task ListSaveableAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = 2,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.SaveableAsync(userModels);
            //查询全部
            userModels = await _userRepository.QueryAllAsync();
            Assert.Equal(2, userModels.Count);
            userModels.ForEach(s => s.Password = "654321");
            await _userRepository.SaveableAsync(userModels);
            //查询全部
            var userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == 2);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   ListSaveableWithUpdateColumnsAsync_Test 多条实体指定列保存测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体指定列保存测试案例")]
        public async Task ListSaveableWithUpdateColumnsAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = 2,
                UserName = "小明",
                Password = "123456"
              }
            };
            await _userRepository.SaveableAsync(userModels);
            //查询全部
            userModels = await _userRepository.QueryAllAsync();
            Assert.Equal(2, userModels.Count);
            var modifiedTime = DateTime.Now;
            userModels.ForEach(s =>
            {
                s.Password = "654321";
                s.LastModified = modifiedTime;
            });
            await _userRepository.SaveableAsync(userModels, s => new { s.Password });
            //查询全部
            var userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            Assert.NotEqual(userModel.LastModified, modifiedTime);
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == 2);
            Assert.Equal("654321", userModel.Password);
            Assert.NotEqual(userModel.LastModified, modifiedTime);
        }

        #endregion

        #region  BulkCopyAsync大数据插入测试案例

        /// <summary>
        ///   ListBulkCopyAsync_Test 大数据实体插入测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "大数据实体插入测试案例")]
        public async Task ListBulkCopyAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = 2,
                UserName = "小明",
                Password = "123456"
              }
            };
            var insertSuccessedCount = await _userRepository.BulkCopyAsync(userModels);
            //查询全部
            userModels = await _userRepository.QueryAllAsync();
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }


        /// <summary>
        ///   DataTableBulkCopyAsync_Test 大数据DataTable插入测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "大数据DataTable插入测试案例")]
        public async Task DataTableBulkCopyAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var dataTable = new DataTable();
            dataTable.Columns.Add("id", typeof(long));
            dataTable.Columns.Add("create_time", typeof(DateTime));
            dataTable.Columns.Add("modify_time", typeof(DateTime));
            dataTable.Columns.Add("record_state", typeof(int));
            dataTable.Columns.Add("user_name", typeof(string));
            dataTable.Columns.Add("password", typeof(string));
            dataTable.Columns.Add("gender", typeof(int));
            dataTable.Rows.Add(1, DateTime.Now, DateTime.Now, 1, "小明", "123456", GenderEnum.Boy);
            dataTable.Rows.Add(2, DateTime.Now, DateTime.Now, 1, "小花", "123456", GenderEnum.Girl);

            var insertSuccessedCount = await _userRepository.BulkCopyAsync("user", dataTable);
            //查询全部
            var userModels = await _userRepository.QueryAllAsync();
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }


        #endregion

        #region  BulkCopyAsync大数据更新测试案例

        /// <summary>
        ///   ListBulkUpdateAsync_Test 大数据实体更新测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "大数据实体更新测试案例")]
        public async Task ListBulkUpdateAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456",
                Birthday = DateTime.Now
              },
              new User
              {
                Id = id1,
                UserName = "小明",
                Password = "123456",
                Birthday = DateTime.Now
              }
            };
            var insertSuccessedCount = await _userRepository.BulkCopyAsync(userModels);
            userModels.ForEach(s => s.Password = "654321");
            var updateSuccessedCount = await _userRepository.BulkUpdateAsync(userModels);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            var userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id1);
            Assert.Equal("654321", userModel.Password);
        }

        #endregion

        #region ExecuteCommandAsyncSQL执行测试案例

        /// <summary>
        ///   ExecuteCommandAsync_Test SQL执行测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "SQL执行测试案例")]
        public async Task ExecuteCommandAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //创建ID标识
            var id1 = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
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
            //更新数据
            var sql = "update user set password = '654321' where id = @id";
            await _userRepository.ExecuteCommandAsync(sql, new { id = id });
            var userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            //更新数据
            sql = "update user set password = '654321' where id = @id";
            await _userRepository.ExecuteCommandAsync(sql, new { id = id1 });
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id1);
            Assert.Equal("654321", userModel.Password);
        }

        #endregion

        #region InsertQueue插入测试案例

        /// <summary>
        ///   InsertQueueAsync_Test 单条实体插入提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体插入提交测试案例")]
        public async Task InsertQueueAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            _userRepository.InsertQueue(userModel);
            var insertSuccessedCount = await _userRepository.SaveQueuesAsync(true);
            //查询全部
            var userModels = await _userRepository.QueryAllAsync();
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }

        /// <summary>
        ///   AppointInsertQueueAsync_Test 单条实体指定列插入提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体指定列插入提交测试案例")]
        public async Task AppointInsertQueueAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456",
                Gender = GenderEnum.Boy
            };
            _userRepository.InsertQueue(userModel, s => new { s.Id, s.UserName, s.CreationDate, s.LastModified, s.RecordState, s.Gender });
            var insertSuccessedCount = await _userRepository.SaveQueuesAsync(true);
            //指定插入列
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id, "password");
            Assert.NotNull(userModel);
            Assert.Empty(userModel.Password);
        }

        /// <summary>
        ///   ListInsertQueueAsync_Test 多条实体插入提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体插入提交测试案例")]
        public async Task ListInsertQueueAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = 2,
                UserName = "小明",
                Password = "123456"
              }
            };
            _userRepository.InsertQueue(userModels);
            var insertSuccessedCount = await _userRepository.SaveQueuesAsync(true);
            //查询全部
            userModels = await _userRepository.QueryAllAsync();
            Assert.Equal(userModels.Count, insertSuccessedCount);
        }


        /// <summary>
        ///   AppointListInsertQueueAsync_Test 多条实体指定列插入提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体指定列插入提交测试案例")]
        public async Task AppointListInsertQueueAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
                new User
                {
                Id = id,
                UserName = "小明",
                Password = "123456",
                Gender=GenderEnum.Boy
                },
                new User
                {
                Id = 2,
                UserName = "小明",
                Password = "123456",
                Gender=GenderEnum.Boy
                }
            };
            _userRepository.InsertQueue(userModels, s => new { s.Id, s.UserName, s.CreationDate, s.LastModified, s.RecordState, s.Gender });
            var insertSuccessedCount = await _userRepository.SaveQueuesAsync(true);
            //查询全部
            userModels = await _userRepository.QueryAllAsync();
            var passwords = userModels.Select(s => s.Password).Where(s => s.IsNotNullOrWhiteSpace()).ToList();
            Assert.Empty(passwords);
        }


        #endregion

        #region UpdateQueue更新测试案例

        /// <summary>
        ///   UpdateQueueAsync_Test 单条实体更新提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体更新提交测试案例")]
        public async Task UpdateQueueAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            //更新数据
            userModel.Password = "654321";
            _userRepository.UpdateQueue(userModel);
            var updateSuccessedCount = await _userRepository.SaveQueuesAsync(true);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }


        /// <summary>
        ///   UpdateQueueWithUpdateColumnsAsync_Test 单条实体指定列更新提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体指定列更新提交测试案例")]
        public async Task UpdateQueueWithUpdateColumnsAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            //更新数据
            userModel.Password = "654321";
            _userRepository.UpdateQueue(userModel, s => new { s.Password });
            var updateSuccessedCount = await _userRepository.SaveQueuesAsync(true);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }


        /// <summary>
        ///   UpdateQueueWithSetColumnsAndLinqWhereAsync_Test 单条实体Lambda表达式条件动态设置更新提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "单条实体Lambda表达式条件动态设置更新提交测试案例")]
        public async Task UpdateQueueWithSetColumnsAndLinqWhereAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            _userRepository.UpdateQueue(s => new User { Password = "654321" }, s => s.Id == id);
            var updateSuccessedCount = await _userRepository.SaveQueuesAsync(true);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
        }

        /// <summary>
        ///   ListUpdateQueueWithUpdateColumnsAsync_Test 多条实体指定列更新提交测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多条实体指定列更新提交测试案例")]
        public async Task ListUpdateQueueWithUpdateColumnsAsync_Test()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModels = new List<User>
            {
              new User
              {
                Id = id,
                UserName = "小明",
                Password = "123456"
              },
              new User
              {
                Id = 2,
                UserName = "小明",
                Password = "123456"
              }
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModels);
            //更新数据
            userModels.ForEach(s => s.Password = "654321");
            _userRepository.UpdateQueue(userModels, s => new { s.Password });
            var updateSuccessedCount = await _userRepository.SaveQueuesAsync(true);
            Assert.Equal(insertSuccessedCount, updateSuccessedCount);
            //查询全部
            var userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == id);
            Assert.Equal("654321", userModel.Password);
            userModel = await _userRepository.QueryFirstOrDefaultAsync(s => s.Id == 2);
            Assert.Equal("654321", userModel.Password);
        }

        #endregion

        #region 多线程测试案例


        /// <summary>
        ///   ParallelTaskExecuteCommandAsync_Tests 多线程测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "多线程测试案例")]
        public async Task ParallelTaskExecuteCommandAsync_Tests()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            var dbContext1 = _userRepository;
            var supplierModel1 = await dbContext1.QueryFirstOrDefaultAsync(s => s.UserName == "小明");
            var contextID1 = ((ISqlSugarClient)dbContext1.DbContext).ContextID;
            var tasks = new Task[]
            {
                Task.Run(async () =>
                {
                        await SparkScope.CreateAsync(async (serviceScope) =>
                        {
                            //构建数据库上下文
                            var repository = serviceScope.ServiceProvider.GetRequiredService<IDatabaseRepository<User>>();
                            var supplierModel2 = await repository.QueryFirstOrDefaultAsync(s=>s.UserName=="小明");
                            var contextID2 = ((ISqlSugarClient)repository.DbContext).ContextID;
                            Assert.NotEqual(contextID1,contextID2);
                        });
                }),
                Task.Run(async () =>
                {
                        await SparkScope.CreateAsync(async (serviceScope) =>
                        {
                            //构建数据库上下文
                            var repository = serviceScope.ServiceProvider.GetRequiredService<IDatabaseRepository<User>>();
                            var supplierModel3 = await repository.QueryFirstOrDefaultAsync(s=>s.UserName=="小明");
                            var contextID3 = ((ISqlSugarClient)repository.DbContext).ContextID;
                            Assert.NotEqual(contextID1,contextID3);
                        });
                }),
            };
            Task.WaitAll(tasks);
            var dbContext4 = _userRepository;
            var supplierModel4 = await dbContext4.QueryFirstOrDefaultAsync(s => s.UserName == "小明");
            var contextID4 = ((ISqlSugarClient)dbContext4.DbContext).ContextID;
            Assert.Equal(contextID1, contextID4);
        }


        #endregion

        #region 实体特性更改数据库上下文测试案例

        /// <summary>
        ///   CheckAfterAttributeChangeDatabaseDatabase_Tests 实体特性更改数据库上下文测试案例
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName = "实体特性更改数据库上下文测试案例")]
        public async Task CheckAfterAttributeChangeDatabaseDatabase_Tests()
        {
            //创建ID标识
            var id = IDGen.SequentialInt64();
            //初始化表
            _userRepository.TruncateTable();
            //插入数据
            var userModel = new User
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            var insertSuccessedCount = await _userRepository.InsertAsync(userModel);
            var userModels = await _userRepository.QueryAllAsync();
            var userConfigId = ((ISqlSugarClient)_userRepository.DbContext).CurrentConnectionConfig.ConfigId;
            Assert.Equal(userModels.Count, insertSuccessedCount);
            Assert.Equal(userConfigId, "spark");
            //初始化表
            _otherRepository.TruncateTable();
            var otherModel = new Other
            {
                Id = id,
                UserName = "小明",
                Password = "123456"
            };
            insertSuccessedCount = await _otherRepository.InsertAsync(otherModel);
            var otherUserModels = await _otherRepository.QueryAllAsync();
            var otherConfigId = ((ISqlSugarClient)_otherRepository.DbContext).CurrentConnectionConfig.ConfigId;
            Assert.Equal(otherUserModels.Count, insertSuccessedCount);
            Assert.Equal(otherConfigId, "spark1");
            Assert.NotEqual(userConfigId, otherConfigId);
        }

        #endregion

        #endregion
    }
}