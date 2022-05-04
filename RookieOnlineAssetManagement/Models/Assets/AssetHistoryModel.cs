using System;

namespace RookieOnlineAssetManagement.Models.Assets
{
    public class AssetHistoryModel
    {
        public DateTime AssignedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
    }
}