using MessageHelper;
using MessageHelper.Fcm;
using MessageHelper.Fcm.Model;
using MessageHelper.Fcm.Signature;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

public partial class StoredProcedures
{

    [Microsoft.SqlServer.Server.SqlProcedure(Name = "Fcm_Send")]
    public static void FcmSend(string messages, string fcmOptions, ref string result)
    {
        // Deserialize options to FcmOptions and Messages
        var options = JsonConvert.DeserializeObject<List<FcmOptions>>(fcmOptions).FirstOrDefault();
        var fcmMessages = JsonConvert.DeserializeObject<List<Message>>(messages);

        // Configuration connections count to fcm server
        ServicePointManager.DefaultConnectionLimit = options.MaxConnectionCount;

        List<FcmSendResult> sendResults = new List<FcmSendResult>();

        // Dependency resolver for the IFcmSender
        var fcmSender = options.TestMode ? (IFcmSender)new MessageHelper.Fcm.Mock.MessageSender() : new MessageSender(options.ServerKey, options.SenderId);

        List<Task<FcmSendResult>> tasks = new List<Task<FcmSendResult>>();
        fcmMessages.ForEach(m => tasks.Add(fcmSender.SendNotifyAsync(m)));
        Task.WaitAll(tasks.ToArray());
        tasks.ForEach(t =>
        {
            if (t.IsCompleted)
                sendResults.Add(t.Result);
        });
        result = JsonConvert.SerializeObject(sendResults);
    }

    [Microsoft.SqlServer.Server.SqlProcedure(Name = "Fcm_AddTokenToGroup")]
    public static void AddTokenToGroup(bool testMode, string serverKey, string senderId, string groupName, string token, ref string groupKey)
    {
        // For testing base on fake data
        var fcmGroupHandler = testMode ? (IFcmGroupHandler)new MessageHelper.Fcm.Mock.FcmGroupHandler() :
            new FcmGroupHandler(serverKey, senderId);

        groupKey = fcmGroupHandler.AddTokenToGroup(groupName, new string[] { token }).Result;
    }

}

