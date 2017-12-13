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

namespace SolarSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OrbitsCalculator calci = new OrbitsCalculator();

        public MainWindow()
        {
            DataContext = calci;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            calci.StartTimer();
        }

        private void btPause_Click(object sender, RoutedEventArgs e)
        {
            calci.PauseTimer();
        }

        private void btReverse_Click(object sender, RoutedEventArgs e)
        {
            calci.ReverseTime();
        }
    }
}
