using RookieOnlineAssetManagement.Enums;
using System;

namespace RookieOnlineAssetManagement.Models.Users
{
    public class UpdateUserRequest
    {
        public DateTime BirthDay { get; set; }
        public DateTime JoinedDate { get; set; }
        public Gender Gender { get; set; }
        public string Type { get; set; }
    }
}