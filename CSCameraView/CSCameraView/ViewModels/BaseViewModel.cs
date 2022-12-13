using CSCameraView.Classes;
using CSCameraView.Dependency;
using CSCameraView.Models;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Xamarin.Forms.Application;

namespace CSCameraView.ViewModels
{
	public class BaseViewModel : ImplPropertyNotifier
	{

		#region Navigations

		public void PushPage(Xamarin.Forms.ContentPage page)
		{
			if (Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
			{
				Current.MainPage.Navigation.PushAsync(page, false);
			}
			else
			{
				Current.MainPage = new Xamarin.Forms.NavigationPage(page);
			}
		}

		public void PopPage()
		{
			if (Current.MainPage.Navigation.NavigationStack.Count > 1)
			{
				Current.MainPage.Navigation.PopAsync(false);
			}
		}

		//ADDED FOR WHEN AT CHANGE MEETING TIME CLICK ON VIEWAVAILABILITY FOR ALL BUTTON AND CLICK SCHEDULE BUTTON AFTER CONFIRM MEETING GO TO DASHBORAD
		public void PopRootPage()
		{
			if (Current.MainPage.Navigation.NavigationStack.Count > 1)
			{
				Current.MainPage.Navigation.PopToRootAsync(false);
			}
		}

		#endregion Navigations

		#region CustomDialogBoxes

		// Await-able call for display message.
		public Task DisplayMessageAsync(string title, string message, string button)
		{
			return Current.MainPage.DisplayAlert(title, message, button);
		}

		public Task<bool> DisplayMessageAsync(string title, string message, string accept, string cancel)
		{
			return Current.MainPage.DisplayAlert(title, message, accept, cancel);
		}

		public void DisplayMessage(string title, string message, string button)
		{
			Current.MainPage.DisplayAlert(title, message, button);
		}

		public Task<string> DisplayActionSheetAsync(string title, string cancel, string destructor, params string[] buttons)
		{
			return Current.MainPage.DisplayActionSheet(title, cancel, destructor, buttons);
		}

		public async void DisplayException(string message)
		{
			await Current.MainPage.DisplayAlert("Error", message, "OK");
		}

		public void DisplayMessage(string title, string message, string button1, string button2)
		{
			Current.MainPage.DisplayAlert(title, message, button1, button2);
		}

		public Task<bool> DisplayAction(string title, string message, string accept, string cancel)
		{
			return Current.MainPage.DisplayAlert(title, message, accept, cancel);
		}

		#endregion CustomDialogBoxes

		#region Internet Connection Availability

		/// <summary>
		/// Use this method before any call which requires Internet connection
		/// </summary>
		/// <returns>TRUE</returns>
		public async Task<bool> HaveInternet()
		{
			if (!DoIHaveInternet())
			{
				await DisplayMessageAsync(Constants.NoInternetConnection, Constants.Failedduetonetworkerror, Constants.OK);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Use this method before any call which requires Internet connection
		/// </summary>
		/// <returns>TRUE</returns>
		public bool HaveInternetWithoutAlert()
		{
			if (!DoIHaveInternet())
			{
				return false;
			}
			return true;
		}

		private bool DoIHaveInternet()
		{
			if (!CrossConnectivity.IsSupported)
			{
				return true;
			}
			var connectivity = CrossConnectivity.Current;
			return connectivity.IsConnected;
		}

		public string GetDeviceId()
		{
			try
			{
				return DependencyService.Get<ICommonDependecyMethodsInterface>().GetDeviceId();
			}
			catch (Exception ex)
			{
				DisplayException(ex.Message);
				return "";
			}
		}


		#endregion Internet Connection Availability
	}
}
