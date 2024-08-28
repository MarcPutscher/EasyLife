using EasyLife.Interfaces;
using EasyLife.Models;
using EasyLife.Pages;
using EasyLife.Services;
using MvvmHelpers.Commands;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

        public async void Share_DialogOption_Methode(object sender, EventArgs e)
        {
            FileStream sourceStream = null;
            FileStream destinationStream = null;

            string destinationPath = DependencyService.Get<IAccessFile>().CreateFileDocuments("EasyLife_DialogOption.db");
            string sourcePath = Path.Combine(FileSystem.AppDataDirectory, "EasyLife.db");

            try
            {
                sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.ReadWrite);

                if(File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                }

                destinationStream = new FileStream(destinationPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                await AssistantDialogOptionService.CloneTableToNewDB(destinationPath);

                await Share.RequestAsync(new ShareFileRequest { Title = "DialogOptionen", File = new ShareFile(destinationPath) });
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist beim Senden der Dialogoptionen ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
            finally
            {
                sourceStream.Close();

                destinationStream.Close();

                File.Delete(destinationPath);
            }
        }

        private async void Home_Clicked(object sender, EventArgs e)
        {
            dialogoption_popup = new DialogOption_Popup("Home");
            await Shell.Current.ShowPopupAsync(dialogoption_popup);
        }

        private async void Hinzufügen_Clicked(object sender, EventArgs e)
        {
            dialogoption_popup = new DialogOption_Popup("Hinzufügen");
            await Shell.Current.ShowPopupAsync(dialogoption_popup);
        }

        private async void Auftrag_Hinzufügen_Clicked(object sender, EventArgs e)
        {
            dialogoption_popup = new DialogOption_Popup("Auftrag Hinzufügen");
            await Shell.Current.ShowPopupAsync(dialogoption_popup);
        }

        private async void Berarbeiten_Clicked(object sender, EventArgs e)
        {
            dialogoption_popup = new DialogOption_Popup("Berarbeiten");
            await Shell.Current.ShowPopupAsync(dialogoption_popup);
        }

        private async void Auftrag_Berarbeiten_Clicked(object sender, EventArgs e)
        {
            dialogoption_popup = new DialogOption_Popup("Auftrag Berarbeiten");
            await Shell.Current.ShowPopupAsync(dialogoption_popup);
        }

        private async void Bilanz_Clicked(object sender, EventArgs e)
        {
            dialogoption_popup = new DialogOption_Popup("Bilanz");
            await Shell.Current.ShowPopupAsync(dialogoption_popup);
        }

        private async void Vergleichen_Clicked(object sender, EventArgs e)
        {
            dialogoption_popup = new DialogOption_Popup("Vergleichen");
            await Shell.Current.ShowPopupAsync(dialogoption_popup);
        }

        private async void Einstellungen_Clicked(object sender, EventArgs e)
        {
            dialogoption_popup = new DialogOption_Popup("Einstellungen");
            await Shell.Current.ShowPopupAsync(dialogoption_popup);
        }

        private async void Styling_Clicked(object sender, EventArgs e)
        {
            dialogoption_popup = new DialogOption_Popup("Styling");
            await Shell.Current.ShowPopupAsync(dialogoption_popup);
        }

        private async void Papierkorb_Clicked(object sender, EventArgs e)
        {
            dialogoption_popup = new DialogOption_Popup("Papierkorb");
            await Shell.Current.ShowPopupAsync(dialogoption_popup);
        }


        public static DialogOption_Popup dialogoption_popup;

        public IEnumerable<Locale> locale;

        public List<string> Languages = new List<string>();
    }
}