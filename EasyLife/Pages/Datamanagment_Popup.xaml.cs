using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Datamanagment_Popup : Popup
    {
        public Datamanagment_Popup()
        {
            InitializeComponent();
        }

        private void CreateButton_Clicked(object sender, System.EventArgs e)
        {
            Dismiss(1);
        }

        private void ShareButton_Clicked(object sender, System.EventArgs e)
        {
            Dismiss(2);
        }

        private void RestoreButton_Clicked(object sender, System.EventArgs e)
        {
            Dismiss(3);
        }

        private void Show_ConentenButton_Clicked(object sender, System.EventArgs e)
        {
            Dismiss(4);
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }
    }
}