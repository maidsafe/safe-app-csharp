using System.Threading.Tasks;
using SafeApp.Core;

// ReSharper disable once CheckNamespace

namespace SafeApp.AppBindings
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public partial interface IAppBindings
    {
        Task<IpcMsg> DecodeIpcMsgAsync(string msg);
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
