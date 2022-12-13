using System;
using System.Collections.Generic;
using System.Text;

namespace CSCameraView.Models
{
    public class UserToken
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool IsFirstLogin { get; set; }
        public string AccessToken { get; set; }
        public string Name { get; set; }
        public string sipDomain { get; set; }
        public string Password { get; set; }
        public string refreshToken { get; set; }
        public DateTime refreshTokenExpiration { get; set; }
        public string UserType { get; set; }

        public bool IsLoggingEnabled { get; set; }
    }
}
