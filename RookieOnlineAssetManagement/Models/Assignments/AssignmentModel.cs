using RookieOnlineAssetManagement.Entities;
using RookieOnlineAssetManagement.Enums;
using System;

namespace RookieOnlineAssetManagement.Models.Assignments
{
    public class AssignmentModel
    {
        public int Id { get; set; }
        public string UserIdAssigned { get; set; }
        public int AssetId { get; set; }
        public string FullnameAssigned { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string Specification { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public AssignmentState AssignmentState { get; set; }
        public ReturnAsset ReturnAsset { get; set; }
        public string Note { get; set; }
        public string CategoryName { get; set; }
    }
}