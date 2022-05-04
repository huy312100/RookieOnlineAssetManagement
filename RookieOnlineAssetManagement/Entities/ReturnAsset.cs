using RookieOnlineAssetManagement.Enums;
using System;

namespace RookieOnlineAssetManagement.Entities
{
    public class ReturnAsset
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AssignmentId { get; set; }
        public DateTime ReturnedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public ReturnAssetState ReturnAssetState { get; set; }
        public User User { get; set; }
        public Assignment Assignment { get; set; }
    }
}