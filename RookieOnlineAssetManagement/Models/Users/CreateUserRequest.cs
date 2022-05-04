using RookieOnlineAssetManagement.Enums;
using System;
using System.Collections.Generic;

namespace RookieOnlineAssetManagement.Models.Users
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime JoinedDate { get; set; }
        public Gender Gender { get; set; }
        public string Type { get; set; }
    }
}