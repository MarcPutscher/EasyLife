using EasyLife.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EasyLife.Cells
{
    public class Transaktions_Detail_Selector : DataTemplateSelector
    {
        public DataTemplate Order_Option_1 { get; set; }

        public DataTemplate Order_Option_2 { get; set; }

        public DataTemplate Order_Option_3 { get; set; }

        /// <summary>
        /// Funktion die die Details einer Transaktion in der Haushaltsbuchliste nach einem bestimmten Muster auswählt.
        /// </summary>
        /// <returns>Gibt ein Detailobjekt für die Transaktion zurück was spezielle eigenschaften besitzt.</returns>
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var transaktion = (Transaktion)item;

            if (transaktion.Auftrags_Option == 1)
            {
                return Order_Option_1;
            }
            if (transaktion.Auftrags_Option == 2)
            {
                return Order_Option_2;
            }
            if (transaktion.Auftrags_Option == 3)
            {
                return Order_Option_3;
            }
            else
            {
                return Order_Option_1;
            }
        }
    }
}
