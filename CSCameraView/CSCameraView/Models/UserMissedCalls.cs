using System;
using System.Collections.Generic;
using System.Text;

namespace CSCameraView.Models
{
    public class UserMissedCalls
    {
        public string PanelSerialNumber { get; set; }
        public string PanelName { get; set; }
        public DateTime CalledOn { get; set; }

        public string CalledOnDisplay 
        {
            get
            {
                return CalledOn.ToLocalTime().ToString("MM-dd-yyyy • hh:mm tt");
            }
        }
    }
}
