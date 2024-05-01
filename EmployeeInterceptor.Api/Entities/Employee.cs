using EmployeeInterceptor.Api.Interfaces;

namespace EmployeeInterceptor.Api.Entities;

public class Employee : IAuditable
{
    public int Id { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Department { get; set; }
    public string JobTitle { get; set; }
}