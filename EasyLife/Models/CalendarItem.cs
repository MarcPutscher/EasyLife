using EasyLife.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyLife.Models
{
    public class CalendarItem
    {
        public New_Viewtime_Popup new_viewtime_popup {  get; set; }

        public int Year { get; set; }

        public List<string> Months { get; set; }

        public bool Months_Collection_is_Visibile = false;

        public void YearButton_Clicked(object sender, EventArgs e)
        {
            Months_Collection_is_Visibile = !Months_Collection_is_Visibile;

            new_viewtime_popup.MonthCollection_Methode(Months,Months_Collection_is_Visibile);
        }
    }
}
