using RookieOnlineAssetManagement.Enums;
using System;

namespace RookieOnlineAssetManagement.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string StaffCode { get; set; }
        public string FullName { get; set; }
        public DateTime JoinedDate { get; set; }
        public DateTime BirthDay { get; set; }
        public Gender Gender { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public bool IsNew { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}