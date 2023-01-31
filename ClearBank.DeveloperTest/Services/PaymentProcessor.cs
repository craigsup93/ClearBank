using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public static class PaymentProcessor
    {
        public static bool BacsPayment(Account account)
        {
            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
            {
                return false;
            }

            return true;
        }

        public static bool FasterPayment(Account account, decimal amount)
        {
            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
            {
                return false;
            }
            else if (account.Balance < amount)
            {
                return false;
            }

            return true;
        }

        public static bool ChapsPayment(Account account)
        {
            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
            {
                return false;
            }
            else if (account.Status != AccountStatus.Live)
            {
                return false;
            }

            return true;
        }
    }
}
