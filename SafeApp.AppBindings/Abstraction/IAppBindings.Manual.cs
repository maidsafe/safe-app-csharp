using System.Threading.Tasks;
using SafeApp.Core;

// ReSharper disable once CheckNamespace

namespace SafeApp.AppBindings
{
    public partial interface IAppBindings
    {
        Task<IpcMsg> DecodeIpcMsgAsync(string msg);
    }
}
