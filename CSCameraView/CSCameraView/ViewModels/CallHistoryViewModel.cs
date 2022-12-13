using CSCameraView.Classes;
using CSCameraView.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCameraView.ViewModels
{
    class CallHistoryViewModel : BaseViewModel
    {
        private WebAPI _WebAPI = new WebAPI();
        #region Constructor
        public CallHistoryViewModel()
        {
            CallHistoryList = new List<CallHistoryModel>();
            GetCallHistory();
        }
        #endregion

        #region Properties
        private List<UserMissedCalls> _UserMissedCallsList;
        public List<UserMissedCalls> UserMissedCallsList
        {
            get
            {
                return _UserMissedCallsList;
            }
            set
            {
                _UserMissedCallsList = value;
                RaisePropertyChanged("UserMissedCallsList");
            }
        }

        private List<CallHistoryModel> _CallHistoryList;
        public List<CallHistoryModel> CallHistoryList
        {
            get
            {
                return _CallHistoryList;
            }
            set
            {
                _CallHistoryList = value;
                RaisePropertyChanged("CallHistoryList");
            }
        }
        #endregion

        #region Commands
        #endregion

        #region Command Methods
        #endregion

        #region Methods
        private async void GetCallHistory()
        {
            //API call
            try
            {
                List<UserMissedCalls> result = await _WebAPI.GetMissedCalls(Settings.UserId);
                UserMissedCallsList = result.OrderByDescending(e => e.CalledOn).ToList();
            }
            catch (Exception ex)
            {
                DisplayException(ex.Message);
            }            
        }
        #endregion
    }
}
