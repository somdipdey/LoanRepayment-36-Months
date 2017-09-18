using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoanRepayment
{
    public class LoanOffers
    {
        public string Lender { get; set; }
        public double Rate { get; set; }
        public double MonthlyRepayment { get; set; }
        public double TotalRepayment { get; set; }
    }
}
