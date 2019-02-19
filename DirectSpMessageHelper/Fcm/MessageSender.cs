using MessageHelper.Fcm.Enum;
using MessageHelper.Fcm.Model;
using MessageHelper.Fcm.Signature;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MessageHelper.Fcm
{
    class MessageSender : IFcmSender
    {
        private readonly string _fcmServerKey;
        private readonly string _fcmSenderId;

        public MessageSender(string fcmServerKey, string fcmSenderId)
        {
            _fcmServerKey = fcmServerKey;
            _fcmSenderId = fcmSenderId;
        }
        public async Task<FcmSendResult> SendNotifyAsync(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var sendResults = new List<FcmSendResult>();
            var sender = new FcmSender(_fcmServerKey, _fcmSenderId);

            // Create an FCM message to send
            var fcmMessage = new FcmMessage
            {
                Priority = FcmPriority.High,
                DryRun = true,
                To = message.Address,
                Notification = new FcmNotification
                {
                    Title = "General",
                    Body = message.MessageBody
                }
            };

            FcmResponse response = null;
            try
            {
                // Send message to the FCM server
                response = await sender.Send(fcmMessage);
            }
            catch (WebException we)
            {
                if (we.Response != null)
                    using (StreamReader reader = new StreamReader(we.Response.GetResponseStream()))
                    {
                        string error = reader.ReadToEnd();
                        // Set failed status for the message
                        return new FcmSendResult
                        {
                            Error = error,
                            Address = message.Address,
                            Status = FcmSendStatus.Failed
                        };

                    }
            }
            catch (SystemException ex)
            {
                // Set failed status for the message
                return new FcmSendResult
                {
                    Error = ex.Message,
                    Address = message.Address,
                    Status = FcmSendStatus.Failed
                };

            }

            // Create result objects from fcm result
            var sendResult = new FcmSendResult
            {
                MessageId = message.MessageId,
                Address = message.Address
            };

            if (response.Results != null)
                sendResult.Error = response.Results.FirstOrDefault().Error ?? "";
            sendResult.Status = response.Success > 0 ? FcmSendStatus.Sent : FcmSendStatus.Pending;

            return sendResult;
        }
    }
}
