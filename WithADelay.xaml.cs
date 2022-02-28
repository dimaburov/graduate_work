using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
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
using System.Windows.Shapes;

namespace WpfApp4
{
    /*Уравнение с запаздыванием
      x' = r(1-x(t-1))x
      Начальные условия:
        Заданное количество значений phi(t), где -1 < t < 0
        r, шаг psi, Количество точек n, k = 1/psi, Делаем до n-k > 0 
      Получаем новую точку с помощью уравнения
      xn+1 = xn + psi*r*(1 - xn-k)*xn
     */

    public partial class WithADelay : Window
    {
        MyViewModel viewModel;
        DataLine dataLine;

        public WithADelay()
        {
            InitializeComponent();

            viewModel = new MyViewModel();
            dataLine = new DataLine();
            DataContext = viewModel;
        }
        
        private void Button_Drow_Click(object sender, RoutedEventArgs e)
        {
            //              Рисуем линию по полученым точкам

            //Будем случайно выбирать цвет
            Random color_random = new Random();
            int random_color_line = color_random.Next(0, 5);

            //Линия для основного графика
            LineGraph line_main_graph = new LineGraph();

            //              Рисуем линии
            CanvasDrowDelay.AddLineGraph(viewModel.Data_Xdt_X,
                                    new Pen(GetColor(random_color_line), 0),
                                    new CirclePointMarker { Size = 5.0, Fill = GetColor(random_color_line) },
                                    new PenDescription(""));

            //Меняем рисование дополнительного графика
            //Линия Y t
            CanvasDrowTimeDelay.AddLineGraph(viewModel.Data_X_t,
                                   new Pen(GetColor(random_color_line), 1),
                                   new PenDescription(""));
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

            return Brushes.Blue;
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < dataLine.CountLine; i++)
            {
                CanvasDrowDelay.Children.RemoveAt(13);
                CanvasDrowDelay.Children.RemoveAt(12);

            }
            //Пришлось разнести очищение графика из-за разного количества линий
            //Теперь рисуем только 1 график так что убрао множтель
            for (int i = 0; i < dataLine.CountLine; i++)
            {
                CanvasDrowTimeDelay.Children.RemoveAt(12);
            }
            //Очищаем количество линий
            dataLine.CountLine = 0;
        }
    }
}
