using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.Misc {
  [PublicAPI]
  public class Crypto {
    private static readonly IAppBindings AppBindings = AppResolver.Current;
    private SafeAppPtr _appPtr;

    internal Crypto(SafeAppPtr appPtr) {
      _appPtr = appPtr;
    }

    public static Task<byte[]> GenerateNonceAsync()
    {
      return AppBindings.GenerateNonceAsync();
    }

    public static Task<List<byte>> Sha3HashAsync(List<byte> source)
    {
      return AppBindings.Sha3HashAsync(source);
    }

    /// <summary>
    ///   Get App's Public Sign Key
    /// </summary>
    /// <returns>App's Public Sign Key NativeHandle</returns>
    public async Task<NativeHandle> AppPubEncKeyAsync() {
      var appPubSignKeyH = await AppBindings.AppPubEncKeyAsync(_appPtr);
      return new NativeHandle(_appPtr, appPubSignKeyH, EncPubKeyFreeAsync);
    }

    /// <summary>
    ///   Get App's Public Sign Key
    /// </summary>
    /// <returns>App's Public Sign Key NativeHandle</returns>
    public async Task<NativeHandle> AppPubSignKeyAsync() {
      var appPubEncKeyH = await AppBindings.AppPubSignKeyAsync(_appPtr);
      return new NativeHandle(_appPtr, appPubEncKeyH, SignPubKeyFreeAsync);
    }

    public Task<List<byte>> DecryptAsync(List<byte> cipherText, NativeHandle encPubKey, NativeHandle encSecKey) {
      return AppBindings.DecryptAsync(_appPtr, cipherText, encPubKey, encSecKey);
    }

    /// <summary>
    ///   Decrypt cipher text from a Sender
    /// </summary>
    /// <param name="cipherText">Cipher Text to be decrypted</param>
    /// <param name="pkHandle">Sender's Encrypt Public Key</param>
    /// <param name="skHandle">Receiver's Encrypt Secret Key</param>
    /// <returns>Decrypted data</returns>
    public Task<List<byte>> DecryptSealedBoxAsync(List<byte> cipherText, NativeHandle pkHandle, NativeHandle skHandle) {
      return AppBindings.DecryptSealedBoxAsync(_appPtr, cipherText, pkHandle, skHandle);
    }

    /// <summary>
    ///   Generate New Encrypt Key Pair
    /// </summary>
    /// <returns>(Encrypt Public Key, Encrypt Secret Key)</returns>
    public async Task<(NativeHandle, NativeHandle)> EncGenerateKeyPairAsync() {
      var (encPubKeyH, encSecKeyH) = await AppBindings.EncGenerateKeyPairAsync(_appPtr);
      return (new NativeHandle(_appPtr, encPubKeyH, EncPubKeyFreeAsync), new NativeHandle(_appPtr, encSecKeyH, EncSecretKeyFreeAsync));
    }

    /// <summary>
    ///   Free Encrypt Public Key Handle
    /// </summary>
    /// <param name="encPubKeyH"></param>
    /// <returns></returns>
    private Task EncPubKeyFreeAsync(ulong encPubKeyH) {
      return AppBindings.EncPubKeyFreeAsync(_appPtr, encPubKeyH);
    }

    public Task<byte[]> EncPubKeyGetAsync(NativeHandle encPubKeyH) {
      return AppBindings.EncPubKeyGetAsync(_appPtr, encPubKeyH);
    }

    public async Task<NativeHandle> EncPubKeyNewAsync(byte[] asymPublicKeyBytes) {
      var encryptPubKeyH = await AppBindings.EncPubKeyNewAsync(_appPtr, asymPublicKeyBytes);
      return new NativeHandle(_appPtr, encryptPubKeyH, EncPubKeyFreeAsync);
    }

    public Task<List<byte>> EncryptAsync(List<byte> data, NativeHandle encPubKey, NativeHandle encSecKey) {
      return AppBindings.EncryptAsync(_appPtr, data, encPubKey, encSecKey);
    }

    public Task<List<byte>> EncryptSealedBoxAsync(List<byte> inputData, NativeHandle pkHandle) {
      return AppBindings.EncryptSealedBoxAsync(_appPtr, inputData, pkHandle);
    }

    private Task EncSecretKeyFreeAsync(ulong encSecKeyH) {
      return AppBindings.EncSecretKeyFreeAsync(_appPtr, encSecKeyH);
    }

    public Task<byte[]> EncSecretKeyGetAsync(NativeHandle encSecKeyH) {
      return AppBindings.EncSecretKeyGetAsync(_appPtr, encSecKeyH);
    }

    public async Task<NativeHandle> EncSecretKeyNewAsync(byte[] asymSecKeyBytes) {
      var encSecKeyH = await AppBindings.EncSecretKeyNewAsync(_appPtr, asymSecKeyBytes);
      return new NativeHandle(_appPtr, encSecKeyH, EncSecretKeyFreeAsync);
    }

    public Task<List<byte>> SignAsync(List<byte> data, NativeHandle signSecKey) {
      return AppBindings.SignAsync(_appPtr, data, signSecKey);
    }

    /// <summary>
    ///   Generate a new Sign Key Pair
    /// </summary>
    /// <returns>Tuple of Sign Public Key NativeHandle and Sign Secret Key NativeHandle</returns>
    public async Task<(NativeHandle, NativeHandle)> SignGenerateKeyPairAsync() {
      var (publicKeyHandle, secretKeyHandle) = await AppBindings.SignGenerateKeyPairAsync(_appPtr);
      return (new NativeHandle(_appPtr, publicKeyHandle, SignPubKeyFreeAsync), new NativeHandle(
        _appPtr,
        secretKeyHandle,
        SignSecKeyFreeAsync));
    }

    private Task SignPubKeyFreeAsync(ulong pubSignKeyHandle) {
      return AppBindings.SignPubKeyFreeAsync(_appPtr, pubSignKeyHandle);
    }

    /// <summary>
    ///   Get raw Sign Public Key
    /// </summary>
    /// <param name="pubSignKey">Sign Public Key NativeHandle</param>
    /// <returns>Raw Sign Public Key as List</returns>
    public Task<byte[]> SignPubKeyGetAsync(NativeHandle pubSignKey) {
      return AppBindings.SignPubKeyGetAsync(_appPtr, pubSignKey);
    }

    /// <summary>
    ///   Get Sign Public Key Handle from a raw key
    /// </summary>
    /// <param name="rawPubSignKey">Raw Sign Public Key as List</param>
    /// <returns>Public Sign Key NativeHandle</returns>
    public async Task<NativeHandle> SignPubKeyNewAsync(byte[] rawPubSignKey) {
      var handle = await AppBindings.SignPubKeyNewAsync(_appPtr, rawPubSignKey);
      return new NativeHandle(_appPtr, handle, SignPubKeyFreeAsync);
    }

    private Task SignSecKeyFreeAsync(ulong secSignKeyHandle) {
      return AppBindings.SignSecKeyFreeAsync(_appPtr, secSignKeyHandle);
    }

    /// <summary>
    ///   Get Raw Secret Sign Key
    /// </summary>
    /// <param name="secSignKey">Secret Sign Key NativeHandle</param>
    /// <returns>Raw Secret Sign Key as List</returns>
    public Task<byte[]> SignSecKeyGetAsync(NativeHandle secSignKey) {
      return AppBindings.SignSecKeyGetAsync(_appPtr, secSignKey);
    }

    /// <summary>
    ///   Get New Sign Secret Key handle from a raw Sign Secret Key
    /// </summary>
    /// <param name="rawSecSignKey"></param>
    /// <returns>Secret Sign Key NativeHandle</returns>
    public async Task<NativeHandle> SignSecKeyNewAsync(byte[] rawSecSignKey) {
      var handle = await AppBindings.SignSecKeyNewAsync(_appPtr, rawSecSignKey);
      return new NativeHandle(_appPtr, handle, SignSecKeyFreeAsync);
    }

    public Task<List<byte>> VerifyAsync(List<byte> signedData, NativeHandle signPubKey) {
      return AppBindings.VerifyAsync(_appPtr, signedData, signPubKey);
    }
  }
}
