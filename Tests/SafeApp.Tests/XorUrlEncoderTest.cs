using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.API;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    public class XorUrlEncoderTest
    {
        [Test]
        public async Task XorUrlEncoderTestAsync()
        {
            var subNames = new List<string>() { "subname1", "subname2" };
            var xorName = TestUtils.GenerateRandomXorName();
            var typeTag = 16000UL;
            var contentType = ContentType.Wallet;
            var dataType = DataType.UnpublishedImmutableData;
            var encodedString = await XorEncoder.EncodeAsync(
                xorName,
                null,
                typeTag,
                dataType,
                contentType,
                null,
                subNames,
                null,
                null,
                0,
                "base32z");
            Assert.IsNotNull(encodedString);
            Assert.IsTrue(encodedString.StartsWith("safe://", StringComparison.Ordinal));
            Assert.IsTrue(encodedString.Contains("subname1"));
            Assert.IsTrue(encodedString.Contains("subname2"));

            var safeUrl = await XorEncoder.NewSafeUrlAsync(
                xorName,
                null,
                typeTag,
                dataType,
                contentType,
                null,
                subNames,
                null,
                null,
                0);
            Assert.AreEqual(xorName, safeUrl.XorName);
            Assert.AreEqual(subNames, safeUrl.SubNamesList);
            Assert.AreEqual("subname1.subname2", safeUrl.SubNames);
            Validate.Encoder(safeUrl, dataType, contentType, typeTag);

            var parsedEncoder = await XorEncoder.SafeUrlFromUrl(encodedString);
            Assert.AreEqual(xorName, parsedEncoder.XorName);
            Validate.Encoder(parsedEncoder, dataType, contentType, typeTag);
            Assert.AreEqual(subNames, parsedEncoder.SubNamesList);
            Assert.AreEqual(typeTag, parsedEncoder.TypeTag);
            Assert.AreEqual(contentType, parsedEncoder.ContentType);
            Assert.AreEqual(0, parsedEncoder.ContentVersion);
        }

        [Test]
        public async Task EncodeDataTypesTestAsync()
        {
            var safeKeyXorName = TestUtils.GenerateRandomXorName();
            var encodedSafeKeyXorUrl = await XorEncoder.EncodeSafeKeyAsync(safeKeyXorName, "base32z");
            Assert.IsNotNull(encodedSafeKeyXorUrl);

            var iDataXorName = TestUtils.GenerateRandomXorName();
            var encodedIDataXorUrl = await XorEncoder.EncodeImmutableDataAsync(
                iDataXorName,
                ContentType.Raw,
                "base32z");
            Assert.IsNotNull(encodedIDataXorUrl);

            var typeTag = 16000UL;
            var mDataXorName = TestUtils.GenerateRandomXorName();
            var encodedMDataXorUrl = await XorEncoder.EncodeMutableDataAsync(
                mDataXorName,
                typeTag,
                ContentType.Raw,
                "base32z");
            Assert.IsNotNull(encodedMDataXorUrl);
            await Validate.XorUrlAsync(encodedMDataXorUrl, mDataXorName, ContentType.Raw, typeTag);

            var aDataXorName = TestUtils.GenerateRandomXorName();
            var encodedADataXorUrl = await XorEncoder.EncodeAppendOnlyDataAsync(
                aDataXorName,
                typeTag,
                ContentType.FilesContainer,
                "base32z");
            Assert.IsNotNull(encodedADataXorUrl);
            await Validate.XorUrlAsync(encodedADataXorUrl, aDataXorName, ContentType.FilesContainer, typeTag);
        }
    }
}
