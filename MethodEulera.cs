using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp4
{
    class MethodEulera : SetSigFigs
    {
        public double alpha;
        public double tau;

        private int accuracy = 3;

        public MethodEulera(double _alpha, double _tau)
        {
            alpha = _alpha;
            tau = _tau;
        }

        //Метод Эйлера
        public Point methodEulera(Point p)
        {
            Point new_point = new Point(SetSigFig(p.X + tau * p.Y, accuracy), SetSigFig(p.Y + tau * (-alpha * (1 - p.X * p.X) * p.Y - p.X), accuracy));

            return new_point;
        }
    }
}
