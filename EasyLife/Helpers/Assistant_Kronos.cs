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

        public string intruduction = "Hei. Ich bin Kronos und der Assistent in EasyLife.";

        public string intro = "Wie kann ich Ihnen helfen?";

        public string error = "Leider kann ich Ihnen hierbei nicht behilflich sein.";

        public Locale locale { get; set; }

        public float volume { get; set; }

        public float pitch { get; set; }
    }
}

