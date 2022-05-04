using Microsoft.AspNetCore.Identity;
using RookieOnlineAssetManagement.Enums;
using System;
using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public string LocationId { get; set; }
        public DateTime JoinedDate { get; set; }
        public Gender Gender { get; set; }
        public string UserCode { get; private set; }
        public Status Status { get; set; }
        public int Serial { get; set; }
        public bool IsNew { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Location Location { get; set; }
        public List<Assignment> Assignments { get; set; }
        public List<ReturnAsset> ReturnAssets { get; set; }
    }
}