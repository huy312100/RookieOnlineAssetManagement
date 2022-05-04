using RookieOnlineAssetManagement.Enums;
using System;
using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string AssignerId { get; set; }
        public int AssetId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public AssignmentState AssignmentState { get; set; }
        public string Note { get; set; }
        public User User { get; set; }
        public Asset Asset { get; set; }
        public List<ReturnAsset> ReturnAssets { get; set; }
    }
}