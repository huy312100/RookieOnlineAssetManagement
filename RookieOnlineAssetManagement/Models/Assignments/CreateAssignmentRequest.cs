using System;
using System.ComponentModel.DataAnnotations;

namespace RookieOnlineAssetManagement.Models.Assignments
{
    public class CreateAssignmentRequest
    {
        public string UserId { get; set; }
        public int AssetId { get; set; }
        public DateTime AssignedDate { get; set; }
        public string? Note { get; set; }
    }
}