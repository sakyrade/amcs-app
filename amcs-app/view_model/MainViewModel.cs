using amcs_app.model;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Markup;

namespace amcs_app.view_model
{
    public class MainViewModel : ObservableObject, IDataErrorInfo
    {
        private RelayCommand _solutionCommand;
        private double _intensitySC;
        private double _intensityHuman;
        private double _finalValue;
        private int _numPoints;
        private double _step;
        private double _time;
        private Chart _chart;

        public double IntensitySC
        {
            get { return _intensitySC; }
            set
            {
                _intensitySC = value;
                OnPropertyChanged();
            }
        }

        public double IntensityHuman
        {
            get { return _intensityHuman; }
            set
            {
                _intensityHuman = value;
                OnPropertyChanged();
            }
        }

        public double FinalValue
        {
            get { return _finalValue; }
            set
            {
                _finalValue = value;
                OnPropertyChanged();
            }
        }

        public int NumPoints
        {
            get { return _numPoints; }
            set
            {
                _numPoints = value;
                OnPropertyChanged();
            }
        }

        public double Step
        {
            get { return _step; }
            set
            {
                _step = value;
                OnPropertyChanged();
            }
        }

        public double Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DataGridItem> Data { get; set; }

        public double[] AxisX { get; private set; }

        public double[] AxisY1 { get; private set; }

        public double[] AxisY2 { get; private set; }

        public RelayCommand SolutionCommand
        {
            get
            {
                return _solutionCommand ??
                    (new RelayCommand(o =>
                    {
                        Data.Clear();

                        int m = NumPoints;
                        int n = 4;

                        double[,] y0 = new double[m + 1, n + 1];

                        for (int i = 0; i < n + 1; i++)
                            y0[0, i] = 0;
                        y0[0, 1] = 1;

                        CurrentFunction cf = new CurrentFunction(IntensitySC, IntensityHuman);
                        RungeKuttaAlgorithm rka = new RungeKuttaAlgorithm(0, FinalValue, y0, m + 1, n + 1, m, cf.F);

                        rka.SolveSystemRK4();

                        Step = rka.Dx;

                        Time = 1 / IntensitySC + 1 / IntensityHuman - 1 / (IntensitySC + IntensityHuman);

                        double x = 0;
                        int j = 0;

                        AxisX = new double[m + 1];
                        AxisY1 = new double[m + 1];
                        AxisY2 = new double[m + 1];

                        do
                        {
                            double p = 0;

                            for (int k = 1; k < n; k++)
                                p += rka.Y[j, k];

                            AxisX[j] = x;
                            AxisY1[j] = p;
                            AxisY2[j] = cf.P4(x);

                            Data.Add(new DataGridItem(j + 1, AxisY1[j], AxisY2[j]));

                            j++;
                            x += rka.Dx;
                        }
                        while (x < FinalValue);

                        _chart.Series["Series1"].ChartArea = "Default";
                        _chart.Series["Series1"].ChartType = SeriesChartType.Line;
                        _chart.Series["Series1"].Points.DataBindXY(AxisX, AxisY1);

                        _chart.Series["Series2"].ChartArea = "Default";
                        _chart.Series["Series2"].ChartType = SeriesChartType.Line;
                        _chart.Series["Series2"].Points.DataBindXY(AxisX, AxisY2);
                    }));
            }
        }

        public string Error { get { return null; } }

        public string this[string name]
        {
            get
            {
                string err = null;

                switch (name)
                {
                    case "IntensitySC":
                        {
                            if (IntensitySC <= 0)
                                err = "Интенсивность СК не может быть меньше либо равна нулю.";
                        }
                        break;
                    case "IntensityHuman":
                        {
                            if (IntensityHuman <= 0)
                                err = "Интенсивность человека не может быть меньше либо равна нулю.";
                        }
                        break;
                    case "FinalValue":
                        {
                            if (FinalValue < 1)
                                err = "Конечное значение t не может быть меньше единицы.";
                        }
                        break;
                    case "NumPoints":
                        {
                            if (NumPoints < 1)
                                err = "Количество точек не может быть меньше единицы.";
                        }
                        break;
                }

                return err;
            }
        }

        public MainViewModel(Chart chart)
        {
            Data = new ObservableCollection<DataGridItem>();
            _chart = chart;

            _chart.ChartAreas.Add(new ChartArea("Default"));
            _chart.Series.Add(new Series("Series1"));
            _chart.Series.Add(new Series("Series2"));
        }
    }
}
