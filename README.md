# LoanRepayment-36-Months
There is a need for a rate calculation system allowing prospective borrowers to obtain a quote from our pool of lenders for 36 month loans. This system will  take the form of a command-line application and provide the best offer to the customer based on the lender's data provided in a market_data_file.csv.


# Technical Test

There is a need for a rate calculation system allowing prospective borrowers to
obtain a quote from our pool of lenders for 36 month loans. This system will 
take the form of a command-line application.

You will be provided with a file containing a list of all the offers being made
by the lenders within the system in CSV format, see the example market.csv file
provided alongside this specification.

You should strive to provide as low a rate to the borrower as is possible to
ensure that Zopa's quotes are as competitive as they can be against our
competitors'. You should also provide the borrower with the details of the
monthly repayment amount and the total repayment amount.

Repayment amounts should be displayed to 2 decimal places and the rate of the 
loan should be displayed to one decimal place.

Borrowers should be able to request a loan of any £100 increment between £1000
and £15000 inclusive. If the market does not have sufficient offers from
lenders to satisfy the loan then the system should inform the borrower that it
is not possible to provide a quote at that time.

The application should take arguments in the form:

	cmd> [application] [market_file] [loan_amount]

Example:

	cmd> quote.exe market.csv 1500

The application should produce output in the form:

	cmd> [application] [market_file] [loan_amount]
	Requested amount: £XXXX
	Rate: X.X%
	Monthly repayment: £XXXX.XX
	Total repayment: £XXXX.XX

Example:

	cmd> quote.exe market.csv 1000
	Requested amount: £1000
	Rate: 7.0%
	Monthly repayment: £30.78
	Total repayment: £1108.10

## Remarks
 
 * The monthly and total repayment should use monthly compounding interest



## Solution

Here, in this program, LoanRepayment.exe, the formula used is as follows:

Let's assume, ln = (1+(APR/(12*100)))^36

The monthly repayment amount = (amount / ((1 - (1 / ln)) / (APR/(12*100))))

This is the same formula used by Loan repayment calculator by theguardian.com. Link: https://www.theguardian.com/money/loan-repayment-calculator-interest-rates

Assumptions:
3rd Column in the market_data_file determins the maximum amount of loan which can be lend by the lender (specified in the 1st column of the same file).
2nd Column holds the APR% (annual percentage rate of charge (APR)) value, which is charged by the lender.
The repayment period is always of 36 months.
