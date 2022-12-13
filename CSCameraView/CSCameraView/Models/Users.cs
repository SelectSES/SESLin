using System;
using System.Collections.Generic;
using System.Text;

namespace CSCameraView.Models
{
    public class Users
    {
        public int UserId { get; set; }

        public string DeviceId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsFirstLogin { get; set; }
        public string AccessToken { get; set; }
        public string Name { get; set; }
        public string ApartmentNumber { get; set; }
        public string MobileNumber { get; set; }
        public string LandlineNumber { get; set; }
        public string TwNumber { get; set; }
        public string Extension { get; set; }
        public string EmailId { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string SIPDomain { get; set; }
        public int PanelId { get; set; }
        public string PanelPhoneNumber { get; set; }
        public string TwPhoneNumber { get; set; }
        public string FCMToken { get; set; }
        public string DeviceType { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
    }
}
