using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp4
{
    class MyViewModel
    {
        public ObservableDataSource<Point> Data_Xdt_X { get; set; }
        public ObservableDataSource<Point> Data_Xdt_Ydt { get; set; }
        public ObservableDataSource<Point> Data_X_t { get; set; }
        public ObservableDataSource<Point> Data_Xdt_t { get; set; }

        public MyViewModel()
        {
            Data_Xdt_X = new ObservableDataSource<Point>();
            Data_Xdt_Ydt = new ObservableDataSource<Point>();
            Data_X_t = new ObservableDataSource<Point>();
            Data_Xdt_t = new ObservableDataSource<Point>();
        }
    }
}
