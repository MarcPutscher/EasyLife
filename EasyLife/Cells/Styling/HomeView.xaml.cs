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
	public partial class HomeView : ContentView
	{
		public HomeView ()
		{
			InitializeComponent ();

            Seite.CommandParameter = new List<object>() { new Dictionary<string, string>() { {"Hintergrund", "Hintergrund_Home" } }, "Seite", "Home_View" };

            Transaktion_Positiv.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Hintergrund", "Hintergrund_Transaktion_Home" }, { "Rand", "Rand_Transaktion_Home" }, { "Text", "Text_Transaktion_Home" }, { "Hintergrund der Details", "Hintergrund_Detail_Transaktion_Home" }, { "Text der Details", "Text_Detail_Transaktion_Home" }, { "Hintergrund des Betrag","Hintergrund_Positiver_Betrag_Transaktion_Home" }, { "Rand des Betrages" ,"Rand_Positiver_Betrag_Transaktion_Home"}, { "Text des Betrages", "Text_Positiver_Betrag_Transaktion_Home" } }, "Einkommen", "Home_View" };

            Transaktion_Negativ.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Hintergrund", "Hintergrund_Transaktion_Home" }, { "Rand", "Rand_Transaktion_Home" }, { "Text", "Text_Transaktion_Home" }, { "Hintergrund der Details", "Hintergrund_Detail_Transaktion_Home" }, { "Text der Details", "Text_Detail_Transaktion_Home" }, { "Hintergrund des Betrag", "Hintergrund_Negativer_Betrag_Transaktion_Home" }, { "Rand des Betrages", "Rand_Negativer_Betrag_Transaktion_Home" }, { "Text des Betrages", "Text_Negativer_Betrag_Transaktion_Home" } }, "Ausgaben", "Home_View" };

            Bearbeiten.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Hintergrund", "Hintergrund_Bearbeiten_Home" }, { "Rand", "Rand_Bearbeiten_Home" }, { "Text", "Text_Bearbeiten_Home" } }, "Bearbeiten", "Home_View" };

            Löschen.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Hintergrund", "Hintergrund_Löschen_Home" }, { "Rand", "Rand_Löschen_Home" }, { "Text", "Text_Löschen_Home" } }, "Löschen", "Home_View" };

            Budget.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Vordergrund", "Vordergrund_Budget_Home" } }, "Budget", "Home_View" };

            MehrLaden.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Text", "Text_MehrLaden_Home" } }, "Laden", "Home_View" };

            Saldo.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Hintergrund", "Hintergrund_Saldo_Home" }, { "Text", "Text_Saldo_Home" } }, "Stand", "Home_View" };

            Hinzufügen.CommandParameter = new List<object>() { new Dictionary<string, string>() { { "Vordergrund", "Vordergrund_Hinzufügen_Home" } }, "Hinzufügen", "Home_View" };

        }

        public static List<string> Home_View_Item_List = new List<string>()
        {
            "Hintergrund_Home",
            "Hintergrund_Bearbeiten_Home",
            "Rand_Bearbeiten_Home",
            "Text_Bearbeiten_Home",
            "Hintergrund_Löschen_Home",
            "Rand_Löschen_Home",
            "Text_Löschen_Home",
            "Hintergrund_Transaktion_Home",
            "Rand_Transaktion_Home",
            "Text_Transaktion_Home",
            "Hintergrund_Detail_Transaktion_Home",
            "Text_Detail_Transaktion_Home",
            "Hintergrund_Positiver_Betrag_Transaktion_Home",
            "Rand_Positiver_Betrag_Transaktion_Home",
            "Text_Positiver_Betrag_Transaktion_Home",
            "Hintergrund_Negativer_Betrag_Transaktion_Home",
            "Rand_Negativer_Betrag_Transaktion_Home",
            "Text_Negativer_Betrag_Transaktion_Home",
            "Text_MehrLaden_Home",
            "Vordergrund_Budget_Home",
            "Hintergrund_Saldo_Home",
            "Text_Saldo_Home",
            "Vordergrund_Hinzufügen_Home"
        };
    }
}