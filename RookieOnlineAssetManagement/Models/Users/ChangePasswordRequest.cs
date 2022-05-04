using System;

namespace RookieOnlineAssetManagement.Models.Users
{
    public class ChangePasswordRequest 
    {
        public string UserId { get; set; }
        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
    }
}
