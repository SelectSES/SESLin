using System;
using System.Collections.Generic;
using System.Text;

namespace CSCameraView.Models
{ 
    public class SesCallerModel
    {
        public int panelId  {get;set;}
        public string panelPhoneNumber { get; set; }
        public string videoURL { get; set; }
        public int siteid { get; set; }
        public string siteName { get; set; }
        public int sesNumberId { get; set; }
        public string panelSerialNumber { get; set; }
        public string sesPhoneNumber { get; set; }
        public bool isActive { get; set; }
        public int autoTerminateCall { get; set; }
        public int dtmfDigitForDoorUnloack { get; set; }
        public string panelName { get; set; }
        public string description { get; set; }
        public int capacity { get; set; }

    }
}
