using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.MData;
using SafeApp.Misc;
using SafeApp.Utilities;

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

        /// <summary>
        /// AccessContainer API
        /// </summary>
        public AccessContainer AccessContainer { get; private set; }

        /// <summary>
        /// Crypto API
        /// </summary>
        public Crypto Crypto { get; private set; }

        /// <summary>
        /// CipherOpt API
        /// </summary>
        public CipherOpt CipherOpt { get; private set; }

        // ReSharper disable once InconsistentNaming

        /// <summary>
        /// ImmutableData API
        /// </summary>
        public IData.IData IData { get; private set; }

        /// <summary>
        /// MutableData API
        /// </summary>
        public MData.MData MData { get; private set; }

        /// <summary>
        /// Mutable Data Entries API
        /// </summary>
        public MDataEntries MDataEntries { get; private set; }

        /// <summary>
        /// Mutable Data Entry Actions API
        /// </summary>
        public MDataEntryActions MDataEntryActions { get; private set; }

        /// <summary>
        /// MDataInfo API
        /// </summary>
        public MDataInfoActions MDataInfoActions { get; private set; }

        /// <summary>
        /// Mutable Data Permissions API
        /// </summary>
        public MDataPermissions MDataPermissions { get; private set; }

        // ReSharper disable once InconsistentNaming

        /// <summary>
        /// Mutable Data Permissions API
        /// </summary>
        public NFS NFS { get; private set; }

        private Session()
        {
            IsDisconnected = true;
            _appPtr = new SafeAppPtr();
        }

        /// <summary>
        /// Create a new authenticated session using the provided IPC response.
        /// </summary>
        /// <param name="appId">Application Id.</param>
        /// <param name="authGranted">Authentication response.</param>
        /// <returns>New session based on appid and authentication response.</returns>
        public static Task<Session> AppRegisteredAsync(string appId, AuthGranted authGranted)
        {
            return Task.Run(
                () =>
                {
                    var tcs = new TaskCompletionSource<Session>(TaskCreationOptions.RunContinuationsAsynchronously);
                    var session = new Session();
                    Action<FfiResult, IntPtr, GCHandle> acctCreatedCb = (result, ptr, disconnectedHandle) =>
                    {
                        if (result.ErrorCode != 0)
                        {
                            disconnectedHandle.Free();

                            tcs.SetException(result.ToException());
                            return;
                        }

                        session.Init(ptr, disconnectedHandle);
                        tcs.SetResult(session);
                    };

                    Action disconnectedCb = () => { OnDisconnected(session); };

                    AppBindings.AppRegistered(appId, ref authGranted, disconnectedCb, acctCreatedCb);
                    return tcs.Task;
                });
        }

        /// <summary>
        /// Creates an unregistered session based on the config provided.
        /// Config information can be obtained from the UnregisteredIpcResponse.
        /// </summary>
        /// <param name="bootstrapConfig"></param>
        /// <returns></returns>
        public static Task<Session> AppUnregisteredAsync(List<byte> bootstrapConfig)
        {
            return Task.Run(
                () =>
                {
                    var tcs = new TaskCompletionSource<Session>(TaskCreationOptions.RunContinuationsAsynchronously);
                    var session = new Session();
                    Action<FfiResult, IntPtr, GCHandle> acctCreatedCb = (result, ptr, disconnectedHandle) =>
                    {
                        if (result.ErrorCode != 0)
                        {
                            disconnectedHandle.Free();

                            tcs.SetException(result.ToException());
                            return;
                        }

                        session.Init(ptr, disconnectedHandle);
                        tcs.SetResult(session);
                    };

                    Action disconnectedCb = () => { OnDisconnected(session); };

                    AppBindings.AppUnregistered(bootstrapConfig, disconnectedCb, acctCreatedCb);
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
            return AppBindings.EncodeUnregisteredReqAsync(Encoding.UTF8.GetBytes(reqId).ToList());
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
        /// Invoked to fetch the app's root container name.
        /// </summary>
        /// <param name="appId">Application id.</param>
        /// <returns>Application's root container name.</returns>
        public Task<string> AppContainerNameAsync(string appId)
        {
            return AppBindings.AppContainerNameAsync(appId);
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

            AppBindings.AppFree(_appPtr);
            _appPtr.Clear();
        }

        /// <summary>
        /// Returns the AccountInfo of the current session.
        /// </summary>
        /// <returns>AccountInfo object.</returns>
        public Task<AccountInfo> GetAccountInfoAsync()
        {
            return AppBindings.AppAccountInfoAsync(_appPtr);
        }

        private void Init(IntPtr appPtr, GCHandle disconnectedHandle)
        {
            IsDisconnected = false;
            _appPtr = new SafeAppPtr(appPtr);
            _disconnectedHandle = disconnectedHandle;

            AccessContainer = new AccessContainer(_appPtr);
            Crypto = new Crypto(_appPtr);
            CipherOpt = new CipherOpt(_appPtr);
            IData = new IData.IData(_appPtr);
            MData = new MData.MData(_appPtr);
            MDataEntries = new MDataEntries(_appPtr);
            MDataEntryActions = new MDataEntryActions(_appPtr);
            MDataInfoActions = new MDataInfoActions();
            MDataPermissions = new MDataPermissions(_appPtr);
            NFS = new NFS(_appPtr);
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

        /// <summary>
        /// Invoked after Disconnect callback is fired to reconnect the session with the network.
        /// </summary>
        public Task ReconnectAsync()
        {
            return Task.Run(
                async () =>
                {
                    await AppBindings.AppReconnectAsync(_appPtr);
                    IsDisconnected = false;
                });
        }

        /// <summary>
        /// Resets the object cache for the session.
        /// </summary>
        /// <returns></returns>
        public Task ResetObjectCacheAsync()
        {
            return AppBindings.AppResetObjectCacheAsync(_appPtr);
        }
    }
}
