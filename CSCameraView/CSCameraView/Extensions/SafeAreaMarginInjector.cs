using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CSCameraView.Extensions
{
	/// <summary>
	/// Note: This will only work in iOS.
	///
	/// This is an extension class for the <see cref="Xamarin.Forms.Layout{Xamarin.Forms.View}"/>.
	/// </summary>
	public static class SafeAreaMarginInjector
	{

		public static void AddSafeMargin(this View view, Page page)
		{
			if (Device.RuntimePlatform.Equals(Device.iOS))
			{
				NavigationPage.SetHasNavigationBar(page, false);
				NavigationPage.SetHasNavigationBar(page, true);
				Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(page, true);

				var safeAreaMargin = Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.GetSafeAreaInsets(page);
				if (safeAreaMargin != default(Thickness))
				{
					if (page.Height >= page.Width)
					{
						// Add the margin to the view.
						var topMargin = Math.Max(safeAreaMargin.Bottom, Math.Max(safeAreaMargin.Top, Math.Max(safeAreaMargin.Left, safeAreaMargin.Right)));
						view.Margin = new Thickness(0, topMargin, 0, 0);
					}
					else if (page.Width > page.Height)
					{
						var horizontalMargin = Math.Max(safeAreaMargin.Bottom, Math.Max(safeAreaMargin.Top, Math.Max(safeAreaMargin.Left, safeAreaMargin.Right)));
						view.Margin = new Thickness(horizontalMargin, 0);
					}
				}

				Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(page, false);
				NavigationPage.SetHasNavigationBar(page, false);
			}
		}
	}
}
