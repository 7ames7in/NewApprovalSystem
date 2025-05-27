//namespace ApprovalService.Domain.ValueObjects;
namespace ApprovalService.Domain.Entities;

public class Approver
{
    public string EmployeeNumber { get; private set; }
    public string Name { get; private set; }
    public string Position { get; private set; }
    public string Department { get; private set; }

    public Approver(string employeeNumber, string name)
    {
        EmployeeNumber = employeeNumber;
        Name = name;
        Position = string.Empty; // 기본값 설정
        Department = string.Empty; // 기본값 설정
    }

    public Approver(string employeeNumber, string name, string position, string department) 
        : this(employeeNumber, name)
    {
        Position = position;
        Department = department;
    }
}
