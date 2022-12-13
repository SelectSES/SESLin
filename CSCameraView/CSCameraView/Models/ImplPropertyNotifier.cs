using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace CSCameraView.Models
{
   public  class ImplPropertyNotifier : INotifyPropertyChanged
	{

		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (!string.IsNullOrEmpty(propertyName))
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected void SetProperty<T>(ref T referencedVariable, T newValue, [CallerMemberName] string propertyName = "")
		{
			if (!object.Equals(referencedVariable, newValue))
			{
				referencedVariable = newValue;
				RaisePropertyChanged(propertyName);
			}
		}
	}
}
