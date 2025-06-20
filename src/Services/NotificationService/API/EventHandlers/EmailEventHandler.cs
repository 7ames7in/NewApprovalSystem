// EmailEventHandler.cs
using BuildingBlocks.EventBus;
using BuildingBlocks.EventContracts;
using SQLitePCL;

namespace NotificationService.API.EventHandlers;

public class EmailEventHandler : IEventHandler<EmailEvent>
{
    private readonly IEmailService _emailService;
    public EmailEventHandler(IEmailService emailService)
    {
        _emailService = emailService;    
    }

    public async Task Handle(EmailEvent @event)
    {
        // 실제 이메일 전송 로직 (예시 출력)
        Console.WriteLine($"📧 Sending email to: {@event.To}");
        Console.WriteLine($"Subject: {@event.Subject}");
        Console.WriteLine($"Body: {@event.Body}");

        //EmailRequest emailRequest = new { To: @event.to, Subject: @event.Subject, Body: @event.Body, }
        ///send-email
        await _emailService.SendEmailAsync(@event);

        // 여기에 이메일 전송 코드를 추가합니다.
        // 예: SMTP 클라이언트 사용, SendGrid API 호출 등
        // 예시로 Task.CompletedTask를 사용하여 비동기 작업을 완료합니다.
        // 실제 구현에서는 이메일 전송 라이브러리나 API를 사용해야 합니다.
        // 예: await _emailService.SendEmailAsync(@event.To, @event.Subject, @event.Body);
        // 여기서는 단순히 콘솔에 출력하는 것으로 대체합니다.
        Console.WriteLine("Email sent successfully!");
        await Task.CompletedTask;
    }
}

public class EmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    public EmailService(IHttpClientFactory factory) {
        _httpClient = factory.CreateClient("EmailServiceApi");
    }
    public async Task<bool> SendEmailAsync(EmailEvent request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/emailservice/send-email", request);
        return await response.Content.ReadFromJsonAsync<bool>();
    }
}

public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailEvent request);
}