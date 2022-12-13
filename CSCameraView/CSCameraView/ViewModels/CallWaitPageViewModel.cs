using CSCameraView.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSCameraView.ViewModels
{
    public class CallWaitPageViewModel : BaseViewModel
    {
        #region Constructor
        public CallWaitPageViewModel()
        {
        }
        #endregion

        #region Properties
        private bool _IsSendLogsVisible;
        public bool IsSendLogsVisible
        {
            get
            {
                return Settings.IsSendLogsBtnVisible;
            }
            set
            {
                Settings.IsSendLogsBtnVisible = value;
                RaisePropertyChanged("IsSendLogsVisible");
            }
        }
        #endregion

        #region Commands
        #endregion

        #region Command Methods
        #endregion

        #region Methods
        #endregion
    }
}
