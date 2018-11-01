using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CompositeControlDemo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyControl : ContentView
	{
        public MyControl()
        {
            InitializeComponent();

            privateSuggestionList = new ObservableCollection<string>();

            this.IsEntryVisible = false;
            ListHeight = 0;
            innerEntry.SetBinding(Entry.PlaceholderProperty, new Binding("Placeholder", source: this));
            innerEntry.SetBinding(Entry.TextProperty, new Binding("Text", source: this));
            innerSuggestionBox.SetBinding(ListView.ItemsSourceProperty, new Binding("privateSuggestionList", source: this));

            innerEntry.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeSentence | KeyboardFlags.Spellcheck);
            //innerSuggestionBox.IsVisible = false;
            //innerSuggestionBox.SetBinding(ListView.IsVisibleProperty, new Binding("IsEntryVisible", source: this));
        }



        public static BindableProperty PlaceholderProperty =
            BindableProperty.Create("Placeholder", typeof(string), typeof(MyControl), default(string));

        public string Placeholder
        {
            // ----- The display text for the composite control.
            get { return (string)base.GetValue(PlaceholderProperty); }
            set
            {
                if (value != this.Placeholder)
                    base.SetValue(PlaceholderProperty, value);
            }
        }

        public static BindableProperty TextProperty = BindableProperty.Create(
            propertyName: "Text",
            returnType: typeof(string),
            declaringType: typeof(MyControl),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: HandleTextPropertyChanged);

        public string Text
        {
            // ----- The display text for the composite control.
            get { return (string)base.GetValue(TextProperty); }
            set
            {
                if (value != this.Text)
                    base.SetValue(TextProperty, value);
            }
        }

        private static void HandleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            MyControl targetView;

            targetView = (MyControl)bindable;
            if (targetView != null)
                targetView.Text = (string)newValue;
            targetView.IsEntryVisible = true;
        }

        private bool _isEntryVisible;
        public bool IsEntryVisible
        {
            get { return _isEntryVisible; }
            set
            {
                _isEntryVisible = value;
                //RaisePropertyChanged(() => IsEntryVisible);
                OnPropertyChanged();
            }
        }

        private int _listHeight;
        public int ListHeight
        {
            get { return _listHeight; }
            set
            {
                _listHeight = value;
                //RaisePropertyChanged(() => IsEntryVisible);
            }
        }

        public ObservableCollection<string> privateSuggestionList { get; set; }

        public ICommand SuggestionItemTapped => new Command<string>(onSuggestionTapped);

        bool didTapSuggestion = false;
        private void onSuggestionTapped(string item)
        {
            didTapSuggestion = true;
            this.Text = item;
            Debug.WriteLine("List disabled");
            //innerSuggestionBox.IsVisible = false;
            IsEntryVisible = false;
            Debug.WriteLine("IsEntryVisible value: {0}", IsEntryVisible);
        }

        public ICommand EntryTextChanged => new Command(onEntryTextChanged);

        private void onEntryTextChanged()
        {
            if (this.ItemsSource2 != null && !didTapSuggestion)
            {
                List<string> list = new List<string>();
                list.AddRange(ItemsSource2.Cast<string>()
                               .Where(x => x.ToLower().StartsWith(this.Text.ToLower(), StringComparison.Ordinal))
                               .OrderBy(x => x)
                               .ToList());
                this.privateSuggestionList.Clear();
                foreach (string s in list)
                {
                    privateSuggestionList.Add(s);
                }
                Debug.WriteLine("List Enabled");
                //innerSuggestionBox.IsVisible = true;
                IsEntryVisible = true;
                ListHeight = 70;
                Debug.WriteLine("IsEntryVisible value: {0}", IsEntryVisible);
            }
            didTapSuggestion = false;
        }

        public ICommand EntryTextLeft => new Command(onEntryTextLeft);

        private void onEntryTextLeft()
        {
            Debug.WriteLine("List disabled");
            IsEntryVisible = false;
            ListHeight = 0;
            //innerSuggestionBox.IsVisible = false;
        }

        public static readonly BindableProperty ItemsSource2Property =
            BindableProperty.Create(nameof(ItemsSource2), typeof(IEnumerable), typeof(MyControl), null,
                                     BindingMode.Default, null, OnItemsSource2Changed);

        public IEnumerable ItemsSource2
        {
            get { return (IEnumerable)GetValue(ItemsSource2Property); }
            set { SetValue(ItemsSource2Property, value); }
        }

        static void OnItemsSource2Changed(BindableObject bindable, object oldvalue, object newvalue)
        {
            System.Diagnostics.Debug.WriteLine("source changed");
        }
    }
}