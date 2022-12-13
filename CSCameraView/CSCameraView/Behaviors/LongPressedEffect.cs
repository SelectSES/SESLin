using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CSCameraView.Behaviors
{
    public class LongPressedEffect : RoutingEffect
    {
        public LongPressedEffect() : base("CSCameraView.LongPressedEffect")
        {
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.CreateAttached("Command", typeof(ICommand), typeof(LongPressedEffect), (object)null);
        public static ICommand GetCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(CommandProperty);
        }

        public static void SetCommand(BindableObject view, ICommand value)
        {
            view.SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.CreateAttached("CommandParameter", typeof(object), typeof(LongPressedEffect), (object)null);
        public static object GetCommandParameter(BindableObject view)
        {
            return view.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(BindableObject view, object value)
        {
            view.SetValue(CommandParameterProperty, value);
        }

        public static readonly BindableProperty ClickCommandProperty = BindableProperty.CreateAttached("ClickCommand", typeof(ICommand), typeof(LongPressedEffect), (object)null);
        public static ICommand GetClickCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(ClickCommandProperty);
        }

        public static void SetClickCommand(BindableObject view, ICommand value)
        {
            view.SetValue(ClickCommandProperty, value);
        }

        public static readonly BindableProperty ClickCommandParameterProperty = BindableProperty.CreateAttached("ClickCommandParameter", typeof(object), typeof(LongPressedEffect), (object)null);
        public static object GetClickCommandParameter(BindableObject view)
        {
            return view.GetValue(ClickCommandParameterProperty);
        }

        public static void SetClickCommandParameter(BindableObject view, object value)
        {
            view.SetValue(ClickCommandParameterProperty, value);
        }

    }
}
