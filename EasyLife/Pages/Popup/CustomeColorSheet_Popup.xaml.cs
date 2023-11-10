using EasyLife.Helpers;
using EasyLife.Models;
using EasyLife.Services;
using iText.Layout.Borders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomeColorSheet_Popup : Popup
    {
        public List<Color_Template> Color_TemplateList = new List<Color_Template>();

        /// <summary>
        /// Ist ein Popup was eine Liste an Optionen anzeigt die wenn man sie triggert ihren Titel wiedergeben sonst wird null zurückgegeben.
        /// </summary>
        /// <param name="title">Ist der Name des Popups und wird neben dem Abbrechenbutton und oberhalb der Optionen angezeigt.</param>
        /// <param name="width">Ist die Breite die das Popup benötigt</param>
        /// <param name="color_list">Ist eine Liste an Optionen die in dem Popup untereinander angezeigt wird.</param>
        public CustomeColorSheet_Popup(string title,double width, Stylingprofile stylingprofile)
        {
            if(stylingprofile != null)
            {
                Dictionary<string,string> colordic = Stylingprofile_Konverter.Deserilize(stylingprofile);

                foreach(var color in colordic)
                {
                    Color_TemplateList.Add(new Color_Template() { Title = color.Key, Color = color.Value});
                }
            }

            InitializeComponent();

            Title.Text = title;

            if (Color_TemplateList.Count() == 0)
            {
                ColorList.IsVisible = false;
            }
            else
            {
                ColorList.IsVisible = true;

                ColorList.ItemsSource = Color_TemplateList;
            }

            double height = 70 * Color_TemplateList.Count() + 110;

            if (height > 0.9 * (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density))
            {
                height = 0.9 * (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);
            }

            if (width == 0)
            {
                width = 0.9 * (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density);
            }

            if (width > 0.9 * (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density))
            {
                width = 0.9 * (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density);
            }

            CustomeColorSheet.Size = new Size(width, height);

            Title.WidthRequest = width - 75;
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }
    }

    public class Color_Template
    {
        public string Title { get; set; }

        public string Color {  get; set; } 
    }
}