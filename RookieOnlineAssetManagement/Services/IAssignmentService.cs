using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Models.Assignments;
using RookieOnlineAssetManagement.Models.Common;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Services
{
    public interface IAssignmentService
    {
        public AssignmentModel GetById(int id);

        public Task<int> Create(string accountId, CreateAssignmentRequest request);

        public Task<PagedResult<AssignmentModel>> GetAllPaging(string accountId, GetAssignmentPagingRequest request);

        public Task<Assignment> DeleteAssignment(string accountId, int assingmentId);

        public Task<PagedResult<AssignmentModel>> GetHomePaging(string accountId, GetHomePagingRequest request);

        public Task<string> Update(int assignmentId, UpdateAssignmentRequest request);

        public Task<Assignment> ChangeAssignmentState(int assingmentId);

    }
}