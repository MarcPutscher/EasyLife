using EasyLife.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EasyLife.Cells
{
    public class Stackholder_Template_Selecter : DataTemplateSelector
    {
        public DataTemplate Stackholder { get; set; }

        public DataTemplate Total { get; set; }

        public DataTemplate GroupeTotal { get; set; }

        /// <summary>
        /// Funktion die die Listenitems in der Bilanzliste nach einem bestimmten Muster auswählt.
        /// </summary>
        /// <returns>Gibt ein Listobjekt für die Bilanzliste zurück was spezielle eigenschaften besitzt.</returns>
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var stackholderbundle = (StackholderBundle)item;

            if (stackholderbundle.Definition == 0)
            {
                return Stackholder;
            }
            if (stackholderbundle.Definition == 1)
            {
                return Total;
            }
            if (stackholderbundle.Definition == 2)
            {
                return GroupeTotal;
            }
            else
            {
                return Stackholder;
            }
        }
    }
}
