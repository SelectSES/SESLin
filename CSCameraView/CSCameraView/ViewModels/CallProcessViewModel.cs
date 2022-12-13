using CSCameraView.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CSCameraView.ViewModels
{
    public class CallProcessViewModel : BaseViewModel
    {
        private bool IsBtnAvailable;
        #region Constructors
        public CallProcessViewModel()
        {
            IsBtnAvailable = true;
            AllowEntryCommand = new Command(AllowEntryCommandExecute);
            DenyEntryCommand = new Command(DenyEntryCommandExecute);
        }
        #endregion

        #region Properties
        
        #endregion

        #region Commands
        public ICommand AllowEntryCommand { get; set; }
        public ICommand DenyEntryCommand { get; set; }
        #endregion

        #region Command Methods
        private void AllowEntryCommandExecute(object obj)
        {
            try
            {
                if (IsBtnAvailable)
                {
                    IsBtnAvailable = false;
                    MessagingCenter.Send(this, "AllowEntry");
                }
            }
            catch (Exception Ex)
            {
                Helper.WriteLog("App entering btnAllowEntry_Clicked state Exception." + Ex.Message + "\n" + Ex.StackTrace);
            }
        }

        private void DenyEntryCommandExecute(object obj)
        {
            try
            {
                if (IsBtnAvailable)
                {
                    IsBtnAvailable = false;
                    MessagingCenter.Send(this, "DenyEntry");
                }
            }
            catch (Exception Ex)
            {
                Helper.WriteLog("App entering btnAllowEntry_Clicked state Exception." + Ex.Message + "\n" + Ex.StackTrace);
            }
        }
        #endregion

        #region Methods
        #endregion

        #region Events
        #endregion
    }
}
