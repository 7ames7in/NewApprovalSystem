using ApprovalService.Domain.Entities;
using BuildingBlocks.EventContracts;
using BuildingBlocks.EventBus;
using System.Text;

public class EmailHelper
{
    //private readonly ILogger<EmailHelper> _logger;
    private readonly IServiceProvider _serviceProvider;

    public EmailHelper(IServiceProvider provider)
    {
        //_logger = logger;
        _serviceProvider = provider;
    }

    // 템플릿 HTML - 실제로는 별도 HTML 파일로 분리 가능
    private static readonly string _emailTemplate = @"
    <table width='100%' cellpadding='0' cellspacing='0' border='0' style='font-family: Arial, sans-serif; font-size: 14px; color: #333;'>
        <tr>
            <td align='center' style='background-color: #f7f7f7; padding: 20px;'>
                <h2 style='color: #4CAF50;'>Approval Request Notification</h2>
            </td>
        </tr>
        <tr>
            <td style='padding: 20px;'>
                <p>Dear <strong>{ApplicantName}</strong>,</p>
                <p>Your approval request titled <strong>""{RequestTitle}""</strong> has been created successfully.</p>

                <table width='100%' cellpadding='5' cellspacing='0' border='0' style='border: 1px solid #ddd; margin-top: 10px;'>
                    <tr>
                        <td style='background-color: #f0f0f0; width: 30%;'><strong>Requested At:</strong></td>
                        <td>{RequestedAt}</td>
                    </tr>
                    <tr>
                        <td style='background-color: #f0f0f0;'><strong>Current Step:</strong></td>
                        <td>{CurrentStep}</td>
                    </tr>
                    <tr>
                        <td style='background-color: #f0f0f0;'><strong>Current Approver:</strong></td>
                        <td>{CurrentApproverName}</td>
                    </tr>
                </table>

                <h4 style='margin-top: 20px;'>Approval Steps</h4>
                {StepsList}

                <h4 style='margin-top: 20px;'>Request Content</h4>
                <div style='border: 1px solid #ddd; padding: 10px; background-color: #fafafa;'>
                    {RequestContent}
                </div>

                <h4 style='margin-top: 20px;'>Attachments</h4>
                {AttachmentsList}

                <p style='margin-top: 20px;'>Please review and take the necessary actions.</p>
                <p>If you have any questions, feel free to contact us.</p>
                <p style='color: #999;'>Thank you!<br/>Approval System</p>
            </td>
        </tr>
    </table>";

    public async Task SendEmailAsync(string to, ApprovalRequestWithCurrentStepDto approvalRequest)
    {
        var subject = $"[Approval Request] {approvalRequest.RequestTitle}";

        // Steps HTML 만들기
        var stepsBuilder = new StringBuilder();
        stepsBuilder.Append("<ol style='padding-left: 20px;'>");

        foreach (var step in approvalRequest.Steps)
        {
            stepsBuilder.Append($@"
                <li style='margin-bottom: 5px;'>
                    Step {step.Sequence}: <strong>{step.ApproverName}</strong> - <em>{step.ActionStatus}</em>
                </li>");
        }

        stepsBuilder.Append("</ol>");

        // Attachments HTML 만들기
        var attachmentsBuilder = new StringBuilder();
        attachmentsBuilder.Append("<ul style='padding-left: 20px;'>");

        foreach (var attachment in approvalRequest.Attachments)
        {
            attachmentsBuilder.Append($@"
                <li>
                    <a href='{attachment.FilePath}' style='color: #4CAF50; text-decoration: none;'>{attachment.FileName}</a>
                </li>");
        }

        attachmentsBuilder.Append("</ul>");

        // 템플릿 치환
        var body = _emailTemplate
            .Replace("{ApplicantName}", approvalRequest.ApplicantName ?? "")
            .Replace("{RequestTitle}", approvalRequest.RequestTitle ?? "")
            .Replace("{RequestedAt}", approvalRequest.RequestedAt.ToString("yyyy-MM-dd HH:mm"))
            .Replace("{CurrentStep}", approvalRequest.CurrentStep.ToString())
            .Replace("{CurrentApproverName}", approvalRequest.CurrentApproverName ?? "")
            .Replace("{StepsList}", stepsBuilder.ToString())
            .Replace("{RequestContent}", approvalRequest.RequestContent ?? "")
            .Replace("{AttachmentsList}", attachmentsBuilder.ToString());

        // EmailEvent 로 발행
        var emailEvent = new EmailEvent
        {
            To = to,
            Subject = subject,
            Body = body
        };

        // RabbitMQEventBus 사용
        var rabbitLogger = _serviceProvider.GetService(typeof(ILogger<RabbitMQEventBus>)) as ILogger<RabbitMQEventBus>;
        if (rabbitLogger == null)
        {
            throw new InvalidOperationException("ILogger<RabbitMQEventBus> service is not registered.");
        }
        var eventBus = new RabbitMQEventBus(_serviceProvider, rabbitLogger, "localhost", "guest", "guest");
        await eventBus.PublishAsync(emailEvent);
    }
}