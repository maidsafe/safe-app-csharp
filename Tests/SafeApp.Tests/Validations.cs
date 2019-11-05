using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.API;
using SafeApp.Core;

namespace SafeApp.Tests
{
    class Validate
    {
        public static void XorName(byte[] xorName)
        {
            Assert.IsNotNull(xorName);
            Assert.AreEqual(32, xorName.Length);
            Assert.IsFalse(Enumerable.SequenceEqual(new byte[32], xorName));
        }

        public static void NrsContainerInfo(NrsMapContainerInfo info)
        {
            Assert.AreNotEqual(DataType.SafeKey, info.DataType);
            Assert.IsNotNull(info.NrsMap);
            Assert.IsNotNull(info.PublicName);
            Assert.NotZero(info.TypeTag);
            Assert.IsNotNull(info.Version);
            Assert.IsNotNull(info.XorUrl);
            Validate.XorName(info.XorName);
        }

        public static void EnsureNullNrsContainerInfo(NrsMapContainerInfo info)
        {
            Assert.AreEqual(DataType.SafeKey, info.DataType); // since 0 is actually a data type
            Assert.IsEmpty(info.NrsMap);
            Assert.IsEmpty(info.PublicName);
            Assert.Zero(info.TypeTag);
            Assert.Zero(info.Version); // since v 0 is actually the first version
            Assert.IsEmpty(info.XorUrl);
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[32], info.XorName));
        }

        public static async Task XorUrlAsync(string xorUrl, DataType expectedDataType, ContentType expectedContentType, ulong expectedTypeTag)
        {
            var encoder = await XorEncoder.XorUrlEncoderFromUrl(xorUrl);
            Encoder(encoder, expectedDataType, expectedContentType, expectedTypeTag);
        }

        public static void Encoder(XorUrlEncoder encoder, DataType expectedDataType, ContentType expectedContentType, ulong expectedTypeTag)
        {
            Assert.AreEqual(expectedContentType, encoder.ContentType);
            Assert.Zero(encoder.ContentVersion);
            Assert.AreEqual(expectedDataType, encoder.DataType);
            Assert.AreEqual(1, encoder.EncodingVersion);

            // todo: these need to be validated once they contain the correct values

            /**
            Assert.AreEqual(string.Empty, encoder.Path);
            Assert.AreEqual(string.Empty, encoder.SubNames);
            **/
            Assert.AreEqual(expectedTypeTag, encoder.TypeTag);
            Validate.XorName(encoder.XorName);
        }

        public static async Task RawNrsMapAsync(string nrsMapRaw)
        {
            Assert.IsNotNull(nrsMapRaw);
            var nrsMap = Serialization.Deserialize<NrsMap>(nrsMapRaw);

            Assert.IsNull(nrsMap.SubNamesMap);
            /**
            foreach (var entry in nrsMap.SubNamesMap.Values)
            {
                Assert.IsNotNull(entry.SubName);
                Assert.IsNotNull(entry.SubNameRdf);
            }
            **/
            Assert.IsNotNull(nrsMap.Default);
            Assert.AreEqual(1, nrsMap.Default.Count);
            foreach (var rdf in nrsMap.Default.Values)
            {
                Assert.AreNotEqual(default(DateTime), rdf.Created);
                Assert.AreNotEqual(default(DateTime), rdf.Modified);
                Assert.IsNotNull(rdf.Link);
                await Validate.XorUrlAsync(rdf.Link, DataType.PublishedSeqAppendOnlyData, ContentType.FilesContainer, 1100);
            }
        }

        public static void TransientKeyPair(BlsKeyPair keyPair)
        {
            Assert.IsNotNull(keyPair.PK);
            Assert.IsNotNull(keyPair.SK);
            Assert.IsNotEmpty(keyPair.PK);
            Assert.IsNotEmpty(keyPair.SK);
            Assert.AreNotSame(keyPair.PK, keyPair.SK);
        }

        public static async Task PersistedKeyPair(string xorUrl, BlsKeyPair keyPair, Keys api)
        {
            Validate.TransientKeyPair(keyPair);
            await Validate.XorUrlAsync(xorUrl, DataType.SafeKey, ContentType.Raw, 0);
            var publicKey = await api.ValidateSkForUrlAsync(keyPair.SK, xorUrl);
            Assert.AreEqual(keyPair.PK, publicKey);
        }

        public static void IsEqualAmount(string expected, string actual)
        {
            Assert.AreEqual(
                decimal.Parse(expected, CultureInfo.InvariantCulture),
                decimal.Parse(actual, CultureInfo.InvariantCulture));
        }
    }
}
