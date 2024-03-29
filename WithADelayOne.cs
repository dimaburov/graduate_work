﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp4
{
    class WithADelayOne : SetSigFigs
    {
        private List<double> array_data;
        private double r;
        private double tue;
        private double a;

        private int accuracy = 15;

        public WithADelayOne(List<double> _array_data, double _r, double _tue, double _a)
        {
            r = _r;
            array_data = _array_data;
            tue = _tue;
            a = _a;
        }

        //Тело метода по вычислению новой точки
        public Point WithADelay(Point p, int k)
        {
            Point result = new Point(1, 1);
            //result = new Point(SetSigFig(p.Y, accuracy), SetSigFig(p.Y * Math.Exp(r * tue * f(array_data[k])), accuracy));
            result = new Point(Math.Round(p.Y, accuracy), Math.Round(p.Y * Math.Exp(r * tue * f(array_data[k])), accuracy));
            Console.WriteLine("result With A delay " + result);
            return result;
        }
        //Функция f(x)
        private double f(double x)
        { 
            return Math.Round((x-1)/(-1 * Math.Pow(a, -1) * x - 1), accuracy);
        }
    }
}

