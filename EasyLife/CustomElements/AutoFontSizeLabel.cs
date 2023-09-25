using EasyLife.CustomRenderer;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EasyLife.CustomRenderer
{
    public class AutoFontSizeLabel : Label
    {
        public int AutoSizeMaxTextSize
        {
            get => (int)GetValue(AutoSizeMaxTextSizeProperty);
            set => SetValue(AutoSizeMaxTextSizeProperty, value);
        }

        public static readonly BindableProperty AutoSizeMaxTextSizeProperty = BindableProperty.Create(
            nameof(AutoSizeMaxTextSize),      
            typeof(int),     
            typeof(AutoFontSizeLabel));      

        public int AutoSizeMinTextSize
        {
            get => (int)GetValue(AutoSizeMinTextSizeProperty);
            set => SetValue(AutoSizeMinTextSizeProperty, value);
        }

        public static readonly BindableProperty AutoSizeMinTextSizeProperty = BindableProperty.Create(
            nameof(AutoSizeMinTextSize),        
            typeof(int),     
            typeof(AutoFontSizeLabel));      


        public int AutoSizeStepGranularity
        {
            get => (int)GetValue(AutoSizeStepGranularityProperty);
            set => SetValue(AutoSizeStepGranularityProperty, value);
        }

        public static readonly BindableProperty AutoSizeStepGranularityProperty = BindableProperty.Create(
            nameof(AutoSizeStepGranularity),       
            typeof(int),    
            typeof(AutoFontSizeLabel));     
    }
}
