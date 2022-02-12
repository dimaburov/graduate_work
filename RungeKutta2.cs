using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp4
{
    class RungeKutta2 : SetSigFigs
    {
        private double h;
        private double alpha;

        private int accuracy = 3;

        public RungeKutta2(double _alpha, double _h)
        {
            h = _h;
            alpha = _alpha;
        }

        //Метод Рунге-Кутты 2-го порядка
        public Point runge_kutta_2(Point p)
        {
            double new_x = p.X + h * function_x(p);
            double new_y = p.Y + h * function_y(p);

            double x = p.X + h * (function_x(p) + function_x(new Point(new_x, p.Y))) / 2;
            double y = p.Y + h * (function_y(p) + function_y(new Point(p.X, new_y))) / 2;

            return new Point(SetSigFig(x, accuracy), SetSigFig(y, accuracy));
        }

        //Функция для x'
        private double function_x(Point p)
        {
            return SetSigFig(p.Y, accuracy);
        }
        //Функция для y'
        private double function_y(Point p)
        {
            return SetSigFig(-alpha * (1 - p.X * p.X) * p.Y - p.X, accuracy);
        }
    }
}
