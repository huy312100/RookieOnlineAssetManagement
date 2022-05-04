using System;
using RookieOnlineAssetManagement.Enums;

namespace RookieOnlineAssetManagement.Models.Assets
{
    public class UpdateAssetRequest
    {
        public string AssetName { get; set; }
        public string Specification { get; set; }
        public DateTime InstalledDate { get; set; }
        public AssetState AssetState { get; set; }
    }
}
