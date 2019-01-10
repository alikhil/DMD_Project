# Publication Management System

[Specification](https://drive.google.com/folderview?id=0B_Nb0cHQ1W0Qfnc5UTdoQ2ZiOWp2aU9GRXlaenFWNF8zVFUtdFU5bEp3eHFEdU5sWDJJdzg&usp=sharing&tid=0B4PiBKFpK5wxfkU1Zm9JZkxFSUJTYW9NdzlYeHNBT2tIMVNwcENjM3RnajV5b3QzZWo2Z3M)
### Demo Video

[![image](https://cloud.githubusercontent.com/assets/7482065/16802995/fd575be2-4918-11e6-8497-8a5d28f7f664.png)
](https://youtu.be/vjk8IyQYCz0 "Demonstration Video")
## Quick start

#### What you need?
* Visual Studio 2013 or newer
* PostgreSQL 9.3 or newer

#### Configure database
There are must be database named '**PMS**' with owner '**postgres**' whose password '**postgres**'

0. Download [data.sql](https://drive.google.com/file/d/0B9PqrLKb-mQKbnFNb0FXRVpUaDg/view?usp=sharing) and [setup.sql](https://drive.google.com/file/d/0B9PqrLKb-mQKVkx5STNHdndvblE/view?usp=sharing)

1. Execute ```psql -U postgres -d PMS -a -f setup.sql```

2. Execute ```psql -U postgres -d PMS -a -f data.sql```

#### Run app

##### Run throught Visual Studio

1. Open project solution **Project_DMD.sln**
2. Build project(it should install needed Nuget packages)
3. Start app

##### Run on local II Server(Advanced)

Look tutorial [here](http://blogs.msdn.com/b/rickandy/archive/2011/04/22/test-you-asp-net-mvc-or-webforms-application-on-iis-7-in-30-seconds.aspx)

## System Design and Architecture


Firstly, we implemented interfaces for working with fake data which stores in memory and uses LINQ for manipulation with data. Using this ‘fake’ interfaces we could develop front-end despite of that we don’t work with db. In parallel with front-end we implemented interfaces for working with ‘true’ data in database.

Controllers handle requests and calls methods of IDataRepository and IUserRepository interfaces. For developing front-end we used **fake** repositories([FakeDataRepository](https://github.com/alikhil/DMD_Project/blob/master/Project_DMD/Repositories/FakeDataRepository.cs), [FakeAppUserRepository](https://github.com/alikhil/DMD_Project/blob/master/Project_DMD/Repositories/FakeAppUserRepository.cs)) which works with data in CPU and uses LINQ to operate with data. In same time we were implementing **true** repositories([DataRepository](https://github.com/alikhil/DMD_Project/blob/master/Project_DMD/Repositories/DataRepository.cs),  [AppUserRepository](https://github.com/alikhil/DMD_Project/blob/master/Project_DMD/Repositories/AppUserRepository.cs)) they call  [QueryExecutor](https://github.com/alikhil/DMD_Project/blob/master/Project_DMD/Classes/QueryExecutor.cs) which executes hand-written and auto generated sql queries. For simple models we didn't write queries, we developed [AutoSqlGenerator](https://github.com/alikhil/DMD_Project/blob/master/Project_DMD/Classes/AutoSqlGenerator.cs) for that case.

![Application architecture](http://i.imgur.com/ILSSh40.png "Application architecture")
