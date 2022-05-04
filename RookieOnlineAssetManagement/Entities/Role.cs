using Microsoft.AspNetCore.Identity;
using System;

namespace RookieOnlineAssetManagement.Entities
{
    public class Role : IdentityRole
    {
        public string Description { get; set; }
    }
}