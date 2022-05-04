using RookieOnlineAssetManagement.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace RookieOnlineAssetManagement.Models.Assets
{
    public class CreateAssetRequest
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9'' ']+$", ErrorMessage = "Asset Name can not contain special character")]
        public string AssetName { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Specification { get; set; }

        [Required]
        public DateTime InstalledDate { get; set; }

        [Required]
        public AssetState AssetState { get; set; }
    }
}