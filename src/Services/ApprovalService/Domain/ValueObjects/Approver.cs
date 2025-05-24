namespace ApprovalService.Domain.ValueObjects;

public class Approver
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; }

    public Approver(Guid userId, string name)
    {
        UserId = userId;
        Name = name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Approver other) return false;
        return UserId == other.UserId && Name == other.Name;
    }

    public override int GetHashCode() => HashCode.Combine(UserId, Name);
}
