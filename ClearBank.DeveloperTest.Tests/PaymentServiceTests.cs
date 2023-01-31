using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using NUnit.Framework;
using System;
using System.Configuration;

namespace ClearBank.DeveloperTest.Tests
{
    [TestFixture]
    public class PaymentServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings.Set("DataStoreType", "Backup");
        }

        [Test]
        public void MakePayment_Should_Make_Payment_Successfully()
        {
            // arrange
            var request = new MakePaymentRequest()
            {
                Amount = 1,
                CreditorAccountNumber = "123123",
                DebtorAccountNumber = "1234",
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var accountDataStore = new Mock<IAccountDataStore>();
            accountDataStore.Setup(x => x.GetAccount(request.DebtorAccountNumber)).Returns(new Account()
            {
                AccountNumber = request.DebtorAccountNumber,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 100,
                Status = AccountStatus.Live
            });

            var paymentService = new PaymentService(accountDataStore.Object);  

            // act
            var result = paymentService.MakePayment(request);

            // assert
            var expected = true;
            Assert.AreEqual(result.Success, expected);
        }

        [Test]
        public void MakePayment_Should_Make_Payment_Successfully_0_Remaining_Balance()
        {
            // arrange
            var request = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = "123123",
                DebtorAccountNumber = "1234",
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var accountDataStore = new Mock<IAccountDataStore>();
            accountDataStore.Setup(x => x.GetAccount(request.DebtorAccountNumber)).Returns(new Account()
            {
                AccountNumber = request.DebtorAccountNumber,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 100,
                Status = AccountStatus.Live
            });

            var paymentService = new PaymentService(accountDataStore.Object);

            // act
            var result = paymentService.MakePayment(request);

            // assert
            var expected = true;
            Assert.AreEqual(result.Success, expected);
        }

        [Test]
        public void MakePayment_Should_Not_Make_Payment_Successfully_Not_Enough_Balance()
        {
            // arrange
            var request = new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = "123123",
                DebtorAccountNumber = "1234",
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var accountDataStore = new Mock<IAccountDataStore>();
            accountDataStore.Setup(x => x.GetAccount(request.DebtorAccountNumber)).Returns(new Account()
            {
                AccountNumber = request.DebtorAccountNumber,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 50,
                Status = AccountStatus.Live
            });

            var paymentService = new PaymentService(accountDataStore.Object);

            // act
            var result = paymentService.MakePayment(request);

            // assert
            var expected = false;
            Assert.AreEqual(result.Success, expected);
        }

        [Test]
        public void MakePayment_Should_Not_Make_Payment_Successfully_Account_Disabled()
        {
            // arrange
            var request = new MakePaymentRequest()
            {
                Amount = 1,
                CreditorAccountNumber = "123123",
                DebtorAccountNumber = "1234",
                PaymentDate = DateTime.Now,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var accountDataStore = new Mock<IAccountDataStore>();
            accountDataStore.Setup(x => x.GetAccount(request.DebtorAccountNumber)).Returns(new Account()
            {
                AccountNumber = request.DebtorAccountNumber,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = 100,
                Status = AccountStatus.Disabled
            });

            var paymentService = new PaymentService(accountDataStore.Object);

            // act
            var result = paymentService.MakePayment(request);

            // assert
            var expected = false;
            Assert.AreEqual(result.Success, expected);
        }
    }
}
