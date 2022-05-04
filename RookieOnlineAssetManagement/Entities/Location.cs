using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Entities
{
    public class Location
    {
        public string Id { get; set; }
        public string LocationName { get; set; }
        public List<User> Users { get; set; }
    }
}