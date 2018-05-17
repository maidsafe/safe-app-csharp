using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.Misc {

  public class CipherOpt {
    private static readonly IAppBindings AppBindings = AppResolver.Current;
    private readonly SafeAppPtr _appPtr;

    internal CipherOpt(SafeAppPtr appPtr) {
      _appPtr = appPtr;
    }

    private Task FreeAsync(ulong cipherOptHandle) {
      return AppBindings.CipherOptFreeAsync(_appPtr, cipherOptHandle);
    }

    /// <summary>
    ///   Create a new Asymmetric CipherOpt handle
    /// </summary>
    /// <param name="encPubKeyH">NativeHandle</param>
    /// <returns>AsymmetricCipherOpt NativeHandle</returns>
    public async Task<NativeHandle> NewAsymmetricAsync(NativeHandle encPubKeyH) {
      var cipherOptH = await AppBindings.CipherOptNewAsymmetricAsync(_appPtr, encPubKeyH);
      return new NativeHandle(_appPtr, cipherOptH, FreeAsync);
    }

    /// <summary>
    ///   Create a new Plain text CipherOpt handle
    /// </summary>
    /// <returns>Plain text NativeHandle</returns>
    public async Task<NativeHandle> NewPlaintextAsync() {
      var cipherOptH = await AppBindings.CipherOptNewPlaintextAsync(_appPtr);
      return new NativeHandle(_appPtr, cipherOptH, FreeAsync);
    }

    /// <summary>
    ///   Create a new Symmetric CipherOpt handle
    /// </summary>
    /// <returns>Symmetric NativeHandle</returns>
    public async Task<NativeHandle> NewSymmetricAsync() {
      var cipherOptH = await AppBindings.CipherOptNewSymmetricAsync(_appPtr);
      return new NativeHandle(_appPtr, cipherOptH, FreeAsync);
    }
  }
}
