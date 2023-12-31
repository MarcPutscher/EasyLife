﻿using EasyLife.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EasyLife.Cells
{
    public class Transaktions_Template_Selector : DataTemplateSelector
    {
        public DataTemplate Einnahmen { get; set; }

        public DataTemplate Ausgaben { get; set; }

        /// <summary>
        /// Funktion die die Transaktionsitems in der Haushaltsbuchliste nach einem bestimmten Muster auswählt.
        /// </summary>
        /// <returns>Gibt ein Listobjekt für die Haushaltsbuchliste zurück was spezielle eigenschaften besitzt.</returns>
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var transaktion = (Transaktion)item;

            if(double.Parse(transaktion.Betrag) > 0)
            {
                return Einnahmen;
            }
            if(double.Parse(transaktion.Betrag) < 0)
            {
                return Ausgaben;
            }
            else
            {
                return Einnahmen;
            }
        }
    }
}
