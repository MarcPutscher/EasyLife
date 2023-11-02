using EasyLife.Models;
using System;
using System.Security.Cryptography;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home_Remove_Popup : Popup
    {
        public Home_Remove_Popup(bool button1, bool button2, bool button3)
        {
            InitializeComponent();

            Button1.IsVisible = button1;

            Button2.IsVisible = button2;

            Button3.IsVisible = button3;
        }

        private void Button1_Clicked(object sender, System.EventArgs e)
        {
            Dismiss("Diese Transaktion entfernen");
        }

        private void Button2_Clicked(object sender, System.EventArgs e)
        {
            Dismiss("Alle mit dieser Auftrag-ID entfernen");
        }
        private void Button3_Clicked(object sender, System.EventArgs e)
        {
            Dismiss("Alle mit dieser Auftrag-ID ab dieser Transaktion entfernen");
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }
    }
}