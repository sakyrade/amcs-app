using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amcs_app
{
    public delegate double FuncHandler(int i, int j, double x, double[,] y);
    public class RungeKuttaAlgorithm
    {
        private int _m = 0;
        private int _n = 0;
        private FuncHandler _funcHandler;

        public double Dx { get; private set; }
        public double[,] Y { get; private set; }
        public double X0 { get; private set; }
        public double Xf { get; private set; }
        public int NumPoints { get; private set; }

        public RungeKuttaAlgorithm(double x0, double xf, double[,] y, int m, int n, int numPoints, FuncHandler funcHandler)
        {
            _funcHandler = funcHandler;

            X0 = x0;
            Xf = xf;

            Y = y;

            Dx = xf / numPoints;
            NumPoints = numPoints;

            _m = m;
            _n = n;
        }

        public void SolveSystemRK4()
        {
            double h = (Xf - X0) / _m;

            for (int i = 1; i < _m; i++)
            {
                for (int j = 1; j < _n; j++)
                    Y[i, j] = Y[i - 1, j];

                double x = X0 + i * h;

                double[,] yt = new double[_m, _n];

                double[] k1 = new double[_n];
                double[] k2 = new double[_n];
                double[] k3 = new double[_n];
                double[] k4 = new double[_n];

                for (int k = 1; k < _n; k++)
                    k1[k] = h * _funcHandler(i, k, x, Y);

                for (int k = 1; k < _n; k++)
                    yt[i, k] = Y[i, k] + 0.5 * k1[k];

                for (int k = 1; k < _n; k++)
                    k2[k] = h * _funcHandler(i, k, x + h * 0.5, yt);

                for (int k = 1; k < _n; k++)
                    yt[i, k] = Y[i, k] + 0.5 * k2[k];

                for (int k = 1; k < _n; k++)
                    k3[k] = h * _funcHandler(i, k, x + h * 0.5, yt);

                for (int k = 1; k < _n; k++)
                    yt[i, k] = Y[i, k] + k3[k];

                for (int k = 1; k < _n; k++)
                    k4[k] = h * _funcHandler(i, k, x + h, yt);

                for (int k = 1; k < _n; k++)
                    Y[i, k] = Y[i, k] + (k1[k] + 2 * k2[k] + 2 * k3[k] + k4[k]) / 6;
            }
        }
    }
}
