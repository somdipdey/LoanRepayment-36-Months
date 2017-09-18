using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanRepayment
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                /* Check if any parameters are passed in the command line.
                 * If no paramters are passed/mentioned 
                 * then throw an exception warning to the user.
                 */
                Console.WriteLine("");
                Console.WriteLine("Error!");
                Console.WriteLine("Please provide paramters such as [MarketFileName] & [Amount] to continue....");
                Console.WriteLine("Example:");
                Console.WriteLine("> LoanRepayment.exe [market_file.csv] [loan_amount]");
                Console.WriteLine("");
                return;
            }

            if (args.Length > 0)
            {
                /* Check if more than two arguments are assed in the command line.
                 * If Yes, then throw an exception and warn the user.
                 * If No, then let the program continue.
                 */
                if(args.Length > 2)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Error!");
                    Console.WriteLine("Only two parameters can be entered....");
                    Console.WriteLine("Please provide paramters such as [MarketFileName] & [Amount] to continue....");
                    Console.WriteLine("Example:");
                    Console.WriteLine("> LoanRepayment.exe [market_file.csv] [loan_amount]");
                    Console.WriteLine("");
                    return;
                }

                //The program continues from here if the parameters are correct -->

                //check if the first parameter, which is [market_file] has .csv extension
                if(args[0].ToString().Last(4) != ".csv")
                {
                    Console.WriteLine("");
                    Console.WriteLine("Error!");
                    Console.WriteLine("The file is not in .csv format. Pease specify the correct extension.");
                    Console.WriteLine("");
                    return;
                }

                var fileName = args[0].ToString();
                double amount = 0.0;
                bool isNumeric = double.TryParse(args[1].ToString(), out amount);

                //check if the second parameter passed is all numeric or not
                if (!isNumeric)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Error!");
                    Console.WriteLine("The specified amount has to be numeric only!");
                    return;
                }

                ProcessLoanRepayment processRepayment = new ProcessLoanRepayment(fileName, amount);

            }
        }
    }
}
