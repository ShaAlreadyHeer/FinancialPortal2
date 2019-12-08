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
        withdrawal,
        Deposit
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