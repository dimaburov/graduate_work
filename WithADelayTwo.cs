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
        private double sigma;
        private int accuracy = 15;

        public WithADelayTwo(List<double> _array_data, double _r, double _tue, double _a, double _b, double _h, double _sigma)
        {
            r = _r;
            array_data = _array_data;
            tue = _tue;
            a = _a;
            b = _b;
            h = _h;
            sigma = _sigma;
        }

        //Тело метода по вычислению новой точки
        public Point WithADelay(Point p, int k, MyViewModel viewModel)
        {
            Point result = new Point(1, 1);
            //result = new Point(SetSigFig(p.Y, accuracy), SetSigFig(p.Y * Math.Exp(r * tue * f(array_data[k])), accuracy));
            int index_x_t = int.Parse((Math.Round(h*(1 / tue),1) - k).ToString());
            double x_t = 0;
            //test
            h = 1;

            Console.WriteLine("index_x_t " + index_x_t);
            if (index_x_t > 0)
            {
                int real_index_x_t = int.Parse((sigma / tue).ToString()) - index_x_t;
                x_t = array_data[real_index_x_t];
            }
            else
            {
                int real_index_x_t = -1* index_x_t;
                x_t = viewModel.Data_Xdt_X.Collection[real_index_x_t].Y;
            }

            Console.WriteLine("result With A delay " + Math.Round(p.Y * Math.Exp(r * tue * (f(x_t) + g(array_data[k]))), accuracy));
            //result = new Point(Math.Round(p.Y, accuracy), Math.Round(p.Y * Math.Exp(r * tue * (f(x_t) + g(array_data[k]))), accuracy));
            //result = new Point(Math.Round(p.Y, accuracy), Math.Round(p.Y + tue * (f(Math.Exp(array_data[k] * r)) + g(Math.Exp(x_t * r))), accuracy));
            //result = new Point(Math.Round(p.Y, accuracy), Math.Round(p.Y + tue * (f(Math.Exp(x_t * r)) + g(Math.Exp(array_data[k] * r))), accuracy));
            result = new Point(Math.Round(p.Y, accuracy), Math.Round(p.Y + tue * (f(Math.Exp(array_data[k] * r)) + g(Math.Exp(x_t * r))), accuracy));
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
            return Math.Round(x / (-1 * Math.Pow(b, -1) * x - 1), accuracy);
        }
    }
}
