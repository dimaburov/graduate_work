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
        MyViewModel viewModel;
        DataLine dataLine;

        int accuracy = 3;

        int ChechMethod = 0;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new MyViewModel();
            dataLine = new DataLine();
            DataContext = viewModel;
        }

        public static double SetSigFigs(double d, int digits)
        {
            if (d == 0)
                return 0;

            decimal scale = (decimal)Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);

            return (double)(scale * Math.Round((decimal)d / scale, digits));
        }

        private void Button_Drow_Click(object sender, RoutedEventArgs e)
        {
            //Считаем точки для новой линии
            Data_Source();

            //              Рисуем линию по полученым точкам

            //Будем случайно выбирать цвет
            Random color_random = new Random();
            int random_color_line = color_random.Next(0, 5);

            //Линия для основного графика
            LineGraph line_main_graph = new LineGraph();

            //Определяем включени/выключение линий
            int line_check = 0;

            if (Line.IsChecked != true) line_check = 0;
            else line_check = 1;

            //              Рисуем линии

            CanvasDrow.AddLineGraph(viewModel.Data_Xdt_X,
                                    new Pen(GetColor(random_color_line), line_check),
                                    new CirclePointMarker { Size = 5.0, Fill = GetColor(random_color_line) },
                                    new PenDescription("x'' + " + double.Parse(TextBoxAlpha.Text) + "(1-x^2)x' + x =0, n = " + int.Parse(CountN.Text)));

            //Меняем рисование дополнительного графика
            //Линия Y t
            CanvasDrowTime.AddLineGraph(viewModel.Data_X_t,
                                   new Pen(GetColor(random_color_line), 1),
                                   new PenDescription("Y x'' + " + double.Parse(TextBoxAlpha.Text) + "(1-x^2)x' + x =0, n = " + int.Parse(CountN.Text)));
            //Второй график для дополнительного графа
            //Линия X t
            CanvasDrowTime.AddLineGraph(viewModel.Data_Xdt_t,
                                   new Pen(GetColor(random_color_line + 1), 1),
                                   new PenDescription("X x'' + " + double.Parse(TextBoxAlpha.Text) + "(1-x^2)x' + x =0, n = " + int.Parse(CountN.Text)));
            //Увеличиваем количество линий
            dataLine.CountLine = dataLine.CountLine + 1;
            //Чистим хранилище точек    
            viewModel = new MyViewModel();

        }

        //Возвращаем цвет для линии
        private SolidColorBrush GetColor(int color_number)
        {
            switch (color_number)
            {
                case 0:
                    return Brushes.Blue;
                case 1:
                    return Brushes.Red;
                case 2:
                    return Brushes.Yellow;
                case 3:
                    return Brushes.Green;
                case 4:
                    return Brushes.Black;
                case 5:
                    return Brushes.Orange;
            }
            //test

            return Brushes.Blue;
        }

        //Вычисление точек для линии
        private void Data_Source()
        {
            double step = double.Parse(((ComboBoxItem)ComboBoxTue.SelectedItem).Content.ToString());
            double alpha = double.Parse(TextBoxAlpha.Text);
            Point start_point = new Point(double.Parse(TextBoxX0.Text), double.Parse(TextBoxY0.Text));
            CalculatingPoints CalculationMethod = new CalculatingPoints(start_point, alpha, step, ChechMethod, int.Parse(CountN.Text));
            CalculationMethod.Data_Source();
            viewModel = CalculationMethod.viewModel;

        }
       
        //test delete line
        private void Button_All_Clear_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dataLine.CountLine; i++)
            {
                CanvasDrow.Children.RemoveAt(13);
                CanvasDrow.Children.RemoveAt(12);

                //CanvasDrow.Children.RemoveAt(12);
                //CanvasDrow.Children.RemoveAt(11);
            }
            //Пришлось разнести очищение графика из-за разного количества линий
            for (int i = 0; i < dataLine.CountLine*2; i++)
            {
                CanvasDrowTime.Children.RemoveAt(12);

                //CanvasDrowTime.Children.RemoveAt(11);
            }
            //Очищаем количество линий
            dataLine.CountLine = 0;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            if (pressed.Content.ToString() == "Метод Эйлера") ChechMethod = 1;
            else if(pressed.Content.ToString() == "Метод Рунге-Кутта 4-го") ChechMethod = 2;
            else ChechMethod = 3;
        }

        //Убираем бокс наименований для линий
        private void ClearNameLine_Click(object sender, RoutedEventArgs e)
        {
            CanvasDrowTime.Children.RemoveAt(9);
            CanvasDrow.Children.RemoveAt(9);
        }
    }
}
