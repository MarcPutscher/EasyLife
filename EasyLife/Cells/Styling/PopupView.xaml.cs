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
	public partial class PopupView : ContentView
	{
		public PopupView ()
		{
			InitializeComponent ();

			Cancel.CommandParameter = new List<object>() { new Dictionary<string,string>(){ { "Vordergrund", "Vordergrund_Cancel_Popup" }, { "Hintergrund",  "Hintergrund_Cancel_Popup" } }, "Zurück" ,"Popup_View" };

			Content.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Hintergrund", "Hintergrund_Content_Popup" }, { "Rand", "Rand_Content_Popup" }, { "Haupttext", "Text_Content_Popup" }, { "Nebentext" , "Subtext_Content_Popup"}, { "Schalter aktiv", "Aktiv_Schalter_Popup" }, { "Schalter deaktiv", "Deaktiv_Schalter_Popup" } }, "Inhalt", "Popup_View" };

			Button.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Hintergrund","Hintertgrund_Button_Popup" }, { "Rand", "Rand_Button_Popup" }, { "Text", "Text_Button_Popup"}, }, "Schaltfläche", "Popup_View" };

			Title.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Text" ,"Text_Titel_Popup"} }, "Überschrift", "Popup_View" };
		}

		public static List<string> Popup_View_Item_List = new List<string>()
		{
			"Vordergrund_Cancel_Popup",
			"Hintergrund_Cancel_Popup",
            "Hintergrund_Content_Popup",
            "Rand_Content_Popup",
            "Text_Content_Popup",
            "Subtext_Content_Popup",
            "Hintertgrund_Button_Popup",
            "Rand_Button_Popup",
            "Text_Button_Popup",
            "Text_Titel_Popup",
            "Aktiv_Schalter_Popup",
            "Deaktiv_Schalter_Popup"
        };
	}
}