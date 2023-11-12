using ColorPicker.BaseClasses;
using ColorPicker.BaseClasses.ColorPickerEventArgs;
using EasyLife.CustomeEventArgs;
using EasyLife.Pages;
using iText.Kernel.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.CustomeControle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomePicker : ContentView 
    {
        public CustomePicker()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create(nameof(ItemSource),
            typeof(IList),
            typeof(CustomePicker),
            defaultValue: null,
            defaultBindingMode: BindingMode.OneWay);

        public IList ItemSource
        {
            get { return (IList)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }


        public static readonly BindableProperty ItemTextProperty = BindableProperty.Create(nameof(ItemText),
            typeof(string),
            typeof(CustomePicker),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: ItemTextPropertyChanged);

        private static void ItemTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomePicker)bindable;
            control.Item.Text = newValue?.ToString();

            if (control.Item.Text != null)
            {
                control.Item.IsVisible = true;

                control.Holder.IsVisible = false;
            }

            if (newValue == null || String.IsNullOrWhiteSpace(newValue.ToString()))
            {
                control.Item.IsVisible = false;

                control.Holder.IsVisible = true;
            }

            if (oldValue != newValue)
            {
                control.RaiseItemChanged((string)oldValue, (string)newValue);
            }
        }
        protected virtual void RaiseItemChanged(string oldColor, string newColor)
        {
            ItemChanged?.Invoke(this, new ItemChangedEventArgs(oldColor, newColor));
        }

        public string ItemText
        {
            get
            {
                return base.GetValue(ItemTextProperty)?.ToString();
            }

            set
            {
                base.SetValue(ItemTextProperty, value);
            }
        }

        public event EventHandler<ItemChangedEventArgs> ItemChanged;

        public static readonly BindableProperty ItemColorProperty = BindableProperty.Create(nameof(ItemColor),
            typeof(Color),
            typeof(CustomePicker),
            defaultValue: Color.Black,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: ItemColorPropertyChanged);

        private static void ItemColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomePicker)bindable;
            control.Item.TextColor = (Color)newValue;
        }

        public Color ItemColor
        {
            get
            {
                return (Color)base.GetValue(ItemColorProperty);
            }

            set
            {
                base.SetValue(ItemColorProperty, value);
            }
        }


        public static readonly BindableProperty PlaceholderTextProperty = BindableProperty.Create(nameof(PlaceholderText),
            typeof(string),
            typeof(CustomePicker),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: PlaceholderTextPropertyChanged);

        private static void PlaceholderTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomePicker)bindable;
            control.Holder.Text = newValue?.ToString();

            if(control.Item.Text == null)
            {
                control.Item.IsVisible = false;
            }
            else
            {
                control.Item.IsVisible = true;
            }
        }

        public string PlaceholderText
        {
            get
            {
                return base.GetValue(PlaceholderTextProperty)?.ToString();
            }

            set
            {
                base.SetValue(PlaceholderTextProperty, value);
            }
        }

        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor),
            typeof(Color),
            typeof(CustomePicker),
            defaultValue: Color.Gray,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: PlaceholderColorPropertyChanged);

        private static void PlaceholderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomePicker)bindable;
            control.Holder.TextColor = (Color)newValue;
        }

        public Color PlaceholderColor
        {
            get
            {
                return (Color)base.GetValue(PlaceholderColorProperty);
            }

            set
            {
                base.SetValue(PlaceholderColorProperty, value);
            }
        }


        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Titel),
            typeof(string),
            typeof(CustomePicker),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.OneWay);

        public string Titel
        {
            get
            {
                return base.GetValue(TitleProperty)?.ToString();
            }

            set
            {
                base.SetValue(TitleProperty, value);
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            List<string> itemlist = ItemSource.Cast<string>().ToList();

            var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup(Titel,350, itemlist));

            if(result == null)
            {
                return;
            }
            else
            {
                ItemText = (string)result;

                Item.Text = (string)result;

                Item.IsVisible = true;

                Holder.IsVisible = false;
            }
        }
    }
}