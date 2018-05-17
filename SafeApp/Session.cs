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

// ReSharper disable ConvertToLocalFunction

namespace SafeApp {
  [PublicAPI]
  public sealed class Session : IDisposable {
    private static readonly IAppBindings AppBindings = AppResolver.Current;

    public static EventHandler Disconnected;
    private SafeAppPtr _appPtr;
    private GCHandle _disconnectedHandle;

    public bool IsDisconnected { get; private set; }

#if SAFE_APP_MOCK
    public IntPtr SafeApPtr() {
      return _appPtr;
    }
#endif

    /// <summary>
    ///   AccessConatiner API
    /// </summary>
    public AccessContainer AccessContainer { get; private set; }

    /// <summary>
    ///   Crypto API
    /// </summary>
    public Crypto Crypto { get; private set; }

    /// <summary>
    ///   CipherOpt API
    /// </summary>
    public CipherOpt CipherOpt { get; private set; }

    // ReSharper disable once InconsistentNaming

    /// <summary>
    ///   ImmutableData API
    /// </summary>
    public IData.IData IData { get; private set; }

    /// <summary>
    ///   MutableData API
    /// </summary>
    public MData.MData MData { get; private set; }

    /// <summary>
    ///   Mutable Data Entries API
    /// </summary>
    public MDataEntries MDataEntries { get; private set; }

    /// <summary>
    ///   Mutable Data Entry Actions API
    /// </summary>
    public MDataEntryActions MDataEntryActions { get; private set; }

    /// <summary>
    ///   MDataInfo API
    /// </summary>
    public MDataInfoActions MDataInfoActions { get; private set; }

    /// <summary>
    ///   Mutable Data Permissions API
    /// </summary>
    public MDataPermissions MDataPermissions { get; private set; }

    // ReSharper disable once InconsistentNaming

    /// <summary>
    ///   Mutable Data Permissions API
    /// </summary>
    public NFS NFS { get; private set; }

    private Session() {
      IsDisconnected = true;
      _appPtr = new SafeAppPtr();
    }

    public void Dispose() {
      FreeApp();
      GC.SuppressFinalize(this);
    }

    /// <summary>
    ///   Invoked to fetch the app's root container name
    /// </summary>
    /// <param name="appId"></param>
    /// <returns>string</returns>
    public Task<string> AppContainerNameAsync(string appId) {
      return AppBindings.AppContainerNameAsync(appId);
    }

    public static Task<Session> AppRegisteredAsync(string appId, AuthGranted authGranted) {
      return Task.Run(
        () => {
          var tcs = new TaskCompletionSource<Session>(TaskCreationOptions.RunContinuationsAsynchronously);
          var session = new Session();
          Action<FfiResult, IntPtr, GCHandle> acctCreatedCb = (result, ptr, disconnectedHandle) => {
            if (result.ErrorCode != 0) {
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

    public static Task<Session> AppUnregisteredAsync(List<byte> bootstrapConfig) {
      return Task.Run(
        () => {
          var tcs = new TaskCompletionSource<Session>(TaskCreationOptions.RunContinuationsAsynchronously);
          var session = new Session();
          Action<FfiResult, IntPtr, GCHandle> acctCreatedCb = (result, ptr, disconnectedHandle) => {
            if (result.ErrorCode != 0) {
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

    public static Task<IpcMsg> DecodeIpcMessageAsync(string encodedReq) {
      return AppBindings.DecodeIpcMsgAsync(encodedReq);
    }

    /// <summary>
    ///   Encodes an authorisation request to send to Authenticator
    /// </summary>
    /// <param name="authReq"></param>
    /// <returns>RequestId, Encoded auth request</returns>
    public static Task<(uint, string)> EncodeAuthReqAsync(AuthReq authReq) {
      return AppBindings.EncodeAuthReqAsync(ref authReq);
    }

    public static Task<(uint, string)> EncodeContainerRequestAsync(ContainersReq containersReq) {
      return AppBindings.EncodeContainersReqAsync(ref containersReq);
    }

    public static Task<(uint, string)> EncodeShareMDataRequestAsync(ShareMDataReq shareMDataReq) {
      return AppBindings.EncodeShareMDataReqAsync(ref shareMDataReq);
    }

    public static Task<(uint, string)> EncodeUnregisteredRequestAsync(string reqId) {
      return AppBindings.EncodeUnregisteredReqAsync(Encoding.UTF8.GetBytes(reqId).ToList());
    }

    ~Session() {
      FreeApp();
    }

    private void FreeApp() {
      if (_disconnectedHandle.IsAllocated) {
        _disconnectedHandle.Free();
      }

      if (_appPtr == IntPtr.Zero) {
        return;
      }

      AppBindings.AppFree(_appPtr);
      _appPtr.Clear();
    }

    /// <summary>
    ///   Returns the AccountInfo of the current Session
    /// </summary>
    /// <returns>AccountInfo</returns>
    public Task<AccountInfo> GetAccountInfoAsyc() {
      return AppBindings.AppAccountInfoAsync(_appPtr);
    }

    public static Task<string> GetExeFileStemAsync() {
      return AppBindings.AppExeFileStemAsync();
    }

    private void Init(IntPtr appPtr, GCHandle disconnectedHandle) {
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
      MDataInfoActions = new MDataInfoActions(_appPtr);
      MDataPermissions = new MDataPermissions(_appPtr);
      NFS = new NFS(_appPtr);
    }

    public static async Task InitLoggingAsync(string configFilesPath) {
      await AppBindings.AppSetAdditionalSearchPathAsync(configFilesPath);
      await AppBindings.AppInitLoggingAsync(null);
    }

    public static bool IsMockBuild() {
      return AppBindings.IsMockBuild();
    }

    private static void OnDisconnected(Session session) {
      session.IsDisconnected = true;
      Disconnected?.Invoke(session, EventArgs.Empty);
    }

    /// <summary>
    ///   Invoked after Disconnect callback is fired to reconnect the session with the network
    /// </summary>
    public Task ReconnectAsync() {
      return Task.Run(
        async () => {
          await AppBindings.AppReconnectAsync(_appPtr);
          IsDisconnected = false;
        });
    }

    /// <summary>
    ///   Resets the Object Cache for the session
    /// </summary>
    /// <returns>Void</returns>
    public Task ResetObjectCacheAsync() {
      return AppBindings.AppResetObjectCacheAsync(_appPtr);
    }

    public static Task SetAdditionalSearchPathAsync(string path) {
      return AppBindings.AppSetAdditionalSearchPathAsync(path);
    }

    public static Task SetLogOutputPathAsync(string outputFileName) {
      return AppBindings.AppOutputLogPathAsync(outputFileName);
    }
  }
}
