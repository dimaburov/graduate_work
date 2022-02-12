using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp4
{
    class CalculatingPoints
    {
        public double alpha;
        public Point start_point;
        public double step;
        public int ChechMethod;
        public int CountStep;

        public MyViewModel viewModel { get; set; }

        public CalculatingPoints(Point _start_point, double _alpha, double _step, int _ChechMethod, int _CountStep)
        {
            alpha = _alpha;
            start_point = _start_point;
            step = _step;
            ChechMethod = _ChechMethod;
            CountStep = _CountStep;
        }

        //Вычисление точек для линии
        public void Data_Source()
        {
            viewModel = new MyViewModel();

            viewModel.Data_Xdt_X.Collection.Add(start_point);
            //Коллекция точек для графика axisX = t axisY = X
            viewModel.Data_Xdt_t.Collection.Add(new Point(0, start_point.X));
            //Коллекция точек для графика axisX = t axisY = Y
            viewModel.Data_X_t.Collection.Add(new Point(0, start_point.Y));

            Point new_point = start_point;
            MethodEulera method_eulera = new MethodEulera(alpha,step);
            RungeKutta2 runge_kutta2 = new RungeKutta2(alpha, step);
            RungeKutta4 runge_kutta4 = new RungeKutta4(alpha, step);

            if (ChechMethod == 1)
            {
                new_point = method_eulera.methodEulera(start_point);
            }
            else if (ChechMethod == 2)
            {
                double[] runge_kutte = runge_kutta4.runge_kutta(start_point);
                new_point.X = runge_kutte[0];
                new_point.Y = runge_kutte[1];
            }
            else
            {
                new_point = runge_kutta2.runge_kutta_2(start_point);
            }

            viewModel.Data_Xdt_X.Collection.Add(new_point);
            for (double t = step; t < CountStep; t = t + step)
            {
                //test ограничение на размеры
                if (new_point.X > 5000000 || new_point.Y > 500000) return;
                if (new_point.X < -500000 || new_point.Y < -500000) return;


                if (ChechMethod == 1)
                {
                    //Метод Эйлера
                    new_point = method_eulera.methodEulera(new_point);
                }
                else if (ChechMethod == 2)
                {
                    //Метод Рунге-Кутта 4
                    double[] runge_kutte = runge_kutta4.runge_kutta(new_point);
                    new_point.X = runge_kutte[0];
                    new_point.Y = runge_kutte[1];
                }
                else
                {
                    //Метод Рунге-Кутта 2 
                    new_point = runge_kutta2.runge_kutta_2(new_point);
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

    }
}
