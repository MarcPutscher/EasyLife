using EasyLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class KronosOverlay_Popup : Popup
    {
        public static DialogOption_Popup HomePopup; 
        public KronosOverlay_Popup()
        {
            InitializeComponent();

            volumeSlider.Value = App.Kronos.volume;
            pitchSlider.Value = App.Kronos.pitch;
            AssistantDialogOption assistantDialogOption = new AssistantDialogOption() { Answer= App.Kronos.intruduction};
            voicetest.Text = "Teste mich";
            voicetest.Command = assistantDialogOption.Speech_Solution;
        }

        private async void Popup_Opened(object sender, PopupOpenedEventArgs e)
        {
            locale = await TextToSpeech.GetLocalesAsync();

            foreach (var l in locale)
                Languages.Add(l.Name);

            languagePicker.ItemsSource = Languages;
            languagePicker.SelectedItem = App.Kronos.locale.Name;
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        private void languagePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var result = sender as Picker;

            string input = result.SelectedItem as string;

            if(String.IsNullOrEmpty(input) == false)
            {
                App.Kronos.locale = locale.Single(l => l.Name == input);
                Preferences.Set("Kronos_Language", input);
            }
        }

        private void volumeSlider_DragCompleted(object sender, EventArgs e)
        {
            var result = sender as Slider;

            float input = (float)result.Value;

            App.Kronos.volume = input;

            Preferences.Set("Kronos_Volume", input);
        }

        private void pitchSlider_DragCompleted(object sender, EventArgs e)
        {
            var result = sender as Slider;

            float input = (float)result.Value;

            App.Kronos.pitch = input;

            Preferences.Set("Kronos_Pitch", input);
        }

        public IEnumerable<Locale> locale;

        public List<string> Languages = new List<string>();

        private async void Home_Clicked(object sender, EventArgs e)
        {
            HomePopup = new DialogOption_Popup("Home");
            await Shell.Current.ShowPopupAsync(HomePopup);
        }
    }
}