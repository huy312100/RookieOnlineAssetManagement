using RookieOnlineAssetManagement.Enums;
using System;

namespace RookieOnlineAssetManagement.Models.Assets
{
    public class AssetModel
    {
        public int Id { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string CategoryName { get; set; }
        public string Specification { get; set; }
        public DateTime InstalledDate { get; set; }
        public string LocationId { get; set; }
        public AssetState AssetState { get; set; }
    }
}