using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;
using SafeApp.Utilities;

// ReSharper disable ConvertToLocalFunction
// ReSharper disable InconsistentNaming
namespace SafeApp.MData
{
    /// <summary>
    /// Helper APIs for MDataInfo. Exposes crypto functionalities to encrypt and decrypt using MData.
    /// </summary>
    [PublicAPI]
    public class MDataInfoActions
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;

        /// <summary>
        /// Initialises an MDataInfoActions object for the Session instance.
        /// </summary>
        internal MDataInfoActions()
        {
        }

        /// <summary>
        /// Decrypt the entry key/value for a Private Mutable Data.
        /// With the Encryption Key contained in a Private mdataInfo.
        /// </summary>
        /// <param name="mDataInfo">The mdataInfo of a Mutable Data.</param>
        /// <param name="cipherText">Data to be decrypted.</param>
        /// <returns>The decrypted key/value.</returns>
        public Task<List<byte>> DecryptAsync(MDataInfo mDataInfo, List<byte> cipherText)
        {
            return AppBindings.MDataInfoDecryptAsync(ref mDataInfo, cipherText);
        }

        /// <summary>
        /// Create a new mdataInfo object from the Serialised data.
        /// </summary>
        /// <param name="serialisedData">Serialised value to create new mdataInfo object.</param>
        /// <returns>New mdataInfo instance.</returns>
        public Task<MDataInfo> DeserialiseAsync(List<byte> serialisedData)
        {
            return AppBindings.MDataInfoDeserialiseAsync(serialisedData);
        }

        /// <summary>
        /// Encrypt the data with the Mutable Data's encrypt key.
        /// </summary>
        /// <param name="mDataInfo">mdataInfo</param>
        /// <param name="inputBytes">The data to be encrypted.</param>
        /// <returns>The encrypted entry key.</returns>
        public Task<List<byte>> EncryptEntryKeyAsync(MDataInfo mDataInfo, List<byte> inputBytes)
        {
            return AppBindings.MDataInfoEncryptEntryKeyAsync(ref mDataInfo, inputBytes);
        }

        /// <summary>
        /// Encrypt the entry value provided as parameter with the Encryption Key
        /// contained in a Private mdataInfo.
        /// If the Mutable Data is Public, the same (and unencrypted) value is returned.
        /// </summary>
        /// <param name="mDataInfo"></param>
        /// <param name="inputBytes">The data to be encrypted.</param>
        /// <returns>The encrypted entry value.</returns>
        public Task<List<byte>> EncryptEntryValueAsync(MDataInfo mDataInfo, List<byte> inputBytes)
        {
            return AppBindings.MDataInfoEncryptEntryValueAsync(ref mDataInfo, inputBytes);
        }

        /// <summary>
        /// Create a private Mutable Data at a specific XOR address.
        /// </summary>
        /// <param name="xorName">XOR address.</param>
        /// <param name="typeTag">The typeTag to use.</param>
        /// <param name="secEncKey">Secret Encryption Key.</param>
        /// <param name="nonce">nonce</param>
        /// <returns>Newly created mdataInfo instance.</returns>
        public Task<MDataInfo> NewPrivateAsync(byte[] xorName, ulong typeTag, byte[] secEncKey, byte[] nonce)
        {
            return AppBindings.MDataInfoNewPrivateAsync(xorName, typeTag, secEncKey, nonce);
        }

        /// <summary>
        /// Create a private Mutable Data at a random address.
        /// </summary>
        /// <param name="typeTag">The typeTag to use.</param>
        /// <returns>Newly create mdataInfo.</returns>
        public Task<MDataInfo> RandomPrivateAsync(ulong typeTag)
        {
            return AppBindings.MDataInfoRandomPrivateAsync(typeTag);
        }

        /// <summary>
        /// Create a new mdataInfo for Mutable Data at a random address with public access.
        /// </summary>
        /// <param name="typeTag">The typeTag to use.</param>
        /// <returns>Newly create mdataInfo.</returns>
        public Task<MDataInfo> RandomPublicAsync(ulong typeTag)
        {
            return AppBindings.MDataInfoRandomPublicAsync(typeTag);
        }

        /// <summary>
        /// Serialise the mdataInfo.
        /// </summary>
        /// <param name="mDataInfo">mdataInfo to be serialised.</param>
        /// <returns>List of serialised bytes.</returns>
        public async Task<List<byte>> SerialiseAsync(MDataInfo mDataInfo)
        {
            var byteArray = await AppBindings.MDataInfoSerialiseAsync(ref mDataInfo);
            return new List<byte>(byteArray);
        }
    }
}
