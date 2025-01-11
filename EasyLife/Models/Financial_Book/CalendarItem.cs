using EasyLife.Pages;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using Command = MvvmHelpers.Commands.Command;

namespace EasyLife.Models
{
    public class CalendarItem
    {
        public New_Viewtime_Popup new_viewtime_popup;

        public string Year { get; set; }

        public List<Months> Months { get; set; }

        public bool Months_Collection_is_Visibile = false;

        public Command yearbutton_command;

        public Color deafaultcolor = Color.FromHex(Preferences.Get("Hintergrund_Content_Popup", "#0b1c48"));
        public Color Backgroundcolor
        {
            get { return deafaultcolor; }
            set
            {
                if (Backgroundcolor == value)
                {
                    return;
                }

                deafaultcolor = value;
            }
        }

        public ICommand YearButton_Command
        {
            get
            {
                if (yearbutton_command == null)
                {
                    yearbutton_command = new Command(PerformYearButton_Command);
                }

                return yearbutton_command;
            }
        }

        public void PerformYearButton_Command()
        {

            if (Months_Collection_is_Visibile == false)
            {
                Backgroundcolor = ChangeBrightness(Backgroundcolor, +0.2);
            }
            else
            {
                Backgroundcolor = ChangeBrightness(Backgroundcolor, -0.2);
            }

            new_viewtime_popup.YearCollection_Methode(this);

            Months_Collection_is_Visibile = !Months_Collection_is_Visibile;
        }

        public Color ChangeBrightness(Color color , double factor)
        {
            double R = (color.R + factor > 1) ? 1 : color.R + factor;
            double G = (color.G + factor > 1) ? 1 : color.G + factor;
            double B = (color.B + factor > 1) ? 1 : color.B + factor;

            return new Color(R, G, B);
        }
    }

    public class Months
    {
        public New_Viewtime_Popup new_viewtime_popup { get; set; }
        public string Month { get; set; }

        public Command monthbutton_command;

        public bool Months_Selected = false;

        public Color deafaultcolor = Color.FromHex(Preferences.Get("Hintergrund_Content_Popup", "#0b1c48"));
        public Color Backgroundcolor
        {
            get { return deafaultcolor; }
            set
            {
                if (Backgroundcolor == value)
                {
                    return;
                }

                deafaultcolor = value;
            }
        }

        public ICommand MonthButton_Command
        {
            get
            {
                if (monthbutton_command == null)
                {
                    monthbutton_command = new Command(PerformMonthButton_Command);
                }

                return monthbutton_command;
            }
        }

        public void PerformMonthButton_Command()
        {
            Months_Selected = !Months_Selected;

            if (Months_Selected == true)
            {
                Backgroundcolor = ChangeBrightness(Backgroundcolor, +0.2);
            }
            else
            {
                Backgroundcolor = ChangeBrightness(Backgroundcolor, -0.2);
            }

            new_viewtime_popup.MonthCollection_Methode(this);
        }

        public Color ChangeBrightness(Color color, double factor)
        {
            double R = (color.R + factor > 1) ? 1 : color.R + factor;
            double G = (color.G + factor > 1) ? 1 : color.G + factor;
            double B = (color.B + factor > 1) ? 1 : color.B + factor;

            return new Color(R, G, B);
        }
    }
}
