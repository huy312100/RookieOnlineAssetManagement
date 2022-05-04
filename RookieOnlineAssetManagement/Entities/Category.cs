using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Prefix { get; set; }
        public List<Asset> Assets { get; set; }
    }
}