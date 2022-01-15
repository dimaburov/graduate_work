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

        int accuracy =3 ;

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

            //Выбор данных для рисования - ПОКА В КОММЕНТЕ
            //if (Graph_dXdt_X.IsChecked == true) line_main_graph = Drow_Line(viewModel.Data_Xdt_X, random_color_line);
            //if (Graph_dXdt_dtdt.IsChecked == true) line_main_graph = Drow_Line(viewModel.Data_Xdt_Ydt, random_color_line);

            //Определение прозрачная линия или нет

            //Определяем включени/выключение линий
            int line_check = 0;

            if (Line.IsChecked != true) line_check = 0;
            else line_check = 1;

            //              Рисуем линии
            
            CanvasDrow.AddLineGraph(viewModel.Data_Xdt_X,
                                    new Pen(GetColor(random_color_line), line_check),
                                    new CirclePointMarker { Size = 5.0, Fill = GetColor(random_color_line) },
                                    new PenDescription("x'' + "+ double.Parse(TextBoxAlpha.Text) + "(1-x^2)x' + x =0, n = "+ int.Parse(CountN.Text)));
            
            //Меняем рисование дополнительного графика
            //Линия Y t
            CanvasDrowTime.AddLineGraph(viewModel.Data_X_t,
                                   new Pen(GetColor(random_color_line), 1),
                                   new PenDescription("Y x'' + " + double.Parse(TextBoxAlpha.Text) + "(1-x^2)x' + x =0, n = " + int.Parse(CountN.Text)));
            //Второй график для дополнительного графа
            //Линия X t
            CanvasDrowTime.AddLineGraph(viewModel.Data_Xdt_t,
                                   new Pen(GetColor(random_color_line+1), 1),
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
            double alpha = double.Parse(TextBoxAlpha.Text);

            Point start_point = new Point(double.Parse(TextBoxX0.Text), double.Parse(TextBoxY0.Text));
            viewModel.Data_Xdt_X.Collection.Add(start_point);

            //Коллекция точек для графика axisX = t axisY = X
            //viewModel.Data_X_t.Collection.Add(new Point(0, double.Parse(TextBoxX0.Text)));

            //временные ограничения на отображение точек

            //!!очень тестово изменю параметр t
            // !!сделать выбор ля значения 0.1 


            double[] lambda_1_2 = calculation_lambda(alpha);
            double[] const_1_2 = calculation_const(double.Parse(TextBoxX0.Text), double.Parse(TextBoxY0.Text), lambda_1_2[0], lambda_1_2[1]);

            //Метод Рунге-Кутта
            //double h = 0.1;
            Point new_point = start_point;

            if (ChechMethod == 1)
            {
                new_point = function_get_new_point(start_point, alpha);
            }
            else if(ChechMethod == 2)
            {
                double[] runge_kutte = runge_kutta(0, start_point, alpha);
                new_point.X = runge_kutte[0];
                new_point.Y = runge_kutte[1];
            }
            else
            {
                new_point = runge_kutta_2(0, start_point, alpha);
            }

            viewModel.Data_Xdt_X.Collection.Add(new_point);
            double step = double.Parse(((ComboBoxItem)ComboBoxStep.SelectedItem).Content.ToString());
            for (double t = step; t < int.Parse(CountN.Text); t=t+step)
            {
                //test ограничение на размеры
                if (new_point.X > 5000000 || new_point.Y > 500000) return;
                if (new_point.X < -500000 || new_point.Y < -500000) return;

                {
                    {
                        //  Временно
                        // Условие на выбор графика
                        //double x = new_point.X;

                        //new_point = function_get_new_point(new_point, alpha);
                        //if (Graph_dXdt_X.IsChecked == true)
                        //{
                        //Коллекция точек для графика axisX = X axisY = X' 

                        //!!ОЧЕНЬ тестовое изменение домножамем на t

                        //Наименование осей
                        //}
                        //if (Graph_dXdt_dtdt.IsChecked == true)
                        //{
                        //    //Коллекция точек для графика axisX = X' axisY = Y' 
                        //    viewModel.Data_Xdt_Ydt.Collection.Add(new Point(new_point.Y, new_point.X));

                        //    //Наименование осей
                        //}
                        //Коллекция точек для графика axisX = t axisY = X
                    }

                    //          Новое изменение
                    // x(t) = C1 * exp^(t*λ1)/λ1 + C2 * exp^(t*λ2)/λ2
                    // y(t) = C1 * exp^(t*λ1) + C2 * exp^(t*λ2

                    //double xt = SetSigFigs(const_1_2[0] * Math.Exp(t * lambda_1_2[0]) / lambda_1_2[0] + const_1_2[0] * Math.Exp(t * lambda_1_2[1]) / lambda_1_2[1], accuracy) ;
                    //double yt = SetSigFigs(const_1_2[0] * Math.Exp(t * lambda_1_2[0]) + const_1_2[0] * Math.Exp(t * lambda_1_2[1]), accuracy);

                    //double xt = SetSigFigs(Math.Exp(lambda_1_2[0]*t)*(const_1_2[0]*Math.Cos(lambda_1_2[1]*t) + const_1_2[1]*Math.Sin(lambda_1_2[1]*t)), accuracy);
                    //double yt = SetSigFigs(lambda_1_2[0]*xt + Math.Exp(lambda_1_2[0]*t)*(-const_1_2[0]*lambda_1_2[1]*Math.Sin(lambda_1_2[1]*t) + const_1_2[1]*lambda_1_2[1]*Math.Cos(lambda_1_2[1]*t)), accuracy);
                }
                
                if (ChechMethod == 1)
                {
                    //Разностный метод
                    new_point = function_get_new_point(new_point, alpha);
                }
                else if (ChechMethod == 2)
                {
                    //Метод Рунге-Кутта 4
                    Point variable_point = new_point;
                    double[] runge_kutte = runge_kutta(step, new_point, alpha);
                    new_point.X = runge_kutte[0];
                    new_point.Y = runge_kutte[1];
                }
                else
                {
                    //Метод Рунге-Кутта 2 
                    new_point = runge_kutta_2(step, new_point, alpha);
                }
                


                Console.WriteLine("X = " + new_point.X + " Y = " + new_point.Y);
                //Ввод данных для таблиц
                viewModel.Data_Xdt_X.Collection.Add(new_point);
                //Коллекция точек для графика axisX = t axisY = X
                viewModel.Data_Xdt_t.Collection.Add(new Point(t, new_point.X));
                //Коллекция точек для графика axisX = t axisY = Y
                viewModel.Data_X_t.Collection.Add(new Point(t, new_point.Y));


            }
        }

        //Функция вычисления точек
        private Point function_get_new_point(Point p, double alpha)
        {
            //            Function
            // dx/dt = y
            // dy/dt = -alpha(1 - x^2)*y - x
            double tau = double.Parse(((ComboBoxItem)ComboBoxTue.SelectedItem).Content.ToString());
            {
                //test value
                Console.WriteLine("X = " + SetSigFigs(p.X + tau * p.Y, accuracy) + " Y = " + SetSigFigs(p.Y + tau * (-alpha * (1 - p.X * p.X) * p.Y - p.X), accuracy));
            }
            Point new_point = new Point(SetSigFigs(p.X + tau*p.Y, accuracy), SetSigFigs(p.Y + tau*(-alpha * (1 - p.X * p.X) * p.Y - p.X), accuracy));

            //test point

            return new_point;
        }

        //              Test для новых алгоритмов подсчёта

        //Метод Рунге-Кутты 2-го порядка
        private Point runge_kutta_2(double h, Point p, double alpha)
        {
            double new_x = p.X + h * function_x(p, alpha);
            double new_y = p.Y + h * function_y(p, alpha);

            double x = p.X + h * (function_x(p, alpha) + function_x(new Point(new_x, p.Y), alpha)) / 2;
            double y = p.Y + h * (function_y(p, alpha) + function_y(new Point(p.X, new_y), alpha)) / 2;

            return new Point(SetSigFigs(x,accuracy), SetSigFigs(y,accuracy));
        }

        //Метод Рунге-Кутта 4-го порядка
        //Y'
        private double[] runge_kutta(double h, Point start_p, double alpha)
        {
            double step = double.Parse(((ComboBoxItem)ComboBoxStep.SelectedItem).Content.ToString());

            double x1 = function_x(start_p, alpha);
            double y1 = function_y(start_p, alpha);

            double x2 = function_x(new Point(start_p.X + step * x1/ 2.0, start_p.Y + step/ 2.0), alpha);
            double y2 = function_y(new Point(start_p.X + step/2.0, start_p.Y+ step * y1 /2.0), alpha);

            double x3 = function_x(new Point(start_p.X + step * x2 / 2.0, start_p.Y + step / 2.0), alpha);
            double y3 = function_y(new Point(start_p.X + step / 2.0, start_p.Y + step * y2 / 2.0), alpha);

            double x4 = function_x(new Point(start_p.X + step * x3, start_p.Y + step ), alpha);
            double y4 = function_y(new Point(start_p.X + step, start_p.Y + step * y3), alpha);

            Console.WriteLine("Шаг x " + h + " k1 " + x1 + " k2 " + x2 + " k3 " + x3 + " k4 " + x4);
            Console.WriteLine("Шаг y " + h + " k1 " + y1 + " k2 " + y2 + " k3 " + y3 + " k4 " + y4);

            double[] rezult = new double[2];
            if (x4 > 50000) return rezult;
            if (y4 > 50000) return rezult;
            rezult[0] = SetSigFigs(start_p.X + step*(x1 + 2.0 * x2 + 2.0 * x3 + x4) / 6.0, accuracy);
            rezult[1] = SetSigFigs(start_p.Y + step*(y1 + 2.0 * y2 + 2.0 * y3 + y4) / 6.0, accuracy);

            return rezult;
        }

        //Функция для x'
        private double function_x(Point p, double alpha)
        {
            return SetSigFigs(p.Y,accuracy);
        }
        //Функция для y'
        private double function_y(Point p, double alpha)
        {
            return SetSigFigs(-alpha * (1 - p.X * p.X) * p.Y - p.X, accuracy);
        }



        //Вычисление констант по начальным условиям
        private double[] calculation_const(double const1, double const2,double lambda1, double lambda2)
        {
            double[] new_const_1_2 = new double[2];


            //new_const_1_2[1] = SetSigFigs((const1* lambda1 - const2)* lambda2 / (lambda1 - lambda2), accuracy);
            //new_const_1_2[0] = SetSigFigs(const2 - new_const_1_2[1], accuracy);

            new_const_1_2[0] = SetSigFigs(const1, accuracy);
            new_const_1_2[1] = SetSigFigs((const2 - lambda1*const1)/lambda2, accuracy);


            Console.WriteLine("new_const_1_2[0] = " + new_const_1_2[0] + " new_const_1_2[1] = " + new_const_1_2[1]);

            return new_const_1_2;
        }

        //Вычисление лямбд
        private double[] calculation_lambda(double alpha)
        {
            double[] labda_1_2 = new double[2];

            //labda_1_2[0] = SetSigFigs((-alpha - Math.Sqrt(alpha * alpha - 4)) / 2, 3);
            //labda_1_2[1] = SetSigFigs((-alpha + Math.Sqrt(alpha * alpha - 4)) / 2, 3);


            //Dictionary<string, ComplexNumber> lambda = new Dictionary<string, ComplexNumber>();
            //lambda.Add("Lambda1", new ComplexNumber { real = 1, imaginary = 2 });

            //Посчитаем новые константы      !!sqrt(alpha^2 - 4) - !!пока реализация, если корень отрицательный
            // labda_1_2[0]  = -alpha/2    labda_1_2[1] = sqrt(alpha^2 - 4)/2
            labda_1_2[0] = SetSigFigs(-alpha / 2, accuracy);
            labda_1_2[1] = SetSigFigs((alpha * alpha - 4) / 2, accuracy);

            Console.WriteLine("labda_1_2[0] = " + labda_1_2[0] + " labda_1_2[1] = " + labda_1_2[1]);

            return labda_1_2;
        }

        //test delete line
        private void Button_All_Clear_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dataLine.CountLine; i++)
            {
                CanvasDrow.Children.RemoveAt(13);
                CanvasDrow.Children.RemoveAt(12);
            }
            //Пришлось разнести очищение графика из-за разного количества линий
            for (int i = 0; i < dataLine.CountLine*2; i++)
            {
                CanvasDrowTime.Children.RemoveAt(12);
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
    }
}
