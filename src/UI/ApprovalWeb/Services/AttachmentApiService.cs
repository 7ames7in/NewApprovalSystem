using ApprovalWeb.Interfaces;
using ApprovalWeb.Models;

namespace ApprovalWeb.Services;

public class AttachmentApiService : IAttachmentService
{
	private readonly HttpClient _http;

     public AttachmentApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("ApprovalApi");
    }

	public Task<IEnumerable<ApprovalAttachmentViewModel>> GetAttachmentsAsync(string approvalId)
     {
          throw new NotImplementedException();
     }

	public async Task<ApprovalAttachmentViewModel?> GetAttachmentByIdAsync(string attachmentId)
	{
		var attachment = await _http.GetFromJsonAsync<ApprovalAttachmentViewModel>($"api/attachment/{attachmentId}");
          return attachment;
	}

	public Task<bool> AddAttachmentAsync(string approvalId, ApprovalAttachmentViewModel attachment)
	{
		throw new NotImplementedException();
	}

	public Task<bool> DeleteAttachmentAsync(string attachmentId)
	{
		throw new NotImplementedException();
	}

	public Task<bool> UpdateAttachmentAsync(ApprovalAttachmentViewModel attachment)
	{
		throw new NotImplementedException();
	}
}