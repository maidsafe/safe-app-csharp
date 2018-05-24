using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Utilities;

namespace SafeApp.MockAuthBindings {
  // ReSharper disable ConvertToLocalFunction
  // ReSharper disable UnusedMember.Global
  // ReSharper disable MemberCanBePrivate.Global
  public class Authenticator : IDisposable {
    private static readonly IAuthBindings NativeBindings = MockAuthResolver.Current;
    //ReSharper disable once UnassignedField.Global
    public static EventHandler Disconnected;
    private IntPtr _authPtr;
    private GCHandle _disconnectedHandle;
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public bool IsDisconnected { get; private set; }

    private Authenticator() {
      IsDisconnected = true;
      _authPtr = IntPtr.Zero;
    }

    public void Dispose() {
      FreeAuth();
      GC.SuppressFinalize(this);
    }

    public static bool IsMockBuild() {
      return NativeBindings.IsMockBuild();
    }

    public Task<AccountInfo> AuthAccountInfoAsync() {
      return NativeBindings.AuthAccountInfoAsync(_authPtr);
    }

    public Task<List<AppAccess>> AuthAppsAccessingMutableDataAsync(byte[] name, ulong typeTag) {
      return NativeBindings.AuthAppsAccessingMutableDataAsync(_authPtr, name, typeTag);
    }

    public static Task<string> AuthExeFileStemAsync() {
      return NativeBindings.AuthExeFileStemAsync();
    }

    public Task AuthFlushAppRevocationQueueAsync() {
      return NativeBindings.AuthFlushAppRevocationQueueAsync(_authPtr);
    }

    public static Task AuthInitLoggingAsync(string outputFileName) {
      return NativeBindings.AuthInitLoggingAsync(outputFileName);
    }

    public static Task AuthOutputLogPathAsync(string outputFileName) {
      return NativeBindings.AuthOutputLogPathAsync(outputFileName);
    }

    public Task AuthReconnectAsync() {
      return NativeBindings.AuthReconnectAsync(_authPtr);
    }

    public Task<List<RegisteredApp>> AuthRegisteredAppsAsync() {
      return NativeBindings.AuthRegisteredAppsAsync(_authPtr);
    }

    public Task AuthRevokeAppAsync(string appId) {
      return NativeBindings.AuthRevokeAppAsync(_authPtr, appId);
    }

    public Task<List<AppExchangeInfo>> AuthRevokedAppsAsync() {
      return NativeBindings.AuthRevokedAppsAsync(_authPtr);
    }

    public Task AuthRmRevokedAppAsync(string appId) {
      return NativeBindings.AuthRmRevokedAppAsync(_authPtr, appId);
    }

    public static Task AuthSetAdditionalSearchPathAsync(string newPath) {
      return NativeBindings.AuthSetAdditionalSearchPathAsync(newPath);
    }

    public static Task<Authenticator> CreateAccountAsync(string locator, string secret, string invitation) {
      return Task.Run(
        () => {
          var authenticator = new Authenticator();
          var tcs = new TaskCompletionSource<Authenticator>(TaskCreationOptions.RunContinuationsAsynchronously);
          Action disconnect = () => { OnDisconnected(authenticator); };
          Action<FfiResult, IntPtr, GCHandle> cb = (result, ptr, disconnectHandle) => {
            if (result.ErrorCode != 0) {
              if (disconnectHandle.IsAllocated) {
                disconnectHandle.Free();
              }

              tcs.SetException(result.ToException());
              return;
            }

            authenticator.Init(ptr, disconnectHandle);
            tcs.SetResult(authenticator);
          };
          NativeBindings.CreateAccount(locator, secret, invitation, disconnect, cb);
          return tcs.Task;
        });
    }

    public Task<IpcReq> DecodeIpcMessageAsync(string msg) {
      return NativeBindings.DecodeIpcMessage(_authPtr, msg);
    }

    public static Task<IpcReq> UnRegisteredDecodeIpcMsgAsync(string msg) {
      return NativeBindings.UnRegisteredDecodeIpcMsgAsync(msg);
    }

    public Task<string> EncodeAuthRespAsync(AuthIpcReq authIpcReq, bool allow) {
      return NativeBindings.EncodeAuthRespAsync(_authPtr, ref authIpcReq.AuthReq, authIpcReq.ReqId, allow);
    }

    public Task<string> EncodeContainersRespAsync(ContainersIpcReq req, bool allow) {
      return NativeBindings.EncodeContainersRespAsync(_authPtr, ref req.ContainersReq, req.ReqId, allow);
    }

    public Task<string> EncodeShareMdataRespAsync(ShareMDataIpcReq req, bool allow) {
      return NativeBindings.EncodeShareMDataRespAsync(_authPtr, ref req.ShareMDataReq, req.ReqId, allow);
    }

    public static Task<string> EncodeUnregisteredRespAsync(uint reqId, bool allow) {
      return NativeBindings.EncodeUnregisteredRespAsync(reqId, allow);
    }

    ~Authenticator() {
      FreeAuth();
    }

    private void FreeAuth() {
      if (_disconnectedHandle.IsAllocated) {
        _disconnectedHandle.Free();
      }

      if (_authPtr == IntPtr.Zero) {
        return;
      }

      NativeBindings.AuthFree(_authPtr);
      _authPtr = IntPtr.Zero;
    }

    private void Init(IntPtr authPtr, GCHandle disconnectedHandle) {
      _authPtr = authPtr;
      _disconnectedHandle = disconnectedHandle;
      IsDisconnected = false;
    }

    public static Task<Authenticator> LoginAsync(string locator, string secret) {
      return Task.Run(
        () => {
          var authenticator = new Authenticator();
          var tcs = new TaskCompletionSource<Authenticator>(TaskCreationOptions.RunContinuationsAsynchronously);
          Action disconnect = () => { OnDisconnected(authenticator); };
          Action<FfiResult, IntPtr, GCHandle> cb = (result, ptr, disconnectHandle) => {
            if (result.ErrorCode != 0) {
              if (disconnectHandle.IsAllocated) {
                disconnectHandle.Free();
              }

              tcs.SetException(result.ToException());
              return;
            }

            authenticator.Init(ptr, disconnectHandle);
            tcs.SetResult(authenticator);
          };
          NativeBindings.Login(locator, secret, disconnect, cb);
          return tcs.Task;
        });
    }

    private static void OnDisconnected(Authenticator authenticator) {
      authenticator.IsDisconnected = true;
      Disconnected?.Invoke(authenticator, EventArgs.Empty);
    }
  }
}
