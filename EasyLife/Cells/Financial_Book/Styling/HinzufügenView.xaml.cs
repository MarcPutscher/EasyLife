using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Cells.Styling
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HinzufügenView : ContentView
    {
        public HinzufügenView()
        {
            InitializeComponent();

            Seite.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Hintergrund", "Hintergrund_Hinzufügen" } }, "Seite", "Hinzufügen_View" };
            Titel.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Titel", "Title_Eingabefeld_Hinzufügen" } }, "Titel", "Hinzufügen_View" };
            Eingabefeld.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Hintergrund", "Hintergrund_Eingabefeld_Hinzufügen" }, { "Rand", "Rand_Eingabefeld_Hinzufügen" }, { "Text", "Text_Eingabefeld_Hinzufügen" }, { "Platzhalter", "Platzhater_Eingabefeld_Hinzufügen" } }, "Eingabefeld", "Hinzufügen_View" };
            EingabefeldmitSchalter.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Hintergrund", "Hintergrund_Eingabefeld_Hinzufügen" }, { "Rand", "Rand_Eingabefeld_Hinzufügen" }, { "Text", "Text_Eingabefeld_Hinzufügen" }, { "Schalter aktiv", "Aktiv_Schalter_Eingabefeld_Hinzufügen" }, { "Schalter deaktiv", "Deaktiv_Schalter_Eingabefeld_Hinzufügen" } }, "Eingabefeld", "Hinzufügen_View" };
            Auswahlfeld.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Hintergrund", "Hintergrund_Wahlfeld_Hinzufügen" }, { "Haupttext", "Haubttext_Wahlfeld_Hinzufügen" }, { "Nebentext", "Nebentext_Wahlfeld_Hinzufügen" }, { "Hintergrund der Option", "Hintergrund_Option_Wahlfeld_Hinzufügen" }, { "Rand der Option", "Rand_Option_Wahlfeld_Hinzufügen" }, { "Text der Option", "Text_Option_Wahlfeld_Hinzufügen" } }, "Auswahlfeld", "Hinzufügen_View" };
            Anwenden.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Hintergrund", "Hintergrund_Schalter_Hinzufügen" }, { "Rand", "Rand_Schalter_Hinzufügen" }, { "Text", "Text_Schalter_Hinzufügen" } }, "Anwenden", "Hinzufügen_View" };
        }

        public static List<string> Hinzufügen_View_Item_List = new List<string>()
        {
            "Hintergrund_Hinzufügen",
            "Hintergrund_Eingabefeld_Hinzufügen",
            "Rand_Eingabefeld_Hinzufügen",
            "Title_Eingabefeld_Hinzufügen",
            "Text_Eingabefeld_Hinzufügen",
            "Platzhater_Eingabefeld_Hinzufügen",
            "Aktiv_Schalter_Eingabefeld_Hinzufügen",
            "Deaktiv_Schalter_Eingabefeld_Hinzufügen",
            "Hintergrund_Wahlfeld_Hinzufügen",
            "Haubttext_Wahlfeld_Hinzufügen",
            "Nebentext_Wahlfeld_Hinzufügen",
            "Hintergrund_Option_Wahlfeld_Hinzufügen",
            "Rand_Option_Wahlfeld_Hinzufügen",
            "Text_Option_Wahlfeld_Hinzufügen",
            "Hintergrund_Schalter_Hinzufügen",
            "Rand_Schalter_Hinzufügen",
            "Text_Schalter_Hinzufügen"
        };
    }
}