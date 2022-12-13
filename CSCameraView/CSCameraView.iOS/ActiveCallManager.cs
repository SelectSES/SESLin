using CoreFoundation;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

using CallKit;
using Xamarin.Forms;

namespace CSCameraView.iOS
{
    /// <summary>
    /// This Call used to handle the Callkit System Call 
    /// </summary>
    public  class ActiveCallManager
    {
        #region Private Variables
       private static CXCallController CallController = new CXCallController();
        #endregion

        #region Computed Properties
       
        public  List<ActiveCall> Calls = new List<ActiveCall>();

        #endregion

        #region Constructors
        public ActiveCallManager()
        {
            // Initialize
           this.Calls = new List<ActiveCall>();
        }
        #endregion

        #region Private Methods
        private static void SendTransactionRequest(CXTransaction transaction)
        {
            // Send request to call controller
            CallController.RequestTransaction(transaction, (error) => {
                // Was there an error?
                if (error == null)
                {
                    // No, report success
                    Console.WriteLine("Transaction request sent successfully.");
                }
                else
                {
                    // Yes, report error
                    Console.WriteLine("Error requesting transaction: {0}", error);
                }
            });
        }
        #endregion

        #region Public Methods
        public  ActiveCall FindCall(NSUuid uuid)
        {
            // Scan for requested call
            foreach (ActiveCall call in Calls)
            {
                if (call.UUID.Equals(uuid)) return call;
            }

            // Not found
            return null;
        }
        public void StartCall()
        {

           
            // Build call action
            var handle = new CXHandle(CXHandleType.Generic,AppDelegate.fromNO);
            var startCallAction = new CXStartCallAction(AppDelegate.activeCallUuid, handle);


            // Create transaction
            var transaction = new CXTransaction(startCallAction);

            // Inform system of call request
            SendTransactionRequest(transaction);
        }


        public  void EndCall()
        {
            // Build action
           
            var endCallAction = new CXEndCallAction(AppDelegate.activeCallUuid);
            
         
            // Create transaction
            var transaction = new CXTransaction(endCallAction);

            // Inform system of call request
            SendTransactionRequest(transaction);
        }


        
        #endregion
    }

    

}