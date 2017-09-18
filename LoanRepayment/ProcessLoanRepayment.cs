using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace LoanRepayment
{
    public class ProcessLoanRepayment
    {
        private string fileName = "";
        private double LoanAmount = 0.0;

        private static int Num_Of_Payments = 36;//Number of Payments is set as 36

        public ProcessLoanRepayment(string FName, double Amount)
        {
            fileName = FName;
            LoanAmount = Amount;

            LoanViable loanIsViable = CheckIfLoanAmountViable(LoanAmount);

            switch (loanIsViable)
            {
                case LoanViable.NA:
                    Console.WriteLine("");
                    Console.WriteLine("Loan amount of £" + LoanAmount + " can not be processed at this moment");
                    break;

                case LoanViable.AmountIsViable:
                    CalculateLoanRePayment(fileName, LoanAmount);
                    break;

                case LoanViable.AmountNotSupported:
                    Console.WriteLine("");
                    Console.WriteLine("We apologise!. Sorry, it is not possible to provide a quote at this time. The amount is not supported.");
                    Console.WriteLine("Please enter the loan amount between £1000 and £15000 inclusive.");
                    break;

                case LoanViable.AmountNotIncrementOf100:
                    Console.WriteLine("");
                    Console.WriteLine("We apologise!. Sorry, it is not possible to provide a quote at this time. The amount is not supported.");
                    Console.WriteLine("Please enter the loan amount of any £100 increment between £1000 and £15000 inclusive.");
                    break;
            }

        }

        private void CalculateLoanRePayment(string fileName, double LoanAmount)
        {
            //check if the specified file (market_file.csv) exists or not
            if(!File.Exists(fileName))
            {
                Console.WriteLine("");
                Console.WriteLine("Error!");
                Console.WriteLine("The specified file: "+fileName+" does not exist! Please check the name/path again!");
                return;
            }

            //read the file (market_file.csv)
            #region Processing .CSV: To read in all the offers from lender from the market_file
            
            string JTLine1 = "";
            string[] items = null;
            List<MarketOffers> marketOffersList = new List<MarketOffers>();

            try
            {
                using (StreamReader JTsr = new StreamReader(fileName))
                {

                    while ((JTLine1 = JTsr.ReadLine()) != null)
                    {
                        bool ReadingError = false;

                        JTLine1 = JTLine1.Trim().ToUpper();

                        items = JTLine1.Split(',');

                        double tempRate = 0.0;
                        int tempAvailable = 0;

                        if (!double.TryParse(items[1].ToString().Trim(), out tempRate)) //second column of the market_file holds the rate
                            ReadingError = true;

                        if (!int.TryParse(items[2].ToString().Trim(), out tempAvailable)) //third column of the market_file holds the Available loan amount from the lender
                            ReadingError = true;

                        if (!ReadingError)//if no error in reading the rate or available amount from the market_file then use the information for loan calculation
                        {
                            var eachAvailableOffer = new MarketOffers
                            {
                                Lender = items[0].ToString().Trim(), //first column being the name of the Lender
                                Rate = tempRate,
                                AvailableAmount = tempAvailable
                            };

                            marketOffersList.Add(eachAvailableOffer);//add each offer from the market_file to a list used by the proragm later
                        }

                    }

                    JTsr.Close();

                }

            }
            catch (Exception ex)
            {

                if (!EventLog.SourceExists("LoanRePayment"))
                    EventLog.CreateEventSource("LoanRePayment", "Application");

                EventLog.WriteEntry("LoanRePayment", ex.Message.ToString());

                Console.WriteLine("");
                Console.WriteLine("Error!");
                Console.WriteLine("There was some error in the market file! We apologise for not being able to provide an offer at the moment.");
                return;
            }
            #endregion

            #region Process all the lender's offer to find the most suitable offer
            //check if the market_file processed contains at least one or more offer from lenders
            if(marketOffersList.Count > 0)
            {
                //create a list of market_offers that can be provided to the customer
                List<MarketOffers> viableMarketOfferList = new List<MarketOffers>();

                foreach(MarketOffers thisMarketOffer in marketOffersList)
                {
                    if(thisMarketOffer.AvailableAmount >= LoanAmount)
                    {
                        viableMarketOfferList.Add(thisMarketOffer);
                    }
                }

                //check if any lenders are able to make the loan offer
                if(viableMarketOfferList.Count > 0)
                {
                    List<LoanOffers> loanOffersList = new List<LoanOffers>();
                    //create offers from each viable lenders
                    foreach(MarketOffers thisMarketOffer in viableMarketOfferList){

                        var thisRate = thisMarketOffer.Rate.Precision(1) / ( (int)Dividend.Monthly * 100);
                        InterestCalculator IC = new InterestCalculator((int)LoanAmount, thisRate, Num_Of_Payments);
                        var monthlyRepayment = IC.CalculatedRepayment;
                        var totalRepayment = (double)monthlyRepayment * Num_Of_Payments;

                        if (monthlyRepayment >= 0) //add the offer only if monthly repayment is valid and greater than £0
                        {
                            var thisLoanOffer = new LoanOffers
                            {
                                Lender = thisMarketOffer.Lender,
                                Rate = thisMarketOffer.Rate.Precision(1),
                                MonthlyRepayment = monthlyRepayment,
                                TotalRepayment = totalRepayment.Precision(2)
                            };

                            loanOffersList.Add(thisLoanOffer);
                        }
                    }

                    //parse all the viable offers to get the best quote for the customer
                    if(loanOffersList.Count > 0)
                    {
                        //sort the offers from low to high
                        //here stable quicksort is used with complexity of Θ(n log(n)) average and O(n^2) in worst case scenario
                        if (loanOffersList.Count > 1)
                            loanOffersList.Sort((x, y) => x.MonthlyRepayment.CompareTo(y.MonthlyRepayment));

                        //return the first result to the Customer because the list is sorted in ascending order
                        Console.WriteLine("Requested amount: £" + LoanAmount);
                        Console.WriteLine("Rate: " + loanOffersList[0].Rate.Precision(1).ToString("0.0") + "%");
                        Console.WriteLine("Monthly repayment: £" + loanOffersList[0].MonthlyRepayment.ToString("0.00"));
                        Console.WriteLine("Total repayment: £" + loanOffersList[0].TotalRepayment.ToString("0.00"));
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("We apologise! It is not possible for us to provide a quote at this moment.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("We apologise! It is not possible for us to provide a quote at this moment.");
                return;
            }
            #endregion

        }

        //method to check if the loan amount is between £1000 and £15000
        private LoanViable CheckIfLoanAmountViable(double Amount)
        {
            if(Amount < 1000|| Amount > 15000)
            {
                return LoanViable.AmountNotSupported;
            }

            if(Amount >= 1000 && Amount <= 15000)
            {
                if (Amount % 100 != 0)
                    return LoanViable.AmountNotIncrementOf100;
                else
                    return LoanViable.AmountIsViable;
            }

            return LoanViable.NA;
        }
    }

    //Enumerator to hold variables for Loan Amounts to determin whether they can be processed or not
    public enum LoanViable { AmountIsViable, AmountNotSupported, AmountNotIncrementOf100, NA}

    //Enumerator to hold values for the repayment's type
    public enum Dividend { Monthly = 12, Bi_Weekly = 26, Quarterly = 4, Yearly = 1}

}
