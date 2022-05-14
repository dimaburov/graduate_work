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
    /// <summary>
    /// Логика взаимодействия для WithADelayTwoEq.xaml
    /// </summary>
    public partial class WithADelayTwoEq : Window
    {
        MyViewModel viewModelInput;
        MyViewModel viewModelResult;
        DataLine dataLine;
        TextBlock txtBox = new TextBlock();

        public WithADelayTwoEq()
        {
            InitializeComponent();
            viewModelResult = new MyViewModel();
            viewModelInput = new MyViewModel();
            dataLine = new DataLine();
            DataContext = viewModelResult;
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
            CanvasDrowDelay.AddLineGraph(viewModelResult.Data_Xdt_t,
                                    new Pen(GetColor(random_color_line), 0),
                                    new CirclePointMarker { Size = 5.0, Fill = GetColor(random_color_line) },
                                    new PenDescription("r = " + double.Parse(r_value.Text) + " a = " + double.Parse(a_value.Text)+ " h = " + double.Parse(h_value.Text)));

            //Меняем рисование дополнительного графика
            //Сначала образуем точки заданные пользователем для графика, потом присоединяем полученные точки
            FileDataReader data_file = new FileDataReader();
            List<double> array_data = data_file.readerFile();

            CanvasDrowTimeDelay.AddLineGraph(Set_points(array_data),
                                   new Pen(GetColor(random_color_line), 1),
                                   new PenDescription(" "));
            //Линия Xn t
            CanvasDrowTimeDelay.AddLineGraph(viewModelResult.Data_Xdt_Ydt,
                                   new Pen(GetColor(random_color_line), 1),
                                   new PenDescription("r = " + double.Parse(r_value.Text) + " a = " + double.Parse(a_value.Text) + " h = " + double.Parse(h_value.Text)));
            //Увеличиваем количество линий
            dataLine.CountLine = dataLine.CountLine + 1;
            //Чистим хранилище точек    
            viewModelResult = new MyViewModel();

        }
        private ObservableDataSource<Point> Set_points(List<double> array_data)
        {
            ObservableDataSource<Point> data_point = new ObservableDataSource<Point>();
            int k = 0;
            double h = double.Parse(h_value.Text);
            double tue = double.Parse(((ComboBoxItem)ComboBoxTueDelay.SelectedItem).Content.ToString());
            for (int i = -(array_data.Count() - 1); i <= 0; i++, k++)
            {
                data_point.Collection.Add(new Point(i * tue, array_data[k]));
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
                    return Brushes.DarkKhaki;
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
            double a = double.Parse(a_value.Text);
            double b = double.Parse(b_value.Text);
            double h = double.Parse(h_value.Text);
            List<double> array_data = data_file.readerFile();

            //Тестовый обход в 10 шагов
            for (int i = 0; i < int.Parse(countStep.Text); i++)
            {
                //Проход делается тестовые 10 раз - ограничений на минимальное число и очень большое
                CalculatingPoints caclucationMethod = new CalculatingPoints(new Point(array_data[array_data.Count() - 2], array_data[array_data.Count() - 1]), 0, tue, 6, int.Parse((h / tue).ToString()), r, a, b, h);
                caclucationMethod.array_data = array_data;
                caclucationMethod.Data_Source();
                viewModelInput = caclucationMethod.viewModel;
                //Будем перезаписывать значения из Data_xdt_x  в Data_Xdt_t - вывод на 1 графике
                //В Data_Xdt_t будет [array(i),Data_xdt_x[i]]
                for (int j = 0; j < viewModelInput.Data_Xdt_X.Collection.Count(); j++)
                {
                    viewModelResult.Data_Xdt_t.Collection.Add(new Point(array_data[j], viewModelInput.Data_Xdt_X.Collection[j].Y));
                    //Записываем новые значения array_data
                    array_data[j] = viewModelInput.Data_Xdt_X.Collection[j].Y;
                }
                //Перезаписываем Data_X_t в Data_Xdt_Ydt  - вывод на 2 графияке - менять на правильное время относительно шага
                for (int j = 0; j < viewModelInput.Data_X_t.Collection.Count(); j++)
                {
                    viewModelResult.Data_Xdt_Ydt.Collection.Add(new Point(viewModelInput.Data_X_t.Collection[j].X + h*i, viewModelInput.Data_X_t.Collection[j].Y));
                }
            }

        }
        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {

            for (int i = 0; i < dataLine.CountLine; i++)
            {
                CanvasDrowDelay.Children.RemoveAt(13);
                CanvasDrowDelay.Children.RemoveAt(12);

            }
            //Пришлось разнести очищение графика из-за разного количества линий
            for (int i = 0; i < dataLine.CountLine * 2; i++)
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
            error_message = data_file.selectiveChange(int.Parse((1 / tue).ToString()));

            if (error_message != "") MessageBox.Show(error_message);
        }

        //Заполняем файл одинаковыми значениями
        private void Fill_With_The_Same_Values(object sender, RoutedEventArgs e)
        {
            int function_param = 0;
            double h = double.Parse(h_value.Text);
            FileDataReader data_file = new FileDataReader();
            double tue = double.Parse(((ComboBoxItem)ComboBoxTueDelay.SelectedItem).Content.ToString());
            data_file.fillDataFail(double.Parse(fille_value.Text), int.Parse((h / tue).ToString()), function_param);

            drow_button.IsEnabled = true;
        }

        private void fill_value_equals_h()
        {
            double a = double.Parse(a_value.Text);
            double r = double.Parse(r_value.Text);
            double T = 2 + a + 1 / a;
            double t_0 = 1 + 1 / a;
            txtBox.Name = "Equals_h";
            txtBox.Text = Math.Round((r - 1) * T + t_0 + 1,2) + " < h < " + Math.Round(r * T,2);
            txtBox.Width = 108;
            txtBox.Height = 18;
            txtBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            txtBox.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            txtBox.Margin = new Thickness(751,470,0,0);
            txtBox.FontWeight = FontWeights.Bold;
            MainGrid.Children.Add(txtBox);
        }
        private void Fille_value_TextChanged(object sender, TextChangedEventArgs e)
        {
            drow_button.IsEnabled = false;
        }

        private void H_value_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            drow_button.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Children.Remove(txtBox);
            fill_value_equals_h();
        }




        //private void Sigma_value_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    //Выводим ограничения по h
        //    limitations_h.Text = "";
        //    limitations_h.Text = "0 < h < " + sigma_value.Text;
        //}

    }
}
