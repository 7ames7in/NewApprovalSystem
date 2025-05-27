using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using MediatR;
using ApprovalService.Application.Commands;

var services = new ServiceCollection();

// Logging
services.AddLogging(config => config.AddConsole());

// MediatR 등록
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SubmitApprovalCommand).Assembly));

// 필요 시 Repository, UnitOfWork 등의 Mock 또는 Fake 등록
// services.AddScoped<IApprovalRepository, InMemoryApprovalRepository>();

using var provider = services.BuildServiceProvider();
var logger = provider.GetRequiredService<ILoggerFactory>().CreateLogger("Runner");

var mediator = provider.GetRequiredService<IMediator>();

logger.LogInformation("== ApprovalService.Application 테스트 시작 ==");

// 예시 Command 실행
var result = await mediator.Send(new SubmitApprovalCommand
{
    RequestId = Guid.NewGuid().ToString(),
    ApproverId = "A001",
    ApproverName = "Kim Minsoo"
});

logger.LogInformation("생성된 Approval ID: {Result}", result);
