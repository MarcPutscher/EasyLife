using EasyLife.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EasyLife.Cells
{
    public class Transaktions_Template_Selector : DataTemplateSelector
    {
        public DataTemplate Einnahmen { get; set; }

        public DataTemplate Ausgaben { get; set; }

        public DataTemplate Detail_Einnahmen { get; set; }

        public DataTemplate Detail_Ausgaben { get; set; }

        public DataTemplate Laden { get; set; }

        /// <summary>
        /// Funktion die die Transaktionsitems in der Haushaltsbuchliste nach einem bestimmten Muster auswählt.
        /// </summary>
        /// <returns>Gibt ein Listobjekt für die Haushaltsbuchliste zurück was spezielle eigenschaften besitzt.</returns>
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var transaktion = (Transaktion)item;

            if (Preferences.Get("More_Detail", false) == false)
            {
                if (double.Parse(transaktion.Betrag) > 0)
                {
                    return Einnahmen;
                }
                if (double.Parse(transaktion.Betrag) < 0)
                {
                    return Ausgaben;
                }
                else
                {
                    if (transaktion.Auftrags_id == "Load")
                    {
                        return Laden;
                    }
                    return Einnahmen;
                }
            }
            else
            {
                if (double.Parse(transaktion.Betrag) > 0)
                {
                    return Detail_Einnahmen;
                }
                if (double.Parse(transaktion.Betrag) < 0)
                {
                    return Detail_Ausgaben;
                }
                else
                {
                    if (transaktion.Auftrags_id == "Load")
                    {
                        return Laden;
                    }
                    return Detail_Einnahmen;
                }
            }
        }
    }
}
