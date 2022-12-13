using System;
using System.Collections.Generic;
using System.Text;

namespace CSCameraView.Models
{
    public class LogModel
    {
        public string StackStrace { get; set; }
        public string ErrorMessage { get; set; }

        public Guid MemberId { get; set; }
        public string MethodName { get; set; }
    }
}
