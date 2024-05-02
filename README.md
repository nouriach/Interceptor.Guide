# Using Interceptors with Entity Framework Core 🥷

## Create Audit entries in your Database with Interceptors

Interceptors allow you to step into an Entity Framework Core operation. By creating a concrete class that inherits from `SaveChangesInterceptor` can be powerful. 
`SaveChangesInterceptor` exposes methods that allow you to modify the Saving process.

In our example we will create an application that stores information on Employees. The app will allow us to Create, Read, Update and Delete Employees. For simplicity we will use a SQLite database and DB Browser for SQLite to read the data.

## Read the article 📰

A link to the supporting article can be found here.

## User Instructions 🔖

To interact with this repo you will need to have .NET installed on your machine. You can download the latest .NET version [here]("https://dotnet.microsoft.com/en-us/download").
This app is using .NET 8.

You can run the application by entering `dotnet run` at the root of the project.
Once the application is running you can append `/swagger/index.html` to the end of your port to interact with the endpoints using Swagger.

## Known Issues ⚠️

When an `Employee` entity is created via the `POST "/employees"` endpoint an `EmployeeAudit` entity is saved to the database.
However, as we are intercepting using `SavingChangesAsync` our `Employee` entity doesn't have an `Id` yet.
The `Id` is allocated _after_ the save has completed. As a result, the `EmployeeId` in the `EmployeeAudit` entry is incorrect.

Also, the `POST "/employees"` and `PUT "/employees"` endpoints use the `Employee` object to build a request body.
As a result the `Id` field is exposed. In the future there should be a specific Request object to manage this instead
where the `Id` value is obfuscated.

