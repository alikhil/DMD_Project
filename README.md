# Publication Management System

##Quick start

####What you need?
* Visual Studio 2013 or newer
* PostgreSQL 9.3 or newer

####Configure database
There are must be database named '**PMS**' with owner '**postgres**' whose password '**postgres**'

1. Execute ```psql -U postgres -d PMS -a -f setup.sql```

2. Execute ```psql -U postgres -d PMS -a -f data.sql```

####Run app

#####Run throught Visual Studio

1. Open project solution **Project_DMD.sln**
2. Build project(it should install needed Nuget packages)
3. Start app

##### Run on local II Server(Advanced)

Look tutorial [here](http://blogs.msdn.com/b/rickandy/archive/2011/04/22/test-you-asp-net-mvc-or-webforms-application-on-iis-7-in-30-seconds.aspx)

## System Design and Architecture


Firstly, we implemented interfaces for working with fake data which stores in memory and uses LINQ for manipulation with data. Using this ‘fake’ interfaces we could develop front-end despite of that we don’t work with db. In parallel with front-end we implemented interfaces for working with ‘true’ data in database.

![Application architecture](http://i.imgur.com/ILSSh40.png "Application architecture")
