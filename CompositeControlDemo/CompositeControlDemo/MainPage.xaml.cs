using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CompositeControlDemo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            List<string> list = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                list.Add("Name" + i);
            }
            CustomControl.ItemsSource2 = list;
        }
    }
}
