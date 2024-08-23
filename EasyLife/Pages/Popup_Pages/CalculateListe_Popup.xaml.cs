using EasyLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalculateListe_Popup : Popup
    {
        public List<Transaktion> transaktionList = new List<Transaktion>();
        public CalculateListe_Popup(List<Transaktion> liste)
        {
            transaktionList.AddRange(liste);

            InitializeComponent();

            SumList.ItemsSource = transaktionList;

            if(transaktionList.Count() == 0)
            {
                SumList.IsVisible = false;

                Empty.IsVisible = true;
            }
            else
            {
                SumList.IsVisible = true;

                Empty.IsVisible = false;
            }
        }
        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                Dismiss(transaktionList);
            }
            catch
            {
                Dismiss(null);
            }
        }

        private void SwipeItem_Invoked(object sender, EventArgs e)
        {
            var input = sender as SwipeItem;

            Transaktion trans = input.CommandParameter as Transaktion;

            if (trans != null)
            {
                transaktionList.Remove(trans);

                SumList.ItemsSource = null;

                SumList.ItemsSource = transaktionList;
            }
        }
    }
}