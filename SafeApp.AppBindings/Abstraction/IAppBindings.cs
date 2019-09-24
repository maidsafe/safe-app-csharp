using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SafeApp.Core;

// ReSharper disable once CheckNamespace

namespace SafeApp.AppBindings
{
    // ReSharper disable InconsistentNaming
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public partial interface IAppBindings
    {
        Task<string> AppExeFileStemAsync();

        Task AppInitLoggingAsync(string outputFileNameOverride);

        Task<string> AppOutputLogPathAsync(string outputFileName);

        Task AppSetAdditionalSearchPathAsync(string newPath);

        Task<(uint, string)> EncodeAuthReqAsync(ref AuthReq req);

        Task<(uint, string)> EncodeContainersReqAsync(ref ContainersReq req);

        Task<(uint, string)> EncodeShareMDataReqAsync(ref ShareMDataReq req);

        Task<(uint, string)> EncodeUnregisteredReqAsync(byte[] extraData);

        bool IsMockBuild();
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
