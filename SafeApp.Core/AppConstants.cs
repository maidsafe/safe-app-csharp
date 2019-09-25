using JetBrains.Annotations;

namespace SafeApp.Core
{
    /// <summary>
    /// Constants used in SafeApp
    /// </summary>
    [PublicAPI]
    public static class AppConstants
    {
        /// <summary>
        /// Length of Asymmetric key nonce
        /// </summary>
        public const ulong AsymNonceLen = 24;

        /// <summary>
        /// Length of Asymmetric public key
        /// </summary>
        public const ulong AsymPublicKeyLen = 32;

        /// <summary>
        /// Length of Asymmetric Secret Key
        /// </summary>
        public const ulong AsymSecretKeyLen = 32;

        /// <summary>
        /// Get next version for a file in NFS
        /// </summary>
        public const ulong GetNextVersion = 0;

        /// <summary>
        /// Length of Symmetric Public Sign Key
        /// </summary>
        public const ulong SignPublicKeyLen = 32;

        /// <summary>
        /// Length of Symmetric Secret Sign Key
        /// </summary>
        public const ulong SignSecretKeyLen = 64;

        /// <summary>
        /// Length of Symmetric key
        /// </summary>
        public const ulong SymKeyLen = 32;

        /// <summary>
        /// Length of Symmetric key nonce
        /// </summary>
        public const ulong SymNonceLen = 24;

        /// <summary>
        /// `MutableData` type tag for a directory.
        /// </summary>
        public const ulong DirTag = 15000;

        /// <summary>
        /// All Maidsafe tagging should positive-offset from this.
        /// </summary>
        public const ulong MaidsafeTag = 5483000;

        /// <summary>
        /// Entry key under which the metadata are stored.
        /// </summary>
        public const string MDataMetaDataKey = "_metadata";

        /// <summary>
        /// Value for null handle
        /// </summary>
        public const ulong NullObjectHandle = 0;

        /// <summary>
        /// Constant byte length of `XorName`
        /// </summary>
        public const ulong XorNameLen = 32;

        /// <summary>
        /// Constant byte length of `BlsPublicKey`
        /// </summary>
        public const ulong BlsPublicKeyLen = 48;
    }
}
