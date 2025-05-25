using ApprovalWeb.Models;

namespace ApprovalWeb.Services;

public static class MockApprovalData
{
    public static List<ApprovalViewModel> Approvals = new()
    {
        new ApprovalViewModel
        {
            RequestId = "REQ001",
            ApproverName = "Alice",
            Status = "Pending",
            RequestedAt = DateTime.UtcNow.AddHours(-5),
            Title = "New Feature Approval",
            Description = "Please review the new feature proposal.",
            Details = "This feature will enhance user experience by providing additional functionality.",
            Comments = null,
            Priority = "High"
        },
        new ApprovalViewModel
        {
            RequestId = "REQ002",
            ApproverName = "Bob",
            Status = "Approved",
            RequestedAt = DateTime.UtcNow.AddHours(-3),
            Title = "Bug Fix Approval",
            Description = "Please review the bug fix request.",
            Details = "This fix addresses critical issues reported by users.",
            Comments = "Approved after review.",
            Priority = "Medium"
        },
        new ApprovalViewModel
        {
            RequestId = "REQ003",
            ApproverName = "Charlie",
            Status = "Rejected",
            RequestedAt = DateTime.UtcNow.AddHours(-1),
            Title = "Performance Improvement Approval",
            Description = "Please review the performance improvement proposal.",
            Details = "This proposal aims to optimize system performance and reduce latency.",
            Comments = "Rejected due to insufficient details.",
            Priority = "Low"
        },
        new ApprovalViewModel
        {
            RequestId = "REQ004",
            ApproverName = "Diana",
            Status = "Pending",
            RequestedAt = DateTime.UtcNow.AddMinutes(-30),
            Title = "Security Update Approval",
            Description = "Please review the security update request.",
            Details = "This update addresses vulnerabilities identified in the last security audit.",
            Comments = null,
            Priority = "High"
        },
        new ApprovalViewModel
        {
            RequestId = "REQ005",
            ApproverName = "Ethan",
            Status = "Pending",
            RequestedAt = DateTime.UtcNow.AddMinutes(-10),
            Title = "UI Enhancement Approval",
            Description = "Please review the UI enhancement proposal.",
            Details = "This enhancement aims to improve user interface consistency and usability.",
            Comments = null,
            Priority = "Medium"
        }
    };

    public static ApprovalViewModel? GetByRequestId(string requestId)
    {
        return Approvals.FirstOrDefault(a => a.RequestId == requestId);
    }

    public static void Add(ApprovalViewModel model)
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
            item.Comments = comment;
        }
    }
}
