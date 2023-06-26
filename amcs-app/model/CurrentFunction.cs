using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amcs_app.model
{
    public class CurrentFunction
    {
        private double _l;
        private double _dzeta;

        public CurrentFunction(double l, double dzeta)
        {
            _l = l;
            _dzeta = dzeta;
        }

        public double P4(double t) => Math.Exp(-_l * t) + Math.Exp(-_dzeta * t) - Math.Exp(-(_l + _dzeta) * t);

        public double F(int i, int j, double x, double[,] y)
        {
            if (j == 1)
                return -(_l + _dzeta) * y[i, j];
            else if (j == 2)
                return _dzeta * y[i, j - 1] - _l * y[i, j];
            else if (j == 3)
                return _l * y[i, j - 2] - _dzeta * y[i, j];

            return _l * y[i, j - 2] + _dzeta * y[i, j - 1];
        }
    }
}
