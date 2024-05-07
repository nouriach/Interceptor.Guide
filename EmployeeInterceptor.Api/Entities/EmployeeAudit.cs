namespace EmployeeInterceptor.Api.Entities;

public class EmployeeAudit
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string Type { get; set; }
    public string FullName { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}