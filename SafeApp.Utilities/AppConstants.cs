using JetBrains.Annotations;

namespace SafeApp.Utilities {
  [PublicAPI]
  public static class AppConstants {
    public const ulong AsymNonceLen = 24;
    public const ulong AsymPublicKeyLen = 32;
    public const ulong AsymSecretKeyLen = 32;
    public const ulong DirTag = 15000;
    public const ulong MaidsafeTag = 5483000;
    public const string MDataMetaDataKey = "_metadata";
    public const ulong NullObjectHandle = 0;
    public const ulong SignPublicKeyLen = 32;
    public const ulong SignSecretKeyLen = 64;
    public const ulong SymKeyLen = 32;
    public const ulong SymNonceLen = 24;
    public const ulong XorNameLen = 32;
  }
}
