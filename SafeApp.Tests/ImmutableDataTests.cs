using System;
using System.Linq;
using NUnit.Framework;
using SafeApp.Misc;

namespace SafeApp.Tests {
  [TestFixture]
  internal class ImmutableDataTests {
    public async void WriteAndReadUsingPainText() {
      var data = new byte[1024];
      new Random().NextBytes(data);
      Utils.InitialiseSessionForRandomTestApp();
      var cipherOptHandle = await CipherOpt.NewPlaintextAsync();
      var seHandle = await IData.IData.NewSelfEncryptorAsync();
      await IData.IData.WriteToSelfEncryptorAsync(seHandle, data.ToList());
      var dataMapAddress = await IData.IData.CloseSelfEncryptorAsync(seHandle, cipherOptHandle);
      var seReaderHandle = await IData.IData.FetchSelfEncryptorAsync(dataMapAddress);
      var len = await IData.IData.SizeAsync(seReaderHandle);
      var readData = await IData.IData.ReadFromSelfEncryptorAsync(seReaderHandle, 0, len);
      Assert.AreEqual(data, readData);
    }
  }
}
