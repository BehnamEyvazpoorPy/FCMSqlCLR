using MessageHelper.Fcm.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageHelper.Fcm.Signature
{
    interface IFcmSender
    {
        Task<FcmSendResult> SendNotifyAsync(Message message);
    }
}
