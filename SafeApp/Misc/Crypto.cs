using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;

// ReSharper disable ConvertToLocalFunction
namespace SafeApp.Misc
{
    /// <summary>
    /// Encryption functionality for the SafeApp.
    /// </summary>
    [PublicAPI]
    public class Crypto
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;
        private SafeAppPtr _appPtr;

        /// <summary>
        /// Initialises an Crypto instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer</param>
        internal Crypto(SafeAppPtr appPtr)
        {
            _appPtr = appPtr;
        }

        /// <summary>
        /// Generate a nonce.
        /// </summary>
        /// <returns>Generated nonce.</returns>
        public static Task<byte[]> GenerateNonceAsync()
        {
            return AppBindings.GenerateNonceAsync();
        }

        /// <summary>
        /// Get SHA3 hash of the data.
        /// </summary>
        /// <param name="source">Data to be hashed.</param>
        /// <returns>Generated hash.</returns>
        public static Task<List<byte>> Sha3HashAsync(List<byte> source)
        {
            return AppBindings.Sha3HashAsync(source);
        }

        /// <summary>
        /// Get App's Public Sign Key.
        /// </summary>
        /// <returns>App's Public Sign Key handle.</returns>
        public async Task<NativeHandle> AppPubEncKeyAsync()
        {
            var appPubSignKeyH = await AppBindings.AppPubEncKeyAsync(_appPtr);
            return new NativeHandle(_appPtr, appPubSignKeyH, EncPubKeyFreeAsync);
        }

        /// <summary>
        /// Get App's Public Sign Key.
        /// </summary>
        /// <returns>App's Public Sign Key handle.</returns>
        public async Task<NativeHandle> AppPubSignKeyAsync()
        {
            var appPubEncKeyH = await AppBindings.AppPubSignKeyAsync(_appPtr);
            return new NativeHandle(_appPtr, appPubEncKeyH, SignPubKeyFreeAsync);
        }

        /// <summary>
        /// Asymmetric decryption using public and Private Key pair.
        /// </summary>
        /// <param name="cipherText">Cipher to decrypt.</param>
        /// <param name="encPubKey">Public Encryption Key.</param>
        /// <param name="encSecKey">Secret Encryption Key.</param>
        /// <returns>Decrypted data.</returns>
        public Task<List<byte>> DecryptAsync(List<byte> cipherText, NativeHandle encPubKey, NativeHandle encSecKey)
        {
            return AppBindings.DecryptAsync(_appPtr, cipherText, encPubKey, encSecKey);
        }

        /// <summary>
        /// Decrypt cipher text from a Sender.
        /// </summary>
        /// <param name="cipherText">Cipher Text to be decrypted.</param>
        /// <param name="pkHandle">Sender's Encrypt Public Key.</param>
        /// <param name="skHandle">Receiver's Encrypt Secret Key.</param>
        /// <returns>Decrypted data.</returns>
        public Task<List<byte>> DecryptSealedBoxAsync(List<byte> cipherText, NativeHandle pkHandle, NativeHandle skHandle)
        {
            return AppBindings.DecryptSealedBoxAsync(_appPtr, cipherText, pkHandle, skHandle);
        }

        /// <summary>
        /// Generate New Encrypt Key Pair.
        /// </summary>
        /// <returns>(Encrypt Public Key, Encrypt Secret Key).</returns>
        public async Task<(NativeHandle, NativeHandle)> EncGenerateKeyPairAsync()
        {
            var (encPubKeyH, encSecKeyH) = await AppBindings.EncGenerateKeyPairAsync(_appPtr);
            return (new NativeHandle(_appPtr, encPubKeyH, EncPubKeyFreeAsync),
                new NativeHandle(_appPtr, encSecKeyH, EncSecretKeyFreeAsync));
        }

        /// <summary>
        /// Free Encrypt Public Key Handle.
        /// </summary>
        /// <param name="encPubKeyH">Public Encryption Key handle.</param>
        /// <returns></returns>
        private Task EncPubKeyFreeAsync(ulong encPubKeyH)
        {
            return AppBindings.EncPubKeyFreeAsync(_appPtr, encPubKeyH);
        }

        /// <summary>
        /// Generates raw string copy of public Encryption Key.
        /// </summary>
        /// <param name="encPubKeyH"></param>
        /// <returns></returns>
        public Task<byte[]> EncPubKeyGetAsync(NativeHandle encPubKeyH)
        {
            return AppBindings.EncPubKeyGetAsync(_appPtr, encPubKeyH);
        }

        /// <summary>
        /// Get a PublicKey NativeHandle from raw bytes.
        /// </summary>
        /// <param name="asymPublicKeyBytes">raw public Encryption Key raw bytes as string</param>
        /// <returns></returns>
        public async Task<NativeHandle> EncPubKeyNewAsync(byte[] asymPublicKeyBytes)
        {
            var encryptPubKeyH = await AppBindings.EncPubKeyNewAsync(_appPtr, asymPublicKeyBytes);
            return new NativeHandle(_appPtr, encryptPubKeyH, EncPubKeyFreeAsync);
        }

        /// <summary>
        /// Asymmetric encryption using public and Private Key pair.
        /// </summary>
        /// <param name="data">Data to be encrypted.</param>
        /// <param name="encPubKey">Public Encryption Key.</param>
        /// <param name="encSecKey">Secret Encryption Key.</param>
        /// <returns></returns>
        public Task<List<byte>> EncryptAsync(List<byte> data, NativeHandle encPubKey, NativeHandle encSecKey)
        {
            return AppBindings.EncryptAsync(_appPtr, data, encPubKey, encSecKey);
        }

        /// <summary>
        /// Encrypt the input (buffer or string) using the private and public key with a seal
        /// </summary>
        /// <param name="inputData">Data to be encrypted.</param>
        /// <param name="pkHandle"></param>
        /// <returns>Ciphered text in byte list format.</returns>
        public Task<List<byte>> EncryptSealedBoxAsync(List<byte> inputData, NativeHandle pkHandle)
        {
            return AppBindings.EncryptSealedBoxAsync(_appPtr, inputData, pkHandle);
        }

        private Task EncSecretKeyFreeAsync(ulong encSecKeyH)
        {
            return AppBindings.EncSecretKeyFreeAsync(_appPtr, encSecKeyH);
        }

        /// <summary>
        /// Generate raw string copy of secret Encryption Key
        /// </summary>
        /// <param name="encSecKeyH"></param>
        /// <returns></returns>
        public Task<byte[]> EncSecretKeyGetAsync(NativeHandle encSecKeyH)
        {
            return AppBindings.EncSecretKeyGetAsync(_appPtr, encSecKeyH);
        }

        /// <summary>
        /// Interpret the secret Encryption Key from a given raw string.
        /// </summary>
        /// <param name="asymSecKeyBytes">raw secret Encryption Key raw bytes as string</param>
        /// <returns></returns>
        public async Task<NativeHandle> EncSecretKeyNewAsync(byte[] asymSecKeyBytes)
        {
            var encSecKeyH = await AppBindings.EncSecretKeyNewAsync(_appPtr, asymSecKeyBytes);
            return new NativeHandle(_appPtr, encSecKeyH, EncSecretKeyFreeAsync);
        }

        /// <summary>
        /// Sign the given data (buffer) using a Secret Sign Key.
        /// </summary>
        /// <param name="data">Data to sign.</param>
        /// <param name="signSecKey">Secret Sign Key to sign the given data.</param>
        /// <returns>Returns the signed data.</returns>
        public Task<List<byte>> SignAsync(List<byte> data, NativeHandle signSecKey)
        {
            return AppBindings.SignAsync(_appPtr, data, signSecKey);
        }

        /// <summary>
        /// Generate a new Sign Key Pair
        /// </summary>
        /// <returns>Tuple of Sign Public Key NativeHandle and Sign Secret Key NativeHandle</returns>
        public async Task<(NativeHandle, NativeHandle)> SignGenerateKeyPairAsync()
        {
            var (publicKeyHandle, secretKeyHandle) = await AppBindings.SignGenerateKeyPairAsync(_appPtr);
            return (new NativeHandle(_appPtr, publicKeyHandle, SignPubKeyFreeAsync),
                new NativeHandle(_appPtr, secretKeyHandle, SignSecKeyFreeAsync));
        }

        private Task SignPubKeyFreeAsync(ulong pubSignKeyHandle)
        {
            return AppBindings.SignPubKeyFreeAsync(_appPtr, pubSignKeyHandle);
        }

        /// <summary>
        /// Get raw Sign Public Key
        /// </summary>
        /// <param name="pubSignKey">Sign Public Key NativeHandle</param>
        /// <returns>Raw Sign Public Key as List</returns>
        public Task<byte[]> SignPubKeyGetAsync(NativeHandle pubSignKey)
        {
            return AppBindings.SignPubKeyGetAsync(_appPtr, pubSignKey);
        }

        /// <summary>
        /// Get Sign Public Key Handle from a raw key
        /// </summary>
        /// <param name="rawPubSignKey">Raw Sign Public Key as List</param>
        /// <returns>Public Sign Key NativeHandle</returns>
        public async Task<NativeHandle> SignPubKeyNewAsync(byte[] rawPubSignKey)
        {
            var handle = await AppBindings.SignPubKeyNewAsync(_appPtr, rawPubSignKey);
            return new NativeHandle(_appPtr, handle, SignPubKeyFreeAsync);
        }

        private Task SignSecKeyFreeAsync(ulong secSignKeyHandle)
        {
            return AppBindings.SignSecKeyFreeAsync(_appPtr, secSignKeyHandle);
        }

        /// <summary>
        /// Get Raw Secret Sign Key
        /// </summary>
        /// <param name="secSignKey">Secret Sign Key NativeHandle</param>
        /// <returns>Raw Secret Sign Key as List</returns>
        public Task<byte[]> SignSecKeyGetAsync(NativeHandle secSignKey)
        {
            return AppBindings.SignSecKeyGetAsync(_appPtr, secSignKey);
        }

        /// <summary>
        /// Get New Sign Secret Key handle from a raw Sign Secret Key
        /// </summary>
        /// <param name="rawSecSignKey"></param>
        /// <returns>Secret Sign Key NativeHandle</returns>
        public async Task<NativeHandle> SignSecKeyNewAsync(byte[] rawSecSignKey)
        {
            var handle = await AppBindings.SignSecKeyNewAsync(_appPtr, rawSecSignKey);
            return new NativeHandle(_appPtr, handle, SignSecKeyFreeAsync);
        }

        /// <summary>
        /// Verify the given signed data buffer is using a Public Sign Key
        /// </summary>
        /// <param name="signedData">Data to verify signature.</param>
        /// <param name="signPubKey">Public Sign Key to verify.</param>
        /// <returns></returns>
        public Task<List<byte>> VerifyAsync(List<byte> signedData, NativeHandle signPubKey)
        {
            return AppBindings.VerifyAsync(_appPtr, signedData, signPubKey);
        }
    }
}
