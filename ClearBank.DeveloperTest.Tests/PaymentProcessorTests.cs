using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests
{
    [TestFixture]
    public class PaymentProcessorTests
    {
        [Test]
        public void PaymentProcessor_BacsPaymentSuccess()
        {
            // arrange
            var account = new Account()
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };

            // act
            var result = PaymentProcessor.BacsPayment(account);

            // assert
            var expected = true;
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void PaymentProcessor_Allowed_Scheme_Faster_Payment_Bacs_PaymentType()
        {
            // arrange
            var account = new Account()
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
            };

            // act
            var expected = false;
            var result = PaymentProcessor.BacsPayment(account);

            // assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void PaymentProcessor_FasterPayment_With_Valid_Balance_Success()
        {
            // arrange
            var account = new Account()
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 100
            };

            // act
            var result = PaymentProcessor.FasterPayment(account, 1);

            // assert
            var expected = true;
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void PaymentProcessor_FasterPayment_With_Invalid_Balance_Success()
        {
            // arrange
            var account = new Account()
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 1
            };

            // act
            var result = PaymentProcessor.FasterPayment(account, 100);

            // assert
            var expected = false;
            Assert.AreEqual(expected, result);
        }
    }
}
