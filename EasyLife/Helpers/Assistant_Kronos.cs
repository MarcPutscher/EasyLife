using EasyLife.Helpers;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using iText.StyledXmlParser.Jsoup.Nodes;
using Xamarin.Forms.PlatformConfiguration;
using EasyLife.Models;
using EasyLife.Pages;
using EasyLife.Services;
using System.Linq;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;

namespace EasyLife.Helpers
{
    public class Assistant_Kronos
    {
        public Assistant_Kronos(Locale locale, float volume, float pitch)
        {
            this.locale = locale;
            this.volume = volume;
            this.pitch = pitch;
        }

        public async Task Speech(string input)
        {
            if (String.IsNullOrEmpty(input) == false)
            {
                //SpeechSynthesizer synth = new SpeechSynthesizer();
                //PromptBuilder builder = new PromptBuilder();
                //builder.StartVoice(VoiceGender.Female, VoiceAge.Adult, 1);
                //builder.AppendText(input);
                //synth.Speak(builder);

                await TextToSpeech.SpeakAsync(input,
                new SpeechOptions
                {
                    Volume = volume,
                    Pitch = pitch,
                    Locale = locale
                }
                );
            }
        }

        public async Task ShowKronos_Methode(string groupe)
        {
            try
            {
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                IEnumerable<AssistantDialogOption> asdiop = await AssistantDialogOptionService.Get_all_Dialogoption();
                List<AssistantDialogOption> asdiopList = asdiop.Where(x => x.Groupe == groupe).ToList();
                foreach (AssistantDialogOption asiop in asdiopList)
                {
                    keyValuePairs.Add(asiop.Answer, asiop.Question);
                }
                await Shell.Current.ShowPopupAsync(new Assistant_Popup(keyValuePairs));
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public string intruduction = "Hei. Ich bin Kronos und der Assistent in EasyLife.";

        public string intro = "Wie kann ich Ihnen helfen?";

        public string error = "Leider kann ich Ihnen hierbei nicht behilflich sein.";

        public Locale locale { get; set; }

        public float volume { get; set; }

        public float pitch { get; set; }
    }
}

