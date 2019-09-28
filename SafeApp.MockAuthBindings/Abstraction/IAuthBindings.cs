﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SafeApp.Core;

// ReSharper disable once CheckNamespace

namespace SafeApp.MockAuthBindings
{
    internal partial interface IAuthBindings
    {
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

        Task<string> EncodeAuthRespAsync(IntPtr auth, AuthReq req, uint reqId, bool isGranted);

        Task<string> EncodeContainersRespAsync(IntPtr auth, ContainersReq req, uint reqId, bool isGranted);

        Task<string> EncodeShareMDataRespAsync(IntPtr auth, ShareMDataReq req, uint reqId, bool isGranted);

        Task<string> EncodeUnregisteredRespAsync(uint reqId, bool isGranted);

        Task TestSimulateNetworkDisconnectAsync(IntPtr authPtr);

        bool IsMockBuild();
    }
}
