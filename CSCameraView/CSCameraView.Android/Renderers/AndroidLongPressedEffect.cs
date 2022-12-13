using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CSCameraView.Behaviors;
using CSCameraView.Droid.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("CSCameraView")]
[assembly: ExportEffect(typeof(AndroidLongPressedEffect), "LongPressedEffect")]
namespace CSCameraView.Droid.Renderers
{
    public class AndroidLongPressedEffect : PlatformEffect
    {
        private bool _attached;

        /// <summary>
        /// Initializer to avoid linking out
        /// </summary>
        public static void Initialize() { }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Yukon.Application.AndroidComponents.Effects.AndroidLongPressedEffect"/> class.
        /// Empty constructor required for the odd Xamarin.Forms reflection constructor search
        /// </summary>
        //public AndroidLongPressedEffect()
        //{
        //}

        /// <summary>
        /// Apply the handler
        /// </summary>
        protected override void OnAttached()
        {
            //because an effect can be detached immediately after attached (happens in listview), only attach the handler one time.
            if (!_attached)
            {
                if (Control != null)
                {
                    Control.LongClickable = true;
                    Control.LongClick += Control_LongClick;
                    Control.Clickable = true;
                    Control.Click += Control_Click;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.LongClick += Control_LongClick;
                    Container.Clickable = true;
                    Container.Click += Control_Click;
                }
                _attached = true;
            }
        }

        private void Control_Click(object sender, EventArgs e)
        {
            var command = LongPressedEffect.GetClickCommand(Element);
            command?.Execute(LongPressedEffect.GetClickCommandParameter(Element));
        }

        /// <summary>
        /// Invoke the command if there is one
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void Control_LongClick(object sender, Android.Views.View.LongClickEventArgs e)
        {
            var command = LongPressedEffect.GetCommand(Element);
            command?.Execute(LongPressedEffect.GetCommandParameter(Element));
        }

        /// <summary>
        /// Clean the event handler on detach
        /// </summary>
        protected override void OnDetached()
        {
            if (_attached)
            {
                if (Control != null)
                {
                    Control.LongClickable = true;
                    Control.LongClick -= Control_LongClick;
                    Control.Clickable = true;
                    Control.Click -= Control_Click;
                }
                else
                {
                    Container.LongClickable = true;
                    Container.LongClick -= Control_LongClick;
                    Container.Clickable = true;
                    Container.Click -= Control_Click;
                }
                _attached = false;
            }
        }
    }
}