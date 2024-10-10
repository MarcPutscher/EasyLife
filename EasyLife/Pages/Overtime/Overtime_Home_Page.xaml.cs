using EasyLife.Models;
using EasyLife.PageModels.To_Do;
using EasyLife.Services;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.BehaviorsPack;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages.Overtime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Overtime_Home_Page : ContentPage
    {
        ObservableCollection<Overtime_DataItem> time_data = new ObservableCollection<Overtime_DataItem>();
        ObservableCollection<Overtime_DataItem> month_data = new ObservableCollection<Overtime_DataItem>();
        ObservableCollection<Overtime_DataItem> year_data = new ObservableCollection<Overtime_DataItem>();
        ObservableCollection<Overtime_DataItem> day_data = new ObservableCollection<Overtime_DataItem>();

        ObservableCollection<OvertimeItem> logs = new ObservableCollection<OvertimeItem>();

        OvertimeItem ChangingItem = null;

        public bool In_Change = false;

        public int Total_Overtime = 0;

        public Overtime_Home_Page()
        {
            InitializeComponent();

            this.BindingContext = this;

            List<int> minuts = Enumerable.Range(0,37).Select(y=> -y*5).Reverse().ToList();
            minuts.AddRange(Enumerable.Range(1, 36).Select(y => y * 5).ToList());
            int position = 0;
            foreach (int i in minuts)
            {
                time_data.Add(new Overtime_DataItem() { Position = position, Data = i });
                position++;
            }

            List<int> month = Enumerable.Range(1, 12).ToList();
            List<string> monthstring = new List<string>() {"", "Jan", "Feb", "Mrz", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dez" };
            position = 0;
            foreach (int i in month)
            {
                month_data.Add(new Overtime_DataItem() { Position = position, Data = i, Data_String = monthstring[i] });
                position++;
            }

            List<int> year = Enumerable.Range(2020, 10).ToList();
            position = 0;
            foreach (int i in year)
            {
                year_data.Add(new Overtime_DataItem() { Position = position, Data = i });
                position++;
            }

            timepicker.ItemsSource = time_data;

            timepicker.CurrentItem = time_data.FirstOrDefault(x=>x.Data == 0);

            yearpicker.ItemsSource = year_data;

            monthpicker.ItemsSource = month_data;

            yearpicker.CurrentItem = year_data.FirstOrDefault(x => x.Data == DateTime.Today.Year);

            monthpicker.CurrentItem = month_data.FirstOrDefault(x => x.Data == DateTime.Today.Month);

            List<int> days = Enumerable.Range(1, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)).ToList();
            position = 0;
            foreach (int i in days)
            {
                day_data.Add(new Overtime_DataItem() { Position = position, Data = i });
                position++;
            }
            daypicker.ItemsSource = day_data;
            daypicker.CurrentItem = day_data.FirstOrDefault(x => x.Data == DateTime.Today.Day);
        }

        public async void Page_Appearing(object sender, EventArgs e)
        {
            logs.Clear();

            logs = await OvertimeService.Get_all_Overtime_for_Logs();

            Total_Overtime = logs.Sum(x =>x.Time);

            totalovertimelabel.Text = Total_Overtime.ToString();

            logpicker.ItemsSource = logs.OrderByDescending(x=>x.Date);

            ChangingItem = null;
            In_Change = false;
        }

        private async void timepicker_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            Overtime_DataItem overtime_DataItem = (Overtime_DataItem)e.CurrentItem;

            if(In_Change == false)
            {
                if (overtime_DataItem.Data == 0)
                {
                    var a1 = futureovertimeview.TranslateTo(0, 40, 300);
                    var a2 = futureovertimeview.FadeTo(0, 300);

                    await Task.WhenAll(a1, a2);

                    futureovertimeview.IsVisible = false;
                }
                else
                {
                    futureovertimeview.IsVisible = true;

                    var a1 = futureovertimeview.TranslateTo(0, -40, 300);
                    var a2 = futureovertimeview.FadeTo(1, 300);

                    await Task.WhenAll(a1, a2);
                }

                futureovertimelabel.Text = overtime_DataItem.Data.ToString();
            }
            else
            {
                changeovertimelabel.Text = overtime_DataItem.Data.ToString();
            }

            totalovertimelabel.Text = (Total_Overtime + overtime_DataItem.Data).ToString();
        }

        private async void monthpicker_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            Overtime_DataItem overtime_DataItem = (Overtime_DataItem)e.CurrentItem;

            if(overtime_DataItem.Data != 0)
            {
                if (yearpicker.CurrentItem == null)
                    return;
                Overtime_DataItem year = (Overtime_DataItem)yearpicker.CurrentItem;

                if (daypicker.CurrentItem == null)
                    return;
                Overtime_DataItem day = (Overtime_DataItem)daypicker.CurrentItem;

                List<int> days = Enumerable.Range(1, DateTime.DaysInMonth(year.Data, overtime_DataItem.Data)).ToList();
                int position = 0;
                day_data.Clear();
                foreach (int i in days)
                {
                    day_data.Add(new Overtime_DataItem() { Position = position, Data = i });
                    position++;
                }
                daypicker.ItemsSource = day_data;


                if(day_data.Where(x=>x.Data == day.Data).Count() == 0)
                {
                    day.Data = 1;
                }

                Overtime_DataItem new_day = day_data.First(x => x.Data == day.Data);

                daypicker.CurrentItem = new_day;

                await Task.Delay(500).ContinueWith((_) => Device.BeginInvokeOnMainThread(() =>
                    daypicker.ScrollTo(new_day)),
                    TaskScheduler.FromCurrentSynchronizationContext());
            
            }
        }


        private async void Add_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (yearpicker.CurrentItem == null)
                    return;
                Overtime_DataItem year = (Overtime_DataItem)yearpicker.CurrentItem;

                if (monthpicker.CurrentItem == null)
                    return;
                Overtime_DataItem month = (Overtime_DataItem)monthpicker.CurrentItem;

                if (daypicker.CurrentItem == null)
                    return;
                Overtime_DataItem day = (Overtime_DataItem)daypicker.CurrentItem;

                if (timepicker.CurrentItem == null)
                    return;
                Overtime_DataItem time = (Overtime_DataItem)timepicker.CurrentItem;

                OvertimeItem overtime = new OvertimeItem() { Date = new DateTime(year.Data, month.Data, day.Data), Time = time.Data };

                var result = await OvertimeService.Add_Overtime(overtime);

                if (result == 1)
                    return;

                logs.Add(overtime);

                Total_Overtime = logs.Sum(x => x.Time);

                totalovertimelabel.Text = Total_Overtime.ToString();

                logpicker.ItemsSource = logs.OrderByDescending(x => x.Date);
            }
            finally
            {
                var a1 = futureovertimeview.TranslateTo(0, 40, 300);
                var a2 = futureovertimeview.FadeTo(0, 300);

                await Task.WhenAll(a1, a2);

                futureovertimeview.IsVisible = false;

                timepicker.CurrentItem = time_data.FirstOrDefault(x => x.Data == 0);
            }
        }

        private async void Edit_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (yearpicker.CurrentItem == null)
                    return;
                Overtime_DataItem year = (Overtime_DataItem)yearpicker.CurrentItem;

                if (monthpicker.CurrentItem == null)
                    return;
                Overtime_DataItem month = (Overtime_DataItem)monthpicker.CurrentItem;

                if (daypicker.CurrentItem == null)
                    return;
                Overtime_DataItem day = (Overtime_DataItem)daypicker.CurrentItem;

                if (timepicker.CurrentItem == null)
                    return;
                Overtime_DataItem time = (Overtime_DataItem)timepicker.CurrentItem;

                if (ChangingItem == null)
                    return;

                ChangingItem.Date = new DateTime(year.Data, month.Data, day.Data);
                ChangingItem.Time = time.Data;

                await OvertimeService.Edit_Overtime(ChangingItem);

                Total_Overtime = logs.Sum(x => x.Time);

                totalovertimelabel.Text = Total_Overtime.ToString();

                logpicker.ItemsSource = logs.OrderByDescending(x => x.Date);

                In_Change = false;

                ChangingItem = null;
            }
            finally
            {
                var a1 = changeovertimeview.TranslateTo(0, 40, 300);
                var a2 = changeovertimeview.FadeTo(0, 300);

                await Task.WhenAll(a1, a2);

                changeovertimeview.IsVisible = false;

                timepicker.CurrentItem = time_data.FirstOrDefault(x => x.Data == 0);
            }
        }


        private async void Remove_Clicked(object sender, EventArgs e)
        {
            SwipeItemView swipeItemView = (SwipeItemView)sender;
            OvertimeItem overtimeItem = swipeItemView.CommandParameter as OvertimeItem;

            overtimeItem.Is_Removed = true;

            await OvertimeService.Remove_Overtime(overtimeItem);

            logs.Remove(overtimeItem);

            Total_Overtime = logs.Sum(x => x.Time);

            totalovertimelabel.Text = Total_Overtime.ToString();

            logpicker.ItemsSource = logs.OrderByDescending(x => x.Date);
        }

        private async void Change_Clicked(object sender, EventArgs e)
        {
            SwipeItemView swipeItemView = (SwipeItemView)sender;
            OvertimeItem overtimeItem = swipeItemView.CommandParameter as OvertimeItem;

            ChangingItem = overtimeItem;

            In_Change = true;

            if (overtimeItem.Time == 0)
            {
                var a1 = changeovertimeview.TranslateTo(0, 40, 300);
                var a2 = changeovertimeview.FadeTo(0, 300);

                await Task.WhenAll(a1, a2);

                changeovertimeview.IsVisible = false;
            }
            else
            {
                changeovertimeview.IsVisible = true;

                var a1 = changeovertimeview.TranslateTo(0, -40, 300);
                var a2 = changeovertimeview.FadeTo(1, 300);

                await Task.WhenAll(a1, a2);
            }


            if (yearpicker.CurrentItem == null)
                return;
            yearpicker.CurrentItem = year_data.FirstOrDefault(x=>x.Data == ChangingItem.Date.Year);

            if (monthpicker.CurrentItem == null)
                return;
            monthpicker.CurrentItem = month_data.FirstOrDefault(x => x.Data == ChangingItem.Date.Month);

            if (daypicker.CurrentItem == null)
                return;
            daypicker.CurrentItem = day_data.FirstOrDefault(x => x.Data == ChangingItem.Date.Day);

            if (timepicker.CurrentItem == null)
                return;
            Overtime_DataItem time = timepicker.CurrentItem as Overtime_DataItem;
            if (time.Data != 0)
            {
                var a1 = futureovertimeview.TranslateTo(0, 40, 300);
                var a2 = futureovertimeview.FadeTo(0, 300);

                await Task.WhenAll(a1, a2);

                futureovertimeview.IsVisible = false;
            }

            timepicker.CurrentItem = time_data.FirstOrDefault(x => x.Data == ChangingItem.Time);

            changeovertimelabel.Text = ChangingItem.Time.ToString();
        }
    }
}