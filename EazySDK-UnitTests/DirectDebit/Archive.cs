﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using EazySDK;
using Microsoft.Extensions.Configuration;

namespace EazySDK_UnitTests
{
    [TestClass]
    public class UnitTestArchive
    {
        private IConfiguration Settings { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            ClientHandler Handler = new ClientHandler();
            Settings = Handler.Settings();
            Settings.GetSection("currentEnvironment")["Environment"] = "sandbox";
            Settings.GetSection("sandboxClientDetails")["ClientCode"] = "SDKTST";
            Settings.GetSection("sandboxClientDetails")["ApiKey"] = "hkZujzFR2907XAtYe6qkKRsBo";
        }

        [TestMethod]
        public void TestArchiveContractAchiveValidContract()
        {
            var Post = new Post(Settings);
            var Req = Post.ArchiveContract("a899ced6-a601-4146-92f7-5c8ee40bbf93");
            Assert.IsTrue(Req.Contains("has been archived"));
        }

        [TestMethod]
        [ExpectedException(typeof(EazySDK.Exceptions.ResourceNotFoundException))]
        public void TestRestartContractInvalidContractThrowException()
        {
            var Post = new Post(Settings);
            Post.ArchiveContract("a899ced6-92f7-5c8ee40bbf93"); ;
        }
    }
}

