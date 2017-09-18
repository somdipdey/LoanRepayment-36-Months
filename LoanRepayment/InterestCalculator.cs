using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanRepayment
{
    public class InterestCalculator
    {
        private int amount { get; set; }
        private double interestRate { get; set; }
        private int time { get; set; }

        public double CalculatedRepayment { get; set; }

        public InterestCalculator(int thisAmount, double thisInterestRate, int thisTime)
        {
            amount = thisAmount;
            interestRate = thisInterestRate;
            time = thisTime;

            CalculatedRepayment = Calculate(amount, interestRate, time);
        }

        private double Calculate(int amount, double interestRate, int time)
        {
            //if the interest is equal or less than zero then process it accordingly
            if (interestRate <= 0)
            {
                if (time <= 0)
                {
                    return amount;
                }
                else
                {
                    return amount / time;
                }
            }

            //calculate the loan repayment 
            var loan = Math.Pow(1 + interestRate, time);
            return (amount / ((1 - (1 / loan)) / interestRate));
        }
    }
}
