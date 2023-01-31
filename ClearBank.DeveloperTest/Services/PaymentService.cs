using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore AccountDataStore;

        public PaymentService(IAccountDataStore accountDataStore)
        {
            AccountDataStore = accountDataStore;
        }


        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var result = new MakePaymentResult();
            var account = AccountDataStore.GetAccount(request.DebtorAccountNumber);

            if (account == null || account.Status == AccountStatus.Disabled ||
                (account.Status == AccountStatus.InboundPaymentsOnly && request.Amount > 0))
            {
                result.Success = false;

                return result;
            }

            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    result.Success = PaymentProcessor.BacsPayment(account);
                    break;

                case PaymentScheme.FasterPayments:
                    result.Success = PaymentProcessor.FasterPayment(account, request.Amount);
                    break;

                case PaymentScheme.Chaps:
                    result.Success = PaymentProcessor.ChapsPayment(account);
                    break;
            }

            if (result.Success)
                UpdateBalance(request, account);

            return result;
        }

        private void UpdateBalance(MakePaymentRequest request, Account account)
        {
            account.Balance -= request.Amount;

            AccountDataStore.UpdateAccount(account);
        }
    }
}
