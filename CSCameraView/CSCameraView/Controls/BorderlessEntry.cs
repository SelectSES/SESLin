using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CSCameraView.Controls
{
    public class BorderlessEntry : Entry
    {

		public Thickness ContentPadding
		{
			get => (Thickness)GetValue(ContentPaddingProperty);
			set => SetValue(ContentPaddingProperty, value);
		}

		// Using a BindableProperty as the backing store for ContentPadding.  This enables animation, styling, binding, etc...
		public static readonly BindableProperty ContentPaddingProperty =
			BindableProperty.Create(nameof(ContentPadding), typeof(Thickness), typeof(BorderlessEntry), default(Thickness),
			propertyChanged: (sender, oldValue, newValue) =>
			{
			});
	}
}
