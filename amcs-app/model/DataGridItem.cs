using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amcs_app.model
{
    public class DataGridItem
    {
        public int Id { get; private set; }
        public double NumSolution { get; private set; }
        public double AnalyticalSolution { get; private set; }

        public DataGridItem(int id, double numSol, double analytSol)
        {
            Id = id;
            NumSolution = numSol;
            AnalyticalSolution = analytSol;
        }
    }
}
