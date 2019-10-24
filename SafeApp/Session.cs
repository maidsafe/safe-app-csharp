using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.API;
using SafeApp.AppBindings;
using SafeApp.Core;

// ReSharper disable UnusedMember.Global
// ReSharper disable ConvertToLocalFunction

namespace SafeApp
{
    /// <summary>
    /// Holds one session with the network and is the primary interface
    /// to interact with the network.
    /// As such it also provides all API-Providers connected through this session.
    /// </summary>
    [PublicAPI]
    public sealed class Session : IDisposable
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;

        /// <summary>
        /// Content Fetch API.
        /// </summary>
        public Fetch Fetch { get; private set; }

        /// <summary>
        /// Keys API.
        /// </summary>
        public Keys Keys { get; private set; }

        /// <summary>
        /// Wallet API.
        /// </summary>
        public API.Wallet Wallet { get; private set; }

        /// <summary>
        /// Files API.
        /// </summary>
        public Files Files { get; private set; }

        /// <summary>
        /// NRS API.
        /// </summary>
        public Nrs Nrs { get; private set; }

        /// <summary>
        /// Event triggered if session is disconnected from the network.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private
        public static EventHandler Disconnected;
#pragma warning restore SA1401 // Fields should be private
        private SafeAppPtr _appPtr;
        private GCHandle _disconnectedHandle;

        /// <summary>
        /// true if current network connection state is DISCONNECTED.
        /// </summary>
        public bool IsDisconnected { get; private set; }

#if SAFE_APP_MOCK
        public IntPtr SafeApPtr()
        {
            return _appPtr;
        }
#endif

        private Session()
        {
            IsDisconnected = true;
            _appPtr = new SafeAppPtr();
        }

        /// <summary>
        /// Create a new authenticated session using the provided IPC response.
        /// </summary>
        /// <param name="appId">Application Id.</param>
        /// <param name="authResponse">Authentication response message.</param>
        /// <returns>New session based on appid and authentication response.</returns>
        public static Task<Session> AppConnectAsync(string appId, string authResponse)
        {
            return Task.Run(() =>
            {
                var tcs = new TaskCompletionSource<Session>(TaskCreationOptions.RunContinuationsAsynchronously);
                var session = new Session();
                Action<FfiResult, IntPtr, GCHandle> acctConnectedCb = (result, ptr, disconnectedHandle) =>
                {
                    if (result.ErrorCode != 0)
                    {
                        tcs.SetException(result.ToException());
                        return;
                    }

                    session.Init(ptr, disconnectedHandle);
                    tcs.SetResult(session);
                };

                AppBindings.Connect(appId, authResponse, acctConnectedCb);
                return tcs.Task;
            });
        }

        /// <summary>
        /// Creates an unregistered session for the provided app Id using vault_connection_configuration file.
        /// </summary>
        /// <param name="appId">Application Id.</param>
        /// <returns></returns>
        public static Task<Session> AppConnectUnregisteredAsync(string appId)
        {
            return Task.Run(() =>
            {
                var tcs = new TaskCompletionSource<Session>(TaskCreationOptions.RunContinuationsAsynchronously);
                var session = new Session();
                Action<FfiResult, IntPtr, GCHandle> acctConnectedCb = (result, ptr, disconnectedHandle) =>
                {
                    if (result.ErrorCode != 0)
                    {
                        tcs.SetException(result.ToException());
                        return;
                    }

                    session.Init(ptr, disconnectedHandle);
                    tcs.SetResult(session);
                };

                AppBindings.Connect(appId, null, acctConnectedCb);
                return tcs.Task;
            });
        }

        /// <summary>
        /// Decode the IPC response message.
        /// </summary>
        /// <param name="encodedResponse">Encoded response string.</param>
        /// <returns>New decoded IPCMsg instance.</returns>
        public static Task<IpcMsg> DecodeIpcMessageAsync(string encodedResponse)
        {
            return AppBindings.DecodeIpcMsgAsync(encodedResponse);
        }

        /// <summary>
        /// Encodes an authentication request.
        /// </summary>
        /// <param name="authReq">Authentication Request.</param>
        /// <returns>RequestId, Encoded authentication request.</returns>
        public static Task<(uint, string)> EncodeAuthReqAsync(AuthReq authReq)
        {
            return AppBindings.EncodeAuthReqAsync(ref authReq);
        }

        /// <summary>
        /// Encodes a container permission request.
        /// </summary>
        /// <param name="containersReq">Container Request</param>
        /// <returns>Request Id, Encoded container request.</returns>
        public static Task<(uint, string)> EncodeContainerRequestAsync(ContainersReq containersReq)
        {
            return AppBindings.EncodeContainersReqAsync(ref containersReq);
        }

        /// <summary>
        /// Encodes a MDataShareReq.
        /// </summary>
        /// <param name="shareMDataReq">Mutable Data share request.</param>
        /// <returns>Request Id, Encoded Mutable Data share request.</returns>
        public static Task<(uint, string)> EncodeShareMDataRequestAsync(ShareMDataReq shareMDataReq)
        {
            return AppBindings.EncodeShareMDataReqAsync(ref shareMDataReq);
        }

        /// <summary>
        /// Encodes a unregistered access request.
        /// </summary>
        /// <param name="reqId">Request Id.</param>
        /// <returns></returns>
        public static Task<(uint, string)> EncodeUnregisteredRequestAsync(string reqId)
        {
            return AppBindings.EncodeUnregisteredReqAsync(Encoding.UTF8.GetBytes(reqId));
        }

        /// <summary>
        /// Returns the expected name for the application executable without an extension.
        /// </summary>
        /// <returns>Application executable name.</returns>
        public static Task<string> GetExeFileStemAsync()
        {
            return AppBindings.AppExeFileStemAsync();
        }

        /// <summary>
        /// Sets the additional path in `config_file_handler` to search configuration files.
        /// </summary>
        /// <param name="path">Configuration file path.</param>
        /// <returns></returns>
        public static Task SetAdditionalSearchPathAsync(string path)
        {
            return AppBindings.AppSetAdditionalSearchPathAsync(path);
        }

        /// <summary>
        /// Sets path to search configuration files.
        /// </summary>
        /// <param name="path">Configuration file path.</param>
        /// <returns></returns>
        public static Task SetConfigurationFilePathAsync(string path)
        {
            return AppBindings.AppSetConfigDirPathAsync(path);
        }

        /// <summary>
        /// Get the output log file path.
        /// </summary>
        /// <param name="outputFileName">File name.</param>
        /// <returns>Log output file path along with file name.</returns>
        public static Task<string> GetLogOutputPathAsync([Optional] string outputFileName)
        {
            return AppBindings.AppOutputLogPathAsync(outputFileName);
        }

        /// <summary>
        /// Public implementation of Dispose pattern callable by developers.
        /// </summary>
        public void Dispose()
        {
            FreeApp();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Class destructor.
        /// </summary>
        ~Session()
        {
            FreeApp();
        }

        private void FreeApp()
        {
            if (_disconnectedHandle.IsAllocated)
            {
                _disconnectedHandle.Free();
            }

            if (_appPtr == IntPtr.Zero)
            {
                return;
            }

            _appPtr.Clear();
        }

        private void Init(IntPtr appPtr, GCHandle disconnectedHandle)
        {
            IsDisconnected = false;
            _appPtr = new SafeAppPtr(appPtr);
            _disconnectedHandle = disconnectedHandle;

            Fetch = new Fetch(_appPtr);
            Keys = new Keys(_appPtr);
            Wallet = new API.Wallet(_appPtr);
            Files = new Files(_appPtr);
            Nrs = new Nrs(_appPtr);
        }

        /// <summary>
        /// Initialise the logging.
        /// Pass the file name to replace default output file name i.e. client.log.
        /// </summary>
        /// <param name="outputLogFileName">Log output file name.</param>
        /// <returns></returns>
        public static async Task InitLoggingAsync([Optional] string outputLogFileName)
        {
            await AppBindings.AppInitLoggingAsync(outputLogFileName);
        }

        /// <summary>
        /// Check if the native library was compiled with mock-routing feature.
        /// </summary>
        /// <returns>True if native library was compiled with mock-routing feature otherwise false.</returns>
        public static bool IsMockBuild()
        {
            return AppBindings.IsMockBuild();
        }

        private static void OnDisconnected(Session session)
        {
            session.IsDisconnected = true;
            Disconnected?.Invoke(session, EventArgs.Empty);
        }
    }
}
