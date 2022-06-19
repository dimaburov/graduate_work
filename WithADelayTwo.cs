using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp4
{
    class WithADelayTwo : SetSigFigs
    {
        private List<double> array_data;
        private double r;
        private double tue;
        private double a;
        private double b;
        private double h;
        private int accuracy = 15;

        public WithADelayTwo(List<double> _array_data, double _r, double _tue, double _a, double _b, double _h)
        {
            r = _r;
            array_data = _array_data;
            tue = _tue;
            a = _a;
            b = _b;
            h = _h;
        }

        //Тело метода по вычислению новой точки
        public Point WithADelay(Point p, int k, MyViewModel viewModel)
        {
            Point result = new Point(1, 1);
            int index_x_t = int.Parse(((1 / tue)  - Math.Round(h* int.Parse((1 / tue).ToString()), 2) + k).ToString());
            double x_t = 0;

            Console.WriteLine("index_x_t " + index_x_t);
            if (index_x_t < int.Parse((1 / tue).ToString()))
            {
                x_t = array_data[index_x_t];
            }
            else
            {
                int real_index_x_t = index_x_t - int.Parse((1 / tue).ToString());
                Console.WriteLine("real_index_x_t " + real_index_x_t);
                x_t = viewModel.Data_Xdt_X.Collection[real_index_x_t].Y;
            }
            result = new Point(Math.Round(p.Y, accuracy), Math.Round(p.Y + tue * (f(Math.Exp(x_t * r)) + g(Math.Exp(array_data[k] * r))), accuracy));
            return result;
        }
        //Функция f(x)
        private double f(double x)
        {
            return Math.Round((x - 1) / (-1 * Math.Pow(a, -1) * x - 1), accuracy);
        }
        //Функция g(x)
        private double g(double x)
        {
            return Math.Round(x / (-1*Math.Pow(b, -1) * x - 1), accuracy);
        }


        //test funct u
        private Point function_u(Point p, int k, MyViewModel viewModel)
        {
            Point result = new Point(1, 1);
            int index_x_t = int.Parse(((1 / tue) - Math.Round(h * int.Parse((1 / tue).ToString()), 2) + k).ToString());
            double x_t = 0;

            Console.WriteLine("index_x_t " + index_x_t);
            if (index_x_t < int.Parse((1 / tue).ToString()))
            {
                x_t = array_data[index_x_t];
            }
            else
            {
                int real_index_x_t = index_x_t - int.Parse((1 / tue).ToString());
                Console.WriteLine("real_index_x_t " + real_index_x_t);
                x_t = viewModel.Data_Xdt_X.Collection[real_index_x_t].Y;
            }

            double new_value = Math.Round(p.Y + tue * (f(Math.Exp(x_t * r)) + g(Math.Exp(array_data[k] * r))), accuracy);
            double new_value_r = Math.Round( r * new_value, accuracy);
            double new_exp = Math.Round(Math.Exp(new_value_r), accuracy);
            result = new Point(Math.Round(p.Y, accuracy), new_exp);
            return result;
        }
    }
}
