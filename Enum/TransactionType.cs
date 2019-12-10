using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Enum
{
    public class TransactionType
    {
        public enum TransType
        {
        Withdrawal,
        Deposit,
        Payment,
        }
    }

    public class AccountType
    {
        public enum AccType
        {
            Checking,
            Investment,
            Saving,
            Credit
        }
    }

}