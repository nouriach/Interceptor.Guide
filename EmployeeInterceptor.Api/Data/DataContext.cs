using EmployeeInterceptor.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeInterceptor.Api.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
  
    }
    
    public DbSet<Employee> Employees { get; set; }
}