using EasyLife.Cells.Styling;
using EasyLife.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EasyLife.Cells
{
    public class Template_Selector_for_Styling : DataTemplateSelector
    {
        public Dictionary<string, DataTemplate> datatemplatedic = new Dictionary<string, DataTemplate>();
        public DataTemplate Popup { get; set; }
        public DataTemplate Home { get; set; }
        public DataTemplate Hinzufügen { get; set; }
        public DataTemplate Bilanz { get; set; }
        public DataTemplate Error { get; set; }


        public Template_Selector_for_Styling()
        {
            Popup = new DataTemplate(typeof(PopupView));

            Home = new DataTemplate(typeof(HomeView));

            Hinzufügen = new DataTemplate(typeof(HinzufügenView));

            Bilanz = new DataTemplate(typeof(BilanzView));

            Error = new DataTemplate(typeof(ErrorView));

            datatemplatedic.Add("Popup_View", Popup);
            datatemplatedic.Add("Home_View", Home);
            datatemplatedic.Add("Hinzufügen_View", Hinzufügen);
            datatemplatedic.Add("Bilanz_View", Bilanz);
        }

        /// <summary>
        /// Funktion die die Transaktionsitems in der Haushaltsbuchliste nach einem bestimmten Muster auswählt.
        /// </summary>
        /// <returns>Gibt ein Listobjekt für die Haushaltsbuchliste zurück was spezielle eigenschaften besitzt.</returns>
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            StylingCell cell = (StylingCell)item;

            if (datatemplatedic[cell.Cellname] != null)
            {
                return datatemplatedic[cell.Cellname];
            }
            else
            {
                return Error;
            }
        }
    }

    public class StylingCell
    {
        public string Cellname { get; set; }

        public List<string> Items { get; set; }

        public string Title { get; set; }
    }
}