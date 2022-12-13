using System;
using System.Collections.Generic;
using System.Text;

namespace CSCameraView.Dependency
{
    public interface BaterySaverPermission
    {

        void StartSetting();

        bool CheckIsEnableBatteryOptimizations();
    }
}
