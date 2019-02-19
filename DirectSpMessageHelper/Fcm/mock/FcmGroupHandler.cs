using System.Threading.Tasks;

namespace MessageHelper.Fcm.Mock
{
    class FcmGroupHandler : IFcmGroupHandler
    {
        public Task<string> AddTokenToGroup(string groupName, string[] tokens)
        {
            return Task.FromResult("test group key");
        }
    }
}
