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
	public partial class BilanzView : ContentView
	{
		public BilanzView()
		{
			InitializeComponent ();

            Seite.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Hintergrund", "Hintergrund_Bilanz" } }, "Seite", "Bilanz_View" };
            Kopfzeile.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Hintergrund", "Hintergrund_Kopfzeile_Bilanz" }, { "Text", "Text_Kopfzeile_Bilanz" } }, "Kopfzeile", "Bilanz_View" };
            Fußzeile.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Hintergrund", "Hintergrund_Fußzeile_Bilanz" }, { "Text", "Text_Fußzeile_Bilanz" } }, "Fußzeile", "Bilanz_View" };
            Stacks.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Zwecke", "Text_Zweck_Stack_Bilanz" }, { "Anzahl", "Text_Anzahl_Stack_Bilanz" } }, "Interessengruppen", "Bilanz_View" };
            Zusammenfassung.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Text", "Text_Zusammenfassung_Bilanz" } }, "Zusammenfassung", "Bilanz_View" };

        }

        public static List<string> Bilanz_View_Item_List = new List<string>()
        {
            "Hintergrund_Bilanz",
            "Hintergrund_Kopfzeile_Bilanz",
            "Text_Kopfzeile_Bilanz",
            "Hintergrund_Fußzeile_Bilanz",
            "Text_Fußzeile_Bilanz",
            "Text_Zusammenfassung_Bilanz",
            "Text_Zweck_Stack_Bilanz",
            "Text_Anzahl_Stack_Bilanz"
        };
    }
}