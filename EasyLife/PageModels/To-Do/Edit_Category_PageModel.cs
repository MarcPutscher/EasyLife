using EasyLife.Models;
using EasyLife.Pages;
using EasyLife.Services;
using FreshMvvm;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EasyLife.PageModels
{
    public class Edit_Category_PageModel : FreshBasePageModel
    {
        public Edit_Category_PageModel()
        {
            Categories = new ObservableRangeCollection<Category>();

            Appearing_Command = new AsyncCommand(Appearing_Methode);
            Save_Command = new AsyncCommand(Save_Methode);
            Cancel_Command = new AsyncCommand(Cancel_Methode);
            Item_Select_Command = new AsyncCommand<Category>(Item_Select_Methode);
        }

        public async Task Appearing_Methode()
        {
            List<Category> list = await CategoryService.Get_all_Categorys();

            Categories.Clear();

            foreach (Category category in list)
            {
                if(category.Title != "Sonstige")
                {
                    Categories.Add(category);
                }
            }

            if(Categories.Count == 0)
            {
                await Shell.Current.GoToAsync("..");
                return;
            }

            origin_count = Categories.Count;
        }
        public async Task Save_Methode()
        {
            List<To_Do_Item> to_Do_Items = await To_DoService.Get_all_To_DOs();

            List<Category> all = await CategoryService.Get_all_Categorys();

            Category sonstige = await CategoryService.Get_specific_Category("Sonstige");

            foreach (Category category in all)
            {
                if(category.Is_Select == true)
                {
                    await CategoryService.Remove_Category(category);
                    if(to_Do_Items.Where(x=>x.CategoryID == category.Id).Count() != 0)
                    {
                        foreach(To_Do_Item to_Do_Item in to_Do_Items.Where(x=>x.CategoryID == category.Id))
                        {
                            to_Do_Item.CategoryID = sonstige.Id;
                            await To_DoService.Edit_To_Do(to_Do_Item);
                        }
                    }
                }
                else
                {
                    if (Categories.Where(x => x.Id == category.Id).FirstOrDefault() != null)
                    {

                        if (category.Title != Categories.Where(x => x.Id == category.Id).FirstOrDefault().Title)
                        {
                            category.Title = Categories.Where(x => x.Id == category.Id).FirstOrDefault().Title;
                            await CategoryService.Edit_Category(category);
                        }
                    }
                }
            }

            Start_Page.master_To_Do_Page.Update_FlyoutConten2();

            await Shell.Current.GoToAsync("..");
        }
        public async Task Cancel_Methode()
        {
            List<Category> all = await CategoryService.Get_all_Categorys();

            foreach (Category category in all)
            {
                category.Is_Select = false;
                await CategoryService.Edit_Category(category);
            }

            Start_Page.master_To_Do_Page.Update_FlyoutConten2();

            await Shell.Current.GoToAsync("..");
            return;
        }
        public async Task Item_Select_Methode(Category input)
        {
            input.Is_Select = true;
            Categories.Remove(input);
            await CategoryService.Edit_Category(input);
        }

        public AsyncCommand Save_Command { get; }
        public AsyncCommand Cancel_Command { get; }
        public AsyncCommand Appearing_Command {  get;}
        public AsyncCommand<Category> Item_Select_Command { get; }

        public int origin_count = 0;

        public ObservableRangeCollection<Category> categories;

        public ObservableRangeCollection<Category> Categories
        {
            get { return categories; }
            set
            {
                if (categories == value)
                {
                    return;
                }
                categories = value; RaisePropertyChanged();
            }
        }
    }
}
