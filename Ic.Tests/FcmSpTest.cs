using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;

namespace DirectSpMessageHelper.Test
{
    [TestClass]
    public class FcmSpTest
    {
        Lazy<string> _serverKey = new Lazy<string>(() => ConfigurationManager.AppSettings["ServerKey"]);
        Lazy<string> _senderId = new Lazy<string>(() => ConfigurationManager.AppSettings["SenderId"]);
        Lazy<string> _deviceId = new Lazy<string>(() => ConfigurationManager.AppSettings["DeviceId"]);

        [TestMethod]
        public void FcmSendTest()
        {
            var message = $"[{{'MessageBody':'hi hi hi','CustomData':'','Address':'{_deviceId.Value}'}}]".Replace("'", "\"");
            var fcmOptions = $"[{{ 'TestMode':false,'MaxConnectionCount':1000,'ServerKey':'{_serverKey.Value}','SenderId':'{_senderId.Value}'}}]".Replace("'", "\"");
            string result = "";
            StoredProcedures.FcmSend(message, fcmOptions, result: ref result);

            var sendResult = JArray.Parse(result);

            Assert.IsTrue(Convert.ToInt16(sendResult[0]["Status"]) == 1);
        }
    }
}
