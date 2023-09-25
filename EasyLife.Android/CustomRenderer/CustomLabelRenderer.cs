using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Widget;
using EasyLife.CustomRenderer;
using EasyLife.Droid.CustomRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(AutoFontSizeLabel),typeof(CustomLabelRenderer))]
namespace EasyLife.Droid.CustomRenderer
{
    public class CustomLabelRenderer : LabelRenderer
    {
        public CustomLabelRenderer (Context context) : base (context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null || !(e.NewElement is AutoFontSizeLabel autoLabel) || Control == null) { return; }

            TextViewCompat.SetAutoSizeTextTypeUniformWithConfiguration(Control, autoLabel.AutoSizeMinTextSize,
                    autoLabel.AutoSizeMaxTextSize, autoLabel.AutoSizeStepGranularity, (int)ComplexUnitType.Sp);

        }
    }
}