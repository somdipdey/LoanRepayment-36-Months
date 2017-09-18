using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanRepayment
{
    /*String Entension class to provide more computational capacity to string variables*/
    public static class StringExtension
    {

        //computes the last number of characters 
        public static string Last(this string source, int length_of_tail)
        {
            if (length_of_tail >= source.Length)
                return source;
            return source.Substring(source.Length - length_of_tail);
        }
    }
}
