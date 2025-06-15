using System;

namespace ApprovalService.API.DTOs;

public class ApproveRequestDto
{
    public string ApprovalId { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public string? CurrentApproverEmployeeNumber { get; set; } // Employee number of the current approver
}
