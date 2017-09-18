using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanRepayment
{
    //Extension method for doulbe variables
    public static class DoubleExtension
    {
        //computes the double to round up value to specific precision length
        public static double Precision(this double source, int length_of_precision)
        {
            return Math.Round(source, length_of_precision);
        }
    }
}
