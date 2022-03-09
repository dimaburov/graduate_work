using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
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
            //Считаем точки для новой линии
            Data_Source();
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
                                    new PenDescription("ewe"));

            //Меняем рисование дополнительного графика
            //Сначала образуем точки заданные пользователем для графика, потом присоединяем полученные точки
            FileDataReader data_file = new FileDataReader();
            List<double> array_data = data_file.readerFile();

            CanvasDrowTimeDelay.AddLineGraph(Set_points(array_data),
                                   new Pen(GetColor(random_color_line), 1),
                                   new PenDescription("dsd"));
            //Линия Xn t
            CanvasDrowTimeDelay.AddLineGraph(viewModel.Data_Xdt_t,
                                   new Pen(GetColor(random_color_line), 1),
                                   new PenDescription("dsd"));
            //Увеличиваем количество линий
            dataLine.CountLine = dataLine.CountLine + 1;
            //Чистим хранилище точек    
            viewModel = new MyViewModel();

        }
        private ObservableDataSource<Point> Set_points(List<double> array_data)
        {
            ObservableDataSource<Point> data_point = new ObservableDataSource<Point>();
            int k = 0;
            for (int i = -array_data.Count(); i < 0; i++, k++)
            {
                data_point.Collection.Add(new Point(i, array_data[k]));
            }

            for (int i = 0; i < data_point.Collection.Count(); i++)
            {
                Console.WriteLine("X: " + data_point.Collection[i].X + "Y: " + data_point.Collection[i].Y);
            }

            return data_point;
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

        private void Data_Source()
        {
            FileDataReader data_file = new FileDataReader();

            double r = double.Parse(r_value.Text);
            double tue = double.Parse(((ComboBoxItem)ComboBoxTueDelay.SelectedItem).Content.ToString());
            List<double> array_data = data_file.readerFile();

            foreach (var item in array_data)
            {
                Console.Write(" "+item.ToString());
            }

            CalculatingPoints caclucationMethod = new CalculatingPoints(new Point(array_data[array_data.Count()-1], 0), 0, tue, 4, int.Parse((1/tue).ToString()), r);
            caclucationMethod.array_data = array_data;
            caclucationMethod.Data_Source();

            viewModel = caclucationMethod.viewModel;
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

        private void Selective_Data_Modification(object sender, RoutedEventArgs e)
        {
            string error_message = ""; 
            FileDataReader data_file = new FileDataReader();
            double tue = double.Parse(((ComboBoxItem)ComboBoxTueDelay.SelectedItem).Content.ToString());
            error_message =  data_file.selectiveChange(int.Parse((1 / tue).ToString()));

            if (error_message != "") MessageBox.Show(error_message);
        }

        //Заполняем файл одинаковыми значениями
        private void Fill_With_The_Same_Values(object sender, RoutedEventArgs e)
        {
            FileDataReader data_file = new FileDataReader();
            double tue = double.Parse(((ComboBoxItem)ComboBoxTueDelay.SelectedItem).Content.ToString());
            data_file.fillDataFail(double.Parse(fille_value.Text), int.Parse((1 / tue).ToString()));
        }
    }
}
