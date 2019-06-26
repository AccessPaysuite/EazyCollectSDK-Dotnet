﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class StartDateCheck
    {
        private IConfiguration Settings { get; set; }
        private EazySDK.Utilities.ContractPostChecks ContractCheck { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            ClientHandler Handler = new ClientHandler();
            Settings = Handler.Settings();
            ContractCheck = new EazySDK.Utilities.ContractPostChecks();
            Settings.GetSection("currentEnvironment")["Environment"] = "sandbox";
        }

        [TestMethod]
        public void TestValidStartDate()
        {
            Settings.GetSection("directDebitProcessingDays")["InitialProcessingDays"] = "10";
            string Schedule = ContractCheck.CheckStartDateIsValid("2019-07-15", Settings);
            Assert.IsTrue(Schedule == "2019-07-15");
        }

        [TestMethod]
        public void TestValidStartDateAfterBankHoliday()
        {
            Settings.GetSection("directDebitProcessingDays")["InitialProcessingDays"] = "43";
            string Schedule = ContractCheck.CheckStartDateIsValid("2019-08-27", Settings);
            Assert.IsTrue(Schedule == "2019-08-27");
        }

        [TestMethod]
        public void TestStartDateAcceptsNonISODates()
        {
            Settings.GetSection("directDebitProcessingDays")["InitialProcessingDays"] = "10";
            var Scheudle = ContractCheck.CheckStartDateIsValid("15/07/2019", Settings);
            Assert.IsTrue(Scheudle == "2019-07-15");
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidStartDateException))]
        public void TestStartDateInvalidDateThrowsError()
        {
            ContractCheck.CheckStartDateIsValid("2019-13-13", Settings);
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.InvalidSettingsConfigurationException))]
        public void TestStartDateInvalidInitialProcessingDaysThrowsError()
        {
            Settings.GetSection("directDebitProcessingDays")["InitialProcessingDays"] = "ten";
            ContractCheck.CheckStartDateIsValid("2019-07-15", Settings);
        }
    }
}