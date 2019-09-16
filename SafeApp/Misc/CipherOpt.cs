using System.Threading.Tasks;
using SafeApp.AppBindings;

// ReSharper disable ConvertToLocalFunction
// ReSharper disable UnusedMember.Global

namespace SafeApp.Misc
{
    /// <summary>
    /// Provide the CipherOpt APIs
    /// </summary>
    public class CipherOpt
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;
        private readonly SafeAppPtr _appPtr;

        /// <summary>
        /// Initialises an CipherOpt object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer</param>
        internal CipherOpt(SafeAppPtr appPtr)
        {
            _appPtr = appPtr;
        }

        private Task FreeAsync(ulong cipherOptHandle)
        {
            return AppBindings.CipherOptFreeAsync(_appPtr, cipherOptHandle);
        }

        /// <summary>
        /// Create a new Asymmetric CipherOpt handle.
        /// </summary>
        /// <param name="encPubKeyH">NativeHandle to public Encryption Key.</param>
        /// <returns>New Asymmetric CipherOpt NativeHandle.</returns>
        public async Task<NativeHandle> NewAsymmetricAsync(NativeHandle encPubKeyH)
        {
            var cipherOptH = await AppBindings.CipherOptNewAsymmetricAsync(_appPtr, encPubKeyH);
            return new NativeHandle(_appPtr, cipherOptH, FreeAsync);
        }

        /// <summary>
        /// Create a new Plain text CipherOpt handle.
        /// </summary>
        /// <returns>New plain text NativeHandle.</returns>
        public async Task<NativeHandle> NewPlaintextAsync()
        {
            var cipherOptH = await AppBindings.CipherOptNewPlaintextAsync(_appPtr);
            return new NativeHandle(_appPtr, cipherOptH, FreeAsync);
        }

        /// <summary>
        /// Create a new Symmetric CipherOpt handle.
        /// </summary>
        /// <returns>New Symmetric NativeHandle.</returns>
        public async Task<NativeHandle> NewSymmetricAsync()
        {
            var cipherOptH = await AppBindings.CipherOptNewSymmetricAsync(_appPtr);
            return new NativeHandle(_appPtr, cipherOptH, FreeAsync);
        }
    }
}
