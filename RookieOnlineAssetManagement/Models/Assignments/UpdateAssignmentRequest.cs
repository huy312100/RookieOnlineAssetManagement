using System;
namespace RookieOnlineAssetManagement.Models.Assignments
{
    public class UpdateAssignmentRequest
    {
        public string UserId { get; set; }
        public int NewAssetId { get; set; }
        public int OldAssetId { get; set; }
        public DateTime AssignedDate { get; set; }
        public string? Note { get; set; }
    }
}
