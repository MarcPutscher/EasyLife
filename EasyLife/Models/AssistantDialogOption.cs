using EasyLife.Helpers;
using EasyLife.Pages;
using EasyLife.Services;
using MvvmHelpers.Commands;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;
using PropertyChangingEventArgs = System.ComponentModel.PropertyChangingEventArgs;
using PropertyChangingEventHandler = System.ComponentModel.PropertyChangingEventHandler;

namespace EasyLife.Models
{
    public class AssistantDialogOption : INotifyPropertyChanging
    {
        public AssistantDialogOption(string Question, string Answer)
        {
            this.Question = Question;
            this.Answer = Answer;
        }
        public AssistantDialogOption()
        {
        }

        public Assistant_Kronos Kronos = App.Kronos;

        int id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set
            {
                if (Id == value)
                    return;
                id = value; OnPropertyChanged(nameof(Id));
            }
        }

        public string question;
        public string Question
        {
            get { return question; }
            set
            {
                if (Question == value)
                    return;
                question = value; OnPropertyChanged(nameof(Question));
            }
        }

        public string answer;
        public string Answer
        {
            get { return answer; }
            set
            {
                if (Answer == value)
                    return;
                answer = value; OnPropertyChanged(nameof(Answer));
            }
        }
        public string groupe;
        public string Groupe
        {
            get { return groupe; }
            set
            {
                if (Groupe == value)
                    return;
                groupe = value; OnPropertyChanged(nameof(Groupe));
            }
        }

        private Command speech_Solution;

        public ICommand Speech_Solution
        {
            get
            {
                if (speech_Solution == null)
                {
                    speech_Solution = new Command(PerformSpeech_Solution);
                }

                return speech_Solution;
            }
        }

        private Command savechanges_command;

        public ICommand SaveChanges_Command
        {
            get
            {
                if (savechanges_command == null)
                {
                    savechanges_command = new Command(PerformSaveChanges_Command);
                }

                return savechanges_command;
            }
        }

        private Command delete_command;

        public ICommand Delete_Command
        {
            get
            {
                if (delete_command == null)
                {
                    delete_command = new Command(PerformDelete_Command);
                }

                return delete_command;
            }
        }

        private async void PerformSpeech_Solution()
        {
            await Kronos.Speech(Answer);
        }

        private async void PerformSaveChanges_Command()
        {
            await AssistantDialogOptionService.Edit_Dialogoption(this);

            KronosOverlay_Popup.HomePopup?.Popup_Opened(null, null);
        }

        private async void PerformDelete_Command()
        {
            await AssistantDialogOptionService.Remove_Dialogoption(this);

            KronosOverlay_Popup.HomePopup?.Popup_Opened(null, null);
        }

        public event PropertyChangingEventHandler PropertyChanging;

        void OnPropertyChanged(string name) => PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));
    }
}
