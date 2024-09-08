using EasyLife.Models;
using EasyLife.Pages;
using EasyLife.Pages.To_Do;
using EasyLife.Services;
using FreshMvvm;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EasyLife.PageModels
{
    public  class To_Do_Deleted_PageModel : FreshBasePageModel
    {
        public To_Do_Deleted_PageModel()
        {
        }
        public To_Do_Deleted_PageModel( string title, To_Do_Deleted_Page to_Do_Deleted_Page)
        {
            this.ToDoDeletedPage = to_Do_Deleted_Page;


            this.Title = title;
            origin_title = title;


            To_Do_Items = new ObservableRangeCollection<Grouping<string, To_Do_Item>>();


            Refresh_Command = new AsyncCommand(Refresh_Methode);
            Delet_Command = new AsyncCommand<To_Do_Item>(Delet_Methode);
            Delet_Range_Command = new AsyncCommand(Delet_Range_Methode);
            Select_All_Command = new AsyncCommand(Select_All_Methode);
            Choose1_Command = new AsyncCommand<To_Do_Item>(Choose1_Methode);
            Choose2_Command = new AsyncCommand(Choose2_Methode);
            Item_Selected_Command = new AsyncCommand<To_Do_Item>(Item_Selected_Methode);
            Cancel_Command = new AsyncCommand(Cancel_Methode);
            Revive_Command = new AsyncCommand(Revive_Mehtode);


            ViewIsDisappearing_Command = new MvvmHelpers.Commands.Command(ViewIsDisappearing_Methode);
        }

        public async Task Refresh_Methode()
        {
            ObservableRangeCollection<Grouping<string, To_Do_Item>> to_Do_Items = new ObservableRangeCollection<Grouping<string, To_Do_Item>>();
            List<string> dates = new List<string>();

            try
            {
                List<To_Do_Item> result = await To_DoService.Get_all_To_DOs();

                List<To_Do_Item> to_dos = new List<To_Do_Item>();

                to_dos = new List<To_Do_Item>(result.Where(x => x.Is_Removed == true).ToList());


                if (to_dos.Count != 0)
                {
                    Show_Empty_List = false;

                    foreach (To_Do_Item to_do in to_dos)
                    {
                        if (dates.Contains(to_do.Datumanzeige) == false)
                        {
                            dates.Add(to_do.Datumanzeige);
                        }
                    }

                    if (dates.Count != 0)
                    {
                        List<To_Do_Item> to_Do_Items1 = to_dos.ToList();
                        List<To_Do_Item> to_Do_Items2 = new List<To_Do_Item>(to_Do_Items1);

                        if (to_Do_Items1.Count() != 0)
                        {
                            if (to_Do_Items1.Where(x => x.Datumanzeige == DateTime.Today.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"))).Count() != 0)
                            {
                                to_Do_Items.Add(new Grouping<string, To_Do_Item>("Heute", to_Do_Items1.Where(x => x.Datumanzeige == DateTime.Today.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE")))));

                                foreach (To_Do_Item to_Do_Item in to_Do_Items1.Where(x => x.Datumanzeige == DateTime.Today.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"))))
                                {
                                    to_Do_Items2.Remove(to_Do_Item);
                                }
                                to_Do_Items1 = new List<To_Do_Item>(to_Do_Items2);
                            }
                            if (to_Do_Items1.Where(x => x.Datumanzeige == DateTime.Today.AddDays(1).AddSeconds(-1).ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"))).Count() != 0)
                            {
                                to_Do_Items.Add(new Grouping<string, To_Do_Item>("Morgen", to_Do_Items1.Where(x => x.Datumanzeige == DateTime.Today.AddDays(1).AddSeconds(-1).ToString("dddd, d.M.yyyy", new CultureInfo("de-DE")))));
                                foreach (To_Do_Item to_Do_Item in to_Do_Items1.Where(x => x.Datumanzeige == DateTime.Today.AddDays(1).AddSeconds(-1).ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"))))
                                {
                                    to_Do_Items2.Remove(to_Do_Item);
                                }
                                to_Do_Items1 = new List<To_Do_Item>(to_Do_Items2);
                            }
                            if (to_Do_Items1.Where(x => x.Datumanzeige != DateTime.Today.AddDays(1).AddSeconds(-1).ToString("dddd, d.M.yyyy", new CultureInfo("de-DE")) && x.Datumanzeige != DateTime.Today.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"))).Count() != 0)
                            {
                                to_Do_Items.Add(new Grouping<string, To_Do_Item>("Später", to_Do_Items1.Where(x => x.Datumanzeige != DateTime.Today.AddDays(1).AddSeconds(-1).ToString("dddd, d.M.yyyy", new CultureInfo("de-DE")) && x.Datumanzeige != DateTime.Today.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE")))));
                                foreach (To_Do_Item to_Do_Item in to_Do_Items1.Where(x => x.Datumanzeige != DateTime.Today.AddDays(1).AddSeconds(-1).ToString("dddd, d.M.yyyy", new CultureInfo("de-DE")) && x.Datumanzeige != DateTime.Today.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"))))
                                {
                                    to_Do_Items2.Remove(to_Do_Item);
                                }
                                to_Do_Items1 = new List<To_Do_Item>(to_Do_Items2);
                            }
                            if (to_Do_Items1.Where(x => x.Reminder < DateTime.Today).Count() != 0)
                            {
                                to_Do_Items.Add(new Grouping<string, To_Do_Item>("Einst", to_Do_Items1.Where(x => x.Reminder < DateTime.Today)));
                                foreach (To_Do_Item to_Do_Item in to_Do_Items1.Where(x => x.Reminder < DateTime.Today))
                                {
                                    to_Do_Items2.Remove(to_Do_Item);
                                }
                                to_Do_Items1 = new List<To_Do_Item>(to_Do_Items2);
                            }
                        }

                        if (to_Do_Items1.Where(x => x.Reminder != null).Count() != 0)
                        {
                            to_Do_Items.Add(new Grouping<string, To_Do_Item>("KEIN DATUM", to_Do_Items1.Where(x => x.Reminder != null)));
                        }


                    }
                    else
                    {
                        to_Do_Items.Add(new Grouping<string, To_Do_Item>("KEIN DATUM", to_dos));
                    }
                }
                else
                {
                    Show_Empty_List = true;
                }
            }
            finally
            {
                To_Do_Items.Clear();
                To_Do_Items = to_Do_Items;

                if (To_Do_Items.Count == 0)
                {
                    Show_Empty_List = true;

                    if (Show_Tapbar == true)
                    {
                        Show_Select = false;
                        select_all = false;
                        Show_Tapbar = false;
                        Change_Toolbar_Methode(true);
                        Title = origin_title;
                        Flyoutbehavior = FlyoutBehavior.Flyout;
                    }
                }
                else
                {
                    if (Show_Tapbar == true)
                    {
                        Title = "Keine ausgewählt";
                        Check_if_some_Item_is_select();
                    }
                }

                Start_Page.master_To_Do_Page.Update_FlyoutConten();
            }
        }
        public async Task Delet_Range_Methode()
        {
            List<To_Do_Item> to_do_items = new List<To_Do_Item>();

            if (To_Do_Items.Count != 0)
            {
                foreach (var item in To_Do_Items)
                {
                    foreach (var item2 in item.Items)
                    {
                        if (item2.Is_Select == true)
                        { to_do_items.Add(item2); }
                    }
                }
            }

            if (to_do_items.Count != 0)
            {
                string message;
                if (to_do_items.Count == 1)
                {
                    message = "Dieses To-Do dauerhaft löschen";
                }
                else
                {
                    message = to_do_items.Count + " To-Dos dauerhaft löschen?";
                }
                var result = await Shell.Current.ShowPopupAsync(new To_Do_CustomePromt_Popup(to_do_items, message, true));

                if (result != null)
                {
                    await Refresh_Methode();
                }
            }

        }
        public async Task Delet_Methode(To_Do_Item to_do_item)
        {
            if (to_do_item != null)
            {
                var result = await Shell.Current.ShowPopupAsync(new To_Do_CustomePromt_Popup(to_do_item, "Dieses To-Do löschen?", true));

                if (result != null)
                {
                    await Refresh_Methode();
                }
            }
        }
        public async Task Select_All_Methode()
        {
            if (To_Do_Items.Count != 0)
            {
                select_all = !select_all;
                if (select_all == true)
                {
                    foreach (var item in To_Do_Items)
                    {
                        foreach (var item2 in item.Items)
                        {
                            item2.Is_Select = true;
                            await To_DoService.Edit_To_Do(item2);
                        }
                    }
                    Title = To_Do_Items.Count() + " Elemente ausgewählt";
                }
                else
                {
                    foreach (var item in To_Do_Items)
                    {
                        foreach (var item2 in item.Items)
                        {
                            item2.Is_Select = false;
                            await To_DoService.Edit_To_Do(item2);
                        }
                    }
                    Title = "Keine ausgewählt";
                }
                await Refresh_Methode();
                Check_if_some_Item_is_select();
            }
        }
        public async Task Choose1_Methode(To_Do_Item to_do_item)
        {
            Show_Tapbar = true;
            Show_Revive = true;
            Show_Select = true;
            Title = "1 Element ausgewählt";
            Flyoutbehavior = FlyoutBehavior.Disabled;
            to_do_item.Is_Select = true;
            Change_Toolbar_Methode(false);
            await To_DoService.Edit_To_Do(to_do_item);
            await Refresh_Methode();
            Check_if_some_Item_is_select();
        }
        public async Task Choose2_Methode()
        {
            Show_Tapbar = true;
            Show_Revive = false;
            Show_Select = true;
            Title = "Keine ausgewählt";
            Flyoutbehavior = FlyoutBehavior.Disabled;
            Change_Toolbar_Methode(false);
            if (To_Do_Items.Count != 0)
            {
                foreach (var item in To_Do_Items)
                {
                    foreach (var item2 in item.Items)
                    {
                        item2.Is_Select = false;
                        await To_DoService.Edit_To_Do(item2);
                    }
                }
                await Refresh_Methode();
                Check_if_some_Item_is_select();
            }
        }
        public async Task Item_Selected_Methode(To_Do_Item to_do_item)
        {
            await To_DoService.Edit_To_Do(to_do_item);
            await Refresh_Methode();
            Check_if_some_Item_is_select();
        }
        public async Task Cancel_Methode()
        {
            if (To_Do_Items.Count != 0)
            {
                foreach (var item in To_Do_Items)
                {
                    foreach (var item2 in item.Items)
                    {
                        item2.Is_Select = false;
                        await To_DoService.Edit_To_Do(item2);
                    }
                }
                await Refresh_Methode();
            }
            Show_Select = false;
            select_all = false;
            Show_Tapbar = false;
            Change_Toolbar_Methode(true);
            Title = origin_title;
            Flyoutbehavior = FlyoutBehavior.Flyout;
        }
        private async Task Revive_Mehtode()
        {
            List<To_Do_Item> to_do_items = new List<To_Do_Item>();

            if (To_Do_Items.Count != 0)
            {
                foreach (var item in To_Do_Items)
                {
                    foreach (var item2 in item.Items)
                    {
                        if (item2.Is_Select == true)
                        { to_do_items.Add(item2); }
                    }
                }
            }

            if (to_do_items.Count != 0)
            {
                foreach (var item in to_do_items)
                {
                    item.Is_Select = false;
                    item.Is_Removed = false;
                    await To_DoService.Edit_To_Do(item);
                }
                await Refresh_Methode();
            }
        }

        public void Change_Toolbar_Methode(bool input)
        {
            ToDoDeletedPage.ToolbarItems.Clear();

            if (input == true)
            {
                ToDoDeletedPage.ToolbarItems.Add(new ToolbarItem() { Text = "Elemente löschen", Command = Choose2_Command, Order = ToolbarItemOrder.Secondary });
            }
        }
        public void Check_if_some_Item_is_select()
        {
            if (To_Do_Items.Count != 0)
            {
                int count = 0;

                foreach (var item in To_Do_Items)
                {
                    foreach (var item2 in item.Items)
                    {
                        if (item2.Is_Select == true)
                        {
                            count++;
                        }
                    }
                }

                if (count == 0)
                {
                    Some_Item_Select = false;

                    Title = "Keine ausgewählt";
                }
                else
                {
                    Some_Item_Select = true;

                    if (count == 1)
                    {
                        Title = count + " Element ausgewählt";
                    }
                    else
                    {
                        Title = count + " Elemente ausgewählt";
                    }
                }
            }
        }
        private void ViewIsDisappearing_Methode()
        {
            Show_Tapbar = false;
            Show_Select = false;

            if (To_Do_Items.Count != 0)
            {
                foreach (var item in To_Do_Items)
                {
                    foreach (var item2 in item.Items)
                    {
                        item2.Is_Select = false;
                    }
                }
            }
        }



        public AsyncCommand Refresh_Command { get; }
        public AsyncCommand Delet_Range_Command { get; }
        public AsyncCommand<To_Do_Item> Delet_Command { get; }
        public AsyncCommand Select_All_Command { get; }
        public AsyncCommand<To_Do_Item> Choose1_Command { get; }
        public AsyncCommand Choose2_Command { get; }
        public AsyncCommand<To_Do_Item> Item_Selected_Command { get; }
        public AsyncCommand Cancel_Command { get; }
        public AsyncCommand Revive_Command { get; }



        public MvvmHelpers.Commands.Command ViewIsDisappearing_Command { get; }


        public To_Do_Deleted_Page ToDoDeletedPage;


        public bool show_tapbar = false;
        public bool Show_Tapbar
        {
            get { return show_tapbar; }
            set
            {
                if (show_tapbar == value)
                {
                    return;
                }

                show_tapbar = value; RaisePropertyChanged();
            }
        }
        public bool show_select = false;
        public bool Show_Select
        {
            get { return show_select; }
            set
            {
                if (show_select == value)
                {
                    return;
                }

                show_select = value; RaisePropertyChanged();
            }
        }
        public bool select_all = false;
        public bool some_item_select = false;
        public bool Some_Item_Select
        {
            get { return some_item_select; }
            set
            {
                if (value == true)
                {
                    Is_Activ_Color = Color.Black;
                }
                else
                {
                    Is_Activ_Color = Color.Gray;
                }

                if (some_item_select == value)
                {

                    return;
                }

                some_item_select = value; RaisePropertyChanged();
            }
        }
        public bool show_empty_list = false;
        public bool Show_Empty_List
        {
            get { return show_empty_list; }
            set
            {
                if (show_empty_list == value)
                {
                    return;
                }

                show_empty_list = value; RaisePropertyChanged();
            }
        }
        public bool show_revive = false;
        public bool Show_Revive
        {
            get { return show_revive; }
            set
            {
                if (show_revive == value)
                {
                    return;
                }

                show_revive = value; RaisePropertyChanged();
            }
        }


        public FlyoutBehavior flyoutBehavior = FlyoutBehavior.Flyout;
        public FlyoutBehavior Flyoutbehavior
        {
            get { return flyoutBehavior; }
            set
            {
                if (flyoutBehavior == value)
                {
                    return;
                }

                flyoutBehavior = value; RaisePropertyChanged();
            }
        }


        public string origin_title;
        public string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title == value)
                {
                    return;
                }

                title = value; RaisePropertyChanged();
            }
        }



        public Color is_activ_color = Color.Black;
        public Color Is_Activ_Color
        {
            get { return is_activ_color; }
            set
            {
                if (is_activ_color == value)
                {
                    return;
                }

                is_activ_color = value; RaisePropertyChanged();
            }
        }




        public ObservableRangeCollection<Grouping<string, To_Do_Item>> to_do_items;
        public ObservableRangeCollection<Grouping<string, To_Do_Item>> To_Do_Items
        {
            get { return to_do_items; }
            set
            {
                if (To_Do_Items == value)
                {
                    return;
                }

                to_do_items = value; RaisePropertyChanged();
            }
        }

    }
}
