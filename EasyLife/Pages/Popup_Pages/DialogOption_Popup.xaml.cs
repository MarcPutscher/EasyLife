using EasyLife.Models;
using EasyLife.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogOption_Popup : Popup
    {
        public string groupe;
        public DialogOption_Popup(string groupe)
        {
            InitializeComponent();
            this.groupe = groupe;
            captionLabel.Text = ""+groupe+" Dialoge";
        }

        public async void Popup_Opened(object sender, PopupOpenedEventArgs e)
        {
            IEnumerable<AssistantDialogOption> result = await AssistantDialogOptionService.Get_all_Dialogoption();

            dialogoptionList.ItemsSource = result.Where(x=>x.Groupe==groupe);
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        private async void Add_Clicked(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(questionEntry.Text) == false && String.IsNullOrEmpty(answerEntry.Text) == false)
            {
                int result = await AssistantDialogOptionService.Add_Dialogoption(questionEntry.Text,answerEntry.Text,groupe );

                if (result == 1)
                {
                    IEnumerable<AssistantDialogOption> result2 = await AssistantDialogOptionService.Get_all_Dialogoption();

                    dialogoptionList.ItemsSource = result2.Where(x => x.Groupe == groupe);

                    questionEntry.Text = String.Empty;
                    answerEntry.Text = String.Empty;
                }
            }
        }
    }
}