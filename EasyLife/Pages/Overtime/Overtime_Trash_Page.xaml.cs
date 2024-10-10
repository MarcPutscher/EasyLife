using EasyLife.Models;
using EasyLife.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages.Overtime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Overtime_Trash_Page : ContentPage
    {
        ObservableCollection<OvertimeItem> logs = new ObservableCollection<OvertimeItem>();
        public Overtime_Trash_Page()
        {
            InitializeComponent();
        }

        public async void Page_Appearing(object sender, EventArgs e)
        {
            logs.Clear();

            logs = await OvertimeService.Get_all_Overtime_for_Trash_Logs();

            logpicker.ItemsSource = logs.OrderByDescending(x => x.Date);
        }
        private async void Remove_Clicked(object sender, EventArgs e)
        {
            SwipeItemView swipeItemView = (SwipeItemView)sender;
            OvertimeItem overtimeItem = swipeItemView.CommandParameter as OvertimeItem;

            await OvertimeService.Remove_Overtime(overtimeItem);

            logs.Remove(overtimeItem);

            logpicker.ItemsSource = logs.OrderBy(x => x.Date);
        }

        private async void Revive_Clicked(object sender, EventArgs e)
        {
            SwipeItemView swipeItemView = (SwipeItemView)sender;
            OvertimeItem overtimeItem = swipeItemView.CommandParameter as OvertimeItem;

            overtimeItem.Is_Removed = false;

            await OvertimeService.Edit_Overtime(overtimeItem);

            logs.Remove(overtimeItem);

            logpicker.ItemsSource = logs.OrderBy(x => x.Date);
        }
    }
}