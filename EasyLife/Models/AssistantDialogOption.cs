using EasyLife.Helpers;
using MvvmHelpers.Commands;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace EasyLife.Models
{
    public class AssistantDialogOption
    {
        public AssistantDialogOption(string Question, string Answer)
        {
            this.Question = Question;
            this.Answer = Answer;
        }
        public AssistantDialogOption()
        {
        }

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

        public Assistant_Kronos Kronos = App.Kronos;

        public string question { get; set; }
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

        public string answer { get; set; }
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

        private async void PerformSpeech_Solution()
        {
            await Kronos.Speech(Answer);
        }

        public event PropertyChangingEventHandler PropertyChanging;

        void OnPropertyChanged(string name) => PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));

    }
}
