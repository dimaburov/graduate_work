using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp4
{
    class RungeKutta4 : SetSigFigs
    {
        private double step;
        private double alpha;

        private int accuracy = 3;

        public RungeKutta4(double _alpha, double _step)
        {
            step = _step;
            alpha = _alpha;
        }

        public double[] runge_kutta(Point start_p)
        {
            double x1 = SetSigFig(step * function_x(start_p), accuracy);
            double y1 = SetSigFig(step * function_y(start_p), accuracy);

            double x2 = SetSigFig(step * function_x(new Point(start_p.X + x1 / 2.0, start_p.Y + y1 / 2.0)), accuracy);
            double y2 = SetSigFig(step * function_y(new Point(start_p.X + x1 / 2.0, start_p.Y + y1 / 2.0)), accuracy);

            double x3 = SetSigFig(step * function_x(new Point(start_p.X + x2 / 2.0, start_p.Y + y2 / 2.0)), accuracy);
            double y3 = SetSigFig(step * function_y(new Point(start_p.X + x2 / 2.0, start_p.Y + y2 / 2.0)), accuracy);

            double x4 = SetSigFig(step * function_x(new Point(start_p.X + x3, start_p.Y + y3)), accuracy);
            double y4 = SetSigFig(step * function_y(new Point(start_p.X + x3, start_p.Y + y3)), accuracy);

            Console.WriteLine("Шаг x " + step + " k1 " + x1 + " k2 " + x2 + " k3 " + x3 + " k4 " + x4);
            Console.WriteLine("Шаг y " + step + " k1 " + y1 + " k2 " + y2 + " k3 " + y3 + " k4 " + y4);

            double[] rezult = new double[2];


            if (x4 > 50000) return rezult;
            if (y4 > 50000) return rezult;
            rezult[0] = SetSigFig(start_p.X + (x1 + 2.0 * x2 + 2.0 * x3 + x4) / 6.0, accuracy);
            rezult[1] = SetSigFig(start_p.Y + (y1 + 2.0 * y2 + 2.0 * y3 + y4) / 6.0, accuracy);

            return rezult;
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
