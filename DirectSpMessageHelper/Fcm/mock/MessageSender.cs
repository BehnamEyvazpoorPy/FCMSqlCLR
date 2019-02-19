using MessageHelper.Fcm.Enum;
using MessageHelper.Fcm.Model;
using MessageHelper.Fcm.Signature;
using System.Threading.Tasks;

namespace MessageHelper.Fcm.Mock
{
    class MessageSender : IFcmSender
    {
        public async Task<FcmSendResult> SendNotifyAsync(Message message)
        {
            return await Task.Factory.StartNew(() => new FcmSendResult
            {
                MessageId = message.MessageId,
                Error = "Failed",
                Status = FcmSendStatus.Failed,
                Address = message.Address
            });
        }
    }
}
