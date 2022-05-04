using RookieOnlineAssetManagement.Enums;
using System;
using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Entities
{
    public class Asset
    {
        public int Id { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public int CategoryId { get; set; }
        public string Specification { get; set; }
        public DateTime InstalledDate { get; set; }
        public string LocationId { get; set; }
        public AssetState AssetState { get; set; }
        public Status Status { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Category Category { get; set; }
        public List<Assignment> Assignments { get; set; }
    }
}