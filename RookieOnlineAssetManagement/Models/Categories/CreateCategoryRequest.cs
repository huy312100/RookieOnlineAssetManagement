using System.ComponentModel.DataAnnotations;

namespace RookieOnlineAssetManagement.Models.Categories
{
    public class CreateCategoryRequest
    {
        [Required]
        public string CategoryName { get; set; }

        [Required]
        [StringLength(maximumLength: 2, MinimumLength = 2, ErrorMessage = "Prefix length must 2 characters.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Prefix can not contain special character or number")]
        public string Prefix { get; set; }
    }
}