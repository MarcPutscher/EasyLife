using System;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MetaData_Popup : Popup
    {
        public MetaData_Popup(List<int> amount_list)
        {
            InitializeComponent();

            Amount_of_generated_Transaktion.Text = amount_list[0].ToString();
            Amount_of_deleted_Transaktion.Text = amount_list[1].ToString();
            Amount_of_use_Transaktion.Text = amount_list[2].ToString();
            Amount_of_unuse_Transaktion.Text = amount_list[3].ToString();
            Amount_of_Transaktion_in_Orders.Text = amount_list[4].ToString();

            Amount_of_generated_Order.Text = amount_list[5].ToString();
            Amount_of_deleted_Order.Text = amount_list[6].ToString();
            Amount_of_use_Order.Text = amount_list[7].ToString();
            Amount_of_unuse_Order.Text = amount_list[8].ToString();

            Amount_of_generated_Reason.Text = amount_list[9].ToString();
            Amount_of_use_Reason.Text = amount_list[10].ToString();
            Amount_of_unuse_Reason.Text = amount_list[11].ToString();

            Amount_of_generated_Notification.Text = amount_list[12].ToString();
            Amount_of_deleted_Notification.Text = amount_list[13].ToString();
            Amount_of_use_Notification.Text = amount_list[14].ToString();
            Amount_of_unuse_Notification.Text = amount_list[15].ToString();

            Amount_of_generated_Suggestion.Text = amount_list[16].ToString();
            Amount_of_deleted_Suggestion.Text = amount_list[17].ToString();
            Amount_of_use_Suggestion.Text = amount_list[18].ToString();

            Amount_of_generated_Balanceprofile.Text = amount_list[19].ToString();
            Amount_of_deleted_Balanceprofile.Text = amount_list[20].ToString();
            Amount_of_use_Balanceprofile.Text = amount_list[21].ToString();

            Amount_of_generated_Budgets.Text = amount_list[22].ToString();
            Amount_of_deleted_Budgets.Text = amount_list[23].ToString();
            Amount_of_use_Budgets.Text = amount_list[24].ToString();

            Amount_of_generated_Stylingprofiles.Text = amount_list[25].ToString();
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }
    }
}