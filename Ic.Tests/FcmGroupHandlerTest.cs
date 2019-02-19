using DirectSpMessageHelper.Fcm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

namespace DirectSpMessageHelper.Tests
{
    [TestClass]
    public class FcmGroupHandlerTest
    {
        Lazy<string> _serverKey = new Lazy<string>(() => ConfigurationManager.AppSettings["ServerKey"]);
        Lazy<string> _senderId = new Lazy<string>(() => ConfigurationManager.AppSettings["SenderId"]);
        Lazy<string> _deviceId = new Lazy<string>(() => ConfigurationManager.AppSettings["DeviceId"]);

        [TestMethod]
        public void AddTokenToGroup()
        {
            string groupName = $"Group-{Guid.NewGuid().ToString()}";

            FcmGroupHandler fcmGroupHandler = new FcmGroupHandler(_serverKey.Value, _senderId.Value);

            string groupKey = fcmGroupHandler.AddTokenToGroup(groupName, new string[] { _deviceId.Value }).Result;

            // AddTokenToGroup should create new group and return group key
            Assert.AreNotEqual(groupKey, null);

            groupKey = fcmGroupHandler.AddTokenToGroup(groupName, new string[] { _deviceId.Value }).Result;

            // AddTokenToGroup should add token to created group in pervious step
            Assert.AreEqual(groupKey, null);
        }
    }
}
