using EasyLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Cells
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Stackholder_Cell : Frame
    {
        public Stackholder_Cell()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Funktion für das öffnen der gelisteten Transaktionen, die einem speziellen Zweck zugeordnet werden, in der Bilanzliste.
        /// </summary>
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                //  Desirealisiert den Parameter der gedrückt wurde, in eine Liste von Transaktionen
                
                List<Transaktion> input = ((TappedEventArgs)e).Parameter as List<Transaktion>;

                string[] details = new string[] { };

                List<string> detaillist = new List<string>();

                //  Wenn der Input nicht leer ist wird er angezeigt.

                if (input.Count() != 0)
                {
                    //  Erstellt eine Liste an Strings die das Datum und den Betrag der Transaktion mit dem speziellen Zweck anzeigt. 

                    foreach (Transaktion tran in input)
                    {
                        detaillist.Add("" + tran.Datumanzeige + " : " + tran.Betrag + " €");
                    }

                    details = detaillist.ToArray();

                    bool indikator = false;

                    //  Erstellt ein Auswählfenster, wo jeder String in details als Button angezeigt wird.

                    while (indikator == false)
                    {
                        var result = await Shell.Current.DisplayActionSheet("Details zu " + input[0].Zweck + "", "Zurück", null, details);

                        //  Wenn ein Button getrückt wurde, wird die Transaktion nach einem bestimmten Muster, als eingeständiges Fenster, angezeigt, falls dieser Button einem String in der detailliste entspricht.
                        //  Sonst wird das Auswahlfenster geschlossen.
                        
                        if (detaillist.Contains(result) == true)
                        {
                            Transaktion item = input[detaillist.IndexOf(result)];

                            string message = null;

                            if (String.IsNullOrEmpty(item.Auftrags_id) == false)
                            {
                                if (item.Auftrags_Option == 1)
                                {
                                    message = "Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: "+item.Balance_Visibility_String+"\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "";
                                }
                                if (item.Auftrags_Option == 2)
                                {
                                    message = "Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl an Wiederholungen: " + item.Anzahl_an_Wiederholungen + " Mal\nSpeziell: " + item.Speziell + "";
                                }
                                if (item.Auftrags_Option == 3)
                                {
                                    message = "Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nEnddatum: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "";
                                }

                                await Shell.Current.DisplayAlert("Transaktion " + item.Id + "", message, "Zurück");
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Transaktion " + item.Id + "", "Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"", "Zurück");
                            }
                        }
                        else
                        {
                            indikator = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }
    }
}