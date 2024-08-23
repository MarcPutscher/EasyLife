using EasyLife.Helpers;
using EasyLife.Models;
using EasyLife.Services;
using Org.BouncyCastle.Tsp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Assistant_Popup : Popup
    {
        public List<AssistantDialogOption> Options = new List<AssistantDialogOption>();

        public Assistant_Popup(Dictionary<string,string> option)
        {
            InitializeComponent();

            if (option?.Count() == 0)
            { Dismiss(null); }

            int follower = 0;
            while (follower < option.Count())
            {
                Options.Add(new AssistantDialogOption(option.ElementAt(follower).Value, option.ElementAt(follower).Key));
                follower++;
            }

            OptionList.ItemsSource = Options.ToList();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                Dismiss(null);
            }
            catch
            {
                Dismiss(null);
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var result = sender as Button;

            AssistantDialogOption input = result.CommandParameter as  AssistantDialogOption;

            if (input != null)
            {
                answerLabel.Text = input.Answer;
            }
        }
    }
}