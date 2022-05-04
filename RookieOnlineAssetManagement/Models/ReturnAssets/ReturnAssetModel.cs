using RookieOnlineAssetManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RookieOnlineAssetManagement.Models.ReturnAssets
{
    public class ReturnAssetModel
    {
        public int Id { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string RequestedBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public string AcceptedBy { get; set; }
        public DateTime ReturnedDate { get; set; }
        public ReturnAssetState ReturnAssetState { get; set; }
    }
}
