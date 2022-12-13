using System;
using System.Collections.Generic;
using System.Text;

namespace CSCameraView.Dependency
{
    public interface IAndroidService
    {
        void StartService();

        void StopService();
    }
}
