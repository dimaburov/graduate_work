using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp4
{
    class WithADelayMethod : SetSigFigs
    {
        private List<double> array_data;
        private double r;
        private double tue;

        private int accuracy = 5;

        public WithADelayMethod(List<double> _array_data, double _r, double _tue)
        {
            r = _r;
            array_data = _array_data;
            tue = _tue;
        }

        //Тело метода по вычислению новой точки
        public Point WithADelay(Point p,int k)
        {
            Console.WriteLine("k = " + k + " value data["+ array_data[k]+"] xn = "+ SetSigFig(p.Y, accuracy));
            Console.WriteLine("P.Y NEW до умножения" + SetSigFig((1 + tue * r * (1 - array_data[k])), accuracy));
            //return new Point(SetSigFig(p.Y, accuracy), SetSigFig((1 + tue * r) * p.Y * (1 - (tue * r) / (1 + tue * r) * array_data[k]), accuracy));
            return new Point(SetSigFig(p.Y, accuracy), SetSigFig(p.Y * (1 + tue * r * (1 - array_data[k])), accuracy));
            //return new Point(Math.Round(p.Y, accuracy), Math.Round(p.Y * (1 + tue * r * (1 - array_data[k])), accuracy));
        }
    }
}
