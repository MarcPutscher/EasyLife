using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyLife.Interfaces;
using EasyLife.Models;
using FreshMvvm;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EasyLife.PageModels
{
    public class Pluto_PageModel : FreshBasePageModel
    {
        public Pluto_PageModel() 
        {
            Speech_Command = new AsyncCommand(Speech);
            Listen_Command = new AsyncCommand(Listen);
            ViewIsAppearing_Command = new AsyncCommand(Appearing);

            CancelListen_Command = new Xamarin.Forms.Command(CancelListen);
            Languages = new ObservableRangeCollection<string>();
        }

        public async Task Appearing()
        {
            locale = await TextToSpeech.GetLocalesAsync();

            foreach (var l in locale)
                Languages.Add(l.Name);
        }

        public async Task Speech()
        {
            if(String.IsNullOrEmpty(Selected_Language) == false && String.IsNullOrEmpty(Input) == false)
            {
                await TextToSpeech.SpeakAsync(Input, 
                    new SpeechOptions
                    {
                        Locale = locale.Single(l => l.Name == Selected_Language)
                    }
                );
            }

        }

        public async Task Listen()
        {

            var isAuthorized = await speechToText.RequestPermissions();
            if (isAuthorized)
            {
                try
                {
                    Input = await speechToText.Listen(CultureInfo.GetCultureInfo("en-us"),
                        new Progress<string>(partialText =>
                        {
                            if (DeviceInfo.Platform == DevicePlatform.Android)
                            {
                                Input = partialText;
                            }
                            else
                            {
                                Input += partialText + " ";
                            }
                        }), tokenSource.Token);
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Permission Error", "No microphone access", "OK");
            }
        }

        public void CancelListen()
        {
            tokenSource?.Cancel();
        }

        private ISpeechToText speechToText;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        public AsyncCommand Speech_Command { get; }

        public AsyncCommand Listen_Command { get; }

        public AsyncCommand ViewIsAppearing_Command { get; }

        public Xamarin.Forms.Command CancelListen_Command;

        public string input = "Wartet auf Input....";
        public string Input
        {
            get { return input; }
            set
            {
                if (input == value)
                {
                    return;
                }

                input = value; RaisePropertyChanged();
            }
        }

        public string selected_language ;
        public string Selected_Language
        {
            get { return selected_language; }
            set
            {
                if (selected_language == value)
                {
                    return;
                }

                selected_language = value; RaisePropertyChanged();
            }
        }

        public IEnumerable<Locale> locale;

        public ObservableRangeCollection<string> languages;

        public ObservableRangeCollection<string> Languages
        {
            get { return languages; }
            set
            {
                if (languages == value)
                {
                    return;
                }
                languages = value; RaisePropertyChanged();
            }
        }
    }
}
