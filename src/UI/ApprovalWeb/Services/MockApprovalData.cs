using ApprovalWeb.Models;

namespace ApprovalWeb.Services;

public static class MockApprovalData
{
    public static List<ApprovalRequestViewModel> Approvals = new()
    {
        new ApprovalRequestViewModel
        {
            ApprovalId = Guid.NewGuid(),
            RequestTitle = "New Laptop Request",
            RequestContent = "Requesting a new laptop for development work.",
            ApplicantEmployeeNumber = "EMP001",
            ApplicantName = "Alice Smith",
            ApplicantPosition = "Software Engineer",
            ApplicantDepartment = "Development",
            Status = "Pending",
            RequestedAt = DateTime.UtcNow,
            RespondedAt = null,
            ApproverComment = null,
            ApprovalType = "Hardware",
            MisKey = "MIS12345"
        },
        new ApprovalRequestViewModel
        {
            ApprovalId = Guid.NewGuid(),
            RequestTitle = "Conference Attendance",
            RequestContent = "Requesting approval to attend the annual tech conference.",
            ApplicantEmployeeNumber = "EMP002",
            ApplicantName = "Bob Johnson",
            ApplicantPosition = "Product Manager",
            ApplicantDepartment = "Product",
            Status = "Approved",
            RequestedAt = DateTime.UtcNow.AddDays(-2),
            RespondedAt = DateTime.UtcNow.AddDays(-1),
            ApproverComment = "Approved for budget reasons.",
            ApprovalType = "Event",
            MisKey = "MIS67890"
        }
        
    };

    public static ApprovalRequestViewModel? GetByRequestId(string requestId)
    {
        return Approvals.FirstOrDefault(a => a.ApplicantEmployeeNumber == requestId);
    }

    public static void Add(ApprovalRequestViewModel model)
    {
        model.RequestedAt = DateTime.UtcNow;
        model.Status = "Pending";
        Approvals.Add(model);
    }

    public static void UpdateStatus(string requestId, string status, string? comment)
    {
        var item = GetByRequestId(requestId);
        if (item != null)
        {
            item.Status = status;
            //item.Comments = comment;
        }
    }
}
