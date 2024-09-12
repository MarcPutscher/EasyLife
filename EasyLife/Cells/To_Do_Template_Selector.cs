using EasyLife.Models;
using EasyLife.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EasyLife.Cells
{
    class To_Do_Template_Selector : DataTemplateSelector
    {
        public DataTemplate To_Do_with_Time { get; set; }
        public DataTemplate To_Do_without_Time { get; set; }


        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {

            if (item is To_Do_Item)
            {
                To_Do_Item to_Do_Item = (To_Do_Item) item;

                if(to_Do_Item.Datumanzeige != null)
                {
                    return To_Do_with_Time;
                }
            }
            return To_Do_without_Time;
        }
    }
}
