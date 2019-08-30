using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SafeApp.Utilities;

namespace SafeApp.MockAuthBindings
{
    internal partial interface IAuthBindings
    {
        Task<AccountInfo> AuthAccountInfoAsync(IntPtr auth);

        Task<List<AppAccess>> AuthAppsAccessingMutableDataAsync(IntPtr auth, byte[] mdName, ulong mdTypeTag);

        Task<string> AuthExeFileStemAsync();

        Task AuthFlushAppRevocationQueueAsync(IntPtr auth);

        void AuthFree(IntPtr auth);

        Task AuthInitLoggingAsync(string outputFileNameOverride);

        Task<string> AuthOutputLogPathAsync(string outputFileName);

        Task AuthReconnectAsync(IntPtr auth);

        Task<List<RegisteredApp>> AuthRegisteredAppsAsync(IntPtr auth);

        Task<string> AuthRevokeAppAsync(IntPtr auth, string appId);

        Task<List<AppExchangeInfo>> AuthRevokedAppsAsync(IntPtr auth);

        Task AuthRmRevokedAppAsync(IntPtr auth, string appId);

        Task AuthSetAdditionalSearchPathAsync(string newPath);

        Task<string> EncodeAuthRespAsync(IntPtr auth, ref AuthReq req, uint reqId, bool isGranted);

        Task<string> EncodeContainersRespAsync(IntPtr auth, ref ContainersReq req, uint reqId, bool isGranted);

        Task<string> EncodeShareMDataRespAsync(IntPtr auth, ref ShareMDataReq req, uint reqId, bool isGranted);

        Task<string> EncodeUnregisteredRespAsync(uint reqId, bool isGranted);

        Task TestSimulateNetworkDisconnectAsync(IntPtr authPtr);

        bool IsMockBuild();
    }
}
