using ApprovalService.Domain.Entities; // Importing domain entities for ApprovalService
using System.Threading.Tasks; // Importing Task for asynchronous operations

namespace ApprovalService.Application.Interfaces
{
    /// <summary>
    /// Interface for Approval Repository to handle approval-related data operations.
    /// </summary>
    public interface IApprovalRepository
    {
        /// <summary>
        /// Adds a new approval request asynchronously.
        /// </summary>
        /// <param name="approval">The approval request to be added.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task AddAsync(ApprovalRequest approval);

        /// <summary>
        /// Saves changes to the data source asynchronously.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task SaveChangesAsync();
    }
}
