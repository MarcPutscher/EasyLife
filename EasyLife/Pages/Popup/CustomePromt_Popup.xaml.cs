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
    public partial class CustomePromt_Popup : Popup
    {
        /// <summary>
        /// Ist ein Popup was eine Liste an Optionen anzeigt die wenn man sie triggert ihren Titel wiedergeben sonst wird null zurückgegeben.
        /// </summary>
        /// <param name="title">Ist der Name des Popups und wird neben dem Abbrechenbutton und oberhalb der Optionen angezeigt.</param>
        /// <param name="width">Ist die Breite die das Popup benötigt</param>
        /// <param name="height">Ist die Höhe die das Popup benötigt</param>
        /// <param name="accept">Akzeptiert</param>
        /// <param name="refuse">Verwerigert</param>
        /// <param name="placeholder">Lückenfüller</param>
        public CustomePromt_Popup(string title,double width, double height, string accept, string refuse, string placeholder)
        {
            InitializeComponent();

            Title.Text = title;

            if (accept != null)
            {
                AcceptButton.Text = accept;

                AcceptButton.IsVisible = true;
            }
            else
            {
                AcceptButton.IsVisible = false;
            }

            if (refuse != null)
            {
                RefuseButton.Text = refuse;

                RefuseButton.IsVisible = true;
            }
            else
            {
                RefuseButton.IsVisible = false;
            }

            Promt_Entry.Placeholder = placeholder;

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

            Promt_Popup.Size = new Size(width, height);

            Title.WidthRequest = width - 75;
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        private void AcceptButton_Clicked(object sender, EventArgs e)
        {
            var result = Promt_Entry.Text;

            Dismiss(result);
        }
    }
}