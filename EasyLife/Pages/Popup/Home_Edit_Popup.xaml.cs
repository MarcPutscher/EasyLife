using EasyLife.Models;
using System;
using System.Security.Cryptography;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home_Edit_Popup : Popup
    {
        public Home_Edit_Popup(bool button1, bool button2, bool button3, bool button4)
        {
            InitializeComponent();

            Button1.IsVisible = button1;

            Button2.IsVisible = button2;

            Button3.IsVisible = button3;

            Button4.IsVisible = button4;
        }

        private void Button1_Clicked(object sender, System.EventArgs e)
        {
            Dismiss("Diese Transaktion bearbeiten");
        }

        private void Button2_Clicked(object sender, System.EventArgs e)
        {
            Dismiss("Alle mit dieser Auftrags-ID bearbeiten");
        }
        private void Button3_Clicked(object sender, System.EventArgs e)
        {
            Dismiss("Alle mit dieser Auftrags-ID ab dieser Transaktion bearbeiten");
        }

        private void Button4_Clicked(object sender, System.EventArgs e)
        {
            Dismiss("Alle mit dieser Auftrags-ID um eine bestimmte Zeit versetzten");
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }
    }
}