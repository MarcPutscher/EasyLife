using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Grouping_Popup : Popup
    {
        public Grouping_Popup(int input)
        {
            InitializeComponent();

            Init( input);
        }

        public int Output;

        public void Init(int input)
        {
            try
            {
                if(input == 1)
                {
                    ReasonButton.BackgroundColor = Xamarin.Forms.Color.LightGray;

                    DateButton.BackgroundColor = Xamarin.Forms.Color.DarkGray;

                    Output = 1;
                }
                if (input == 0)
                {
                    ReasonButton.BackgroundColor = Xamarin.Forms.Color.DarkGray;

                    DateButton.BackgroundColor = Xamarin.Forms.Color.LightGray;

                    Output = 0;
                }
            }
            catch { }
        }

        private void ReasonButton_Clicked(object sender, System.EventArgs e)
        {
            ReasonButton.BackgroundColor = Xamarin.Forms.Color.LightGray;

            DateButton.BackgroundColor = Xamarin.Forms.Color.DarkGray;

            Output = 1;

            Dismiss(Output);
        }

        private void DateButton_Clicked(object sender, System.EventArgs e)
        {
            ReasonButton.BackgroundColor = Xamarin.Forms.Color.DarkGray;

            DateButton.BackgroundColor = Xamarin.Forms.Color.LightGray;

            Output = 0;

            Dismiss(Output);
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }
    }
}