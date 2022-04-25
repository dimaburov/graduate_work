using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;

namespace WpfApp4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //MainWindow MainWin = new MainWindow();

        public MainWindow()
        {
            InitializeComponent();
        }

        //Обрабаываем выбранное окно
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (VanDerPolEq.IsChecked == true)
            {
                VanDerPol vanDerPolEquation = new VanDerPol();
                vanDerPolEquation.Show();
            }
            if (WithADelayEq.IsChecked == true)
            {
                WithADelay withDelay = new WithADelay();
                withDelay.Show();
            }
            if (WithADelayOne.IsChecked == true)
            {
                WithADelayOneEq withDelayOne = new WithADelayOneEq();
                withDelayOne.Show();
            }
            if (WithADelayTwo.IsChecked == true)
            {
                WithADelayTwoEq withDelayTwo = new WithADelayTwoEq();
                withDelayTwo.Show();
            }
        }
   
    }
}
