using System;
using System.Collections.Generic;
using System.Text;

namespace CSCameraView.Dependency
{
    public interface ISystemCallHelper
    {

        void AcceptCall();
        void TerminateCall();

        void ShowAnswerCallUI();


    }
}
