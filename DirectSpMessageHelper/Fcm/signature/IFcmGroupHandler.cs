using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessageHelper
{
    interface IFcmGroupHandler
    {
        Task<string> AddTokenToGroup(string groupName, string[] tokens);
    }
}
