using EmployeeInterceptor.Api.Data;
using EmployeeInterceptor.Api.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/employees", async (DataContext context) =>
{
    return Results.Ok(await context.Employees.ToListAsync());
});

app.MapGet("/employees/{id}", async (DataContext context, int id) =>
{
    var employee = context.Employees.FirstOrDefault(e => e.Id == id);
    if(employee is null)
        return Results.BadRequest($"{id} is not a valid Employee Id.");

    return Results.Ok(employee);
});

app.MapPost("/employees", async (DataContext context, Employee employee) =>
{
    context.Employees.Add(employee);
    await context.SaveChangesAsync();

    return Results.Ok(await context.Employees.ToListAsync());
});

app.MapPut("/employees/{id}", async (DataContext context, Employee employee, int id) =>
{
    var employeeToUpdate = await context.Employees.FirstOrDefaultAsync(e => e.Id == id);
    
    employeeToUpdate.FirstName = employee.FirstName ?? employeeToUpdate.FirstName;
    employeeToUpdate.LastName =  employee.LastName ?? employeeToUpdate.LastName;
    employeeToUpdate.Department = employee.Department ?? employeeToUpdate.Department;
    employeeToUpdate.JobTitle = employee.JobTitle ?? employeeToUpdate.JobTitle ;

    context.Employees.Update(employeeToUpdate);

    await context.SaveChangesAsync();

    return Results.Ok(employeeToUpdate);
});

app.MapDelete("/employees/{id}", async (DataContext context, int id) =>
{
    var employeeToDelete = await context.Employees.FirstOrDefaultAsync(e => e.Id == id);
    if (employeeToDelete != null)
    {
        context.Employees.Remove(employeeToDelete);
        await context.SaveChangesAsync();
    }
    return Results.Ok(await context.Employees.ToListAsync());
});

app.Run();