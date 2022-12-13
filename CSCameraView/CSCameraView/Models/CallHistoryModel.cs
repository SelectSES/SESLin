using System;
using System.Collections.Generic;
using System.Text;

namespace CSCameraView.Models
{
    public class CallHistoryModel
    {
        public int id { get; set; }
        public string CallerNumber { get; set; }
        public DateTime CallDateTime { get; set; }
        public TimeSpan CallDuration { get; set; }
        public string TimeAndDurationDisplay
        {
            get
            {
                return $"{CallDateTime.ToString("d MMM . hh:mm tt")} - {CallDuration.TotalMinutes} Minutes";
            }

        }
    }
}
