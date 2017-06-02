using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PO_wpf
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Heli h = new Heli(10,100);
            Image hi = new Image();

            hi.Source = new BitmapImage(new Uri(h.img));

            hi.Margin = new Thickness(8,8,0,0);
            hi.Height = 10;
            hi.Width = 10;
            hi.HorizontalAlignment = HorizontalAlignment.Left;
            hi.VerticalAlignment = VerticalAlignment.Top;

            InitializeComponent();

            Map.Children.Add(hi);
        }
    }
}
