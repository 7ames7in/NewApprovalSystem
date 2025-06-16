using System.Net.Http.Json;
using ApprovalPaging.Models;
using ApprovalPaging.Pagination;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main()
    {
        // var serviceProvider = new ServiceCollection()
        //     .AddScoped<IApprovalRequestService, ApprovalRequestApiService>()
        //     .BuildServiceProvider();

        // var approvalRequestService = serviceProvider.GetRequiredService<IApprovalRequestService>();

        HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:5001/api/approvalrequest/")
        };

        var list = await _http.GetFromJsonAsync<IEnumerable<ApprovalRequestViewModel>>("my-requests/EC20505");

        if (list != null)
        {
            var paginator1 = list.ToPaginated(pageSize: 2, comparer: ApprovalRequestViewModel.DateComparer);
            Console.WriteLine($"총 페이지: {paginator1.PagesCount}");
            Console.WriteLine();
            foreach (var page in paginator1)
            {
                Console.WriteLine($"[Page {page.Ordinal}]");
                foreach (var request in page)
                {
                    Console.WriteLine($"{request.RequestTitle} - {request.ApplicantName} - {request.RequestedAt:yyyy-MM-dd}");
                }
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("Error: The list is null.");
        }


        // 샘플 데이터 생성
        var workers = new List<Worker>
        {
            new Worker { Name = "Tom", PayRate = 30 },
            new Worker { Name = "Jane", PayRate = 20 },
            new Worker { Name = "Mike", PayRate = 40 },
            new Worker { Name = "Lucy", PayRate = 25 },
            new Worker { Name = "John", PayRate = 50 },
            new Worker { Name = "Sara", PayRate = 35 },
            new Worker { Name = "Paul", PayRate = 28 },
            new Worker { Name = "Anna", PayRate = 22 },
            new Worker { Name = "Bob", PayRate = 45 },
            new Worker { Name = "Alice", PayRate = 55 },
            new Worker { Name = "Charlie", PayRate = 33 },
            new Worker { Name = "Diana", PayRate = 38 },
            new Worker { Name = "Eve", PayRate = 31 },
            new Worker { Name = "Frank", PayRate = 29 },
            new Worker { Name = "Grace", PayRate = 27 },
            new Worker { Name = "Hank", PayRate = 32 },
            new Worker { Name = "Ivy", PayRate = 26 },
            new Worker { Name = "Jack", PayRate = 34 },
            new Worker { Name = "Kathy", PayRate = 24 },
            new Worker { Name = "Leo", PayRate = 36 },
            new Worker { Name = "Mia", PayRate = 37 },
            new Worker { Name = "Nina", PayRate = 39 },
            new Worker { Name = "Oscar", PayRate = 41 },
            new Worker { Name = "Pete", PayRate = 42 }
        };

        // 페이징 호출
        var paginator = workers.ToPaginated(pageSize: 4, comparer: Worker.RateComparer);

        Console.WriteLine($"총 페이지: {paginator.PagesCount}");
        Console.WriteLine();

        // 페이징 결과 출력
        foreach (var page in paginator)
        {
            Console.WriteLine($"[Page {page.Ordinal}]");
            foreach (var worker in page)
            {
                Console.WriteLine($"{worker.Name} - {worker.PayRate} $");
            }
            Console.WriteLine();
        }

        IPage<Worker> requestedPage = paginator[5-1];

        Console.WriteLine($"[Page {requestedPage.Ordinal}]");
        foreach (var worker in requestedPage)
        {
            Console.WriteLine($"{worker.Name} - {worker.PayRate} $");
        }
    }
}
