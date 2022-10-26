### 0.2.2

2022-10-26
+ **功能**
  + 字符串扩展类、ICollection集合容器扩展类访问作用域调整为internal仅程序集内部可访问，避免更其他组件冲突。

### 0.2.1

2022-10-26
+ **重命名**
  + 泛型数据仓储IBaseRepository<>重命名为IDatabaseRepository<>


### 0.2.0

2022-10-26
+ **功能**
  + 数据库事务工作单元IDatabaseUnitOfWork
+ **重命名**
  + 数据库上下文IDatabaseContextProvider重命名为IDatabaseContext
  + 非泛型数据仓储IBaseRepository重命名为IDatabaseRepository

### 0.1.0

2022-10-20
+ **功能**
  + 数据库上下文IDatabaseContextProvider
  + 泛型、非泛型数据仓储IBaseRepository、IBaseRepository<>