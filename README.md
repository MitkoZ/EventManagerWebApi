# EventManager
An ASP.NET MVC application for managing events

To start the project you will need Visual Studio (tested with 2017 version) and Microsoft SQL Server (tested with Microsoft SQL Server 2017 Express edition).

For configuring the Data Source change the Data Source attribute in the `DataAccess` layer, `App.config` file.

```
  <connectionStrings>
    <add name="EventManagerDb"
         providerName="System.Data.EntityClient"
         connectionString="Data Source=localhost\SQLEXPRESS"/>
  </connectionStrings>
```
In this situation it is "localhost\SQLEXPRESS".
You can also change the database name. Go to `EventManagerDbContext` (again in the `DataAccess` layer) and change the argument passed to the base constructor (in this case "EventManagerDb"). 

 ```
public EventManagerDbContext() : base("EventManagerDb")
        {

        }
 ```
 
 You will also have to change the `name` attribute in the connection string since the name of the database and the name of the connection string match in this case.
 
Hit the Run button (the green arrow).
The database will get created automatically the first time your query the database.
