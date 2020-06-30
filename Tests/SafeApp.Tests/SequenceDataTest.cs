using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SafeApp.Core;

namespace SafeApp.Tests
{
    [TestFixture]
    public class SequenceDataTest
    {
        [Test]
        public async Task SequenceDataCreateAndGetTest()
        {
            var session = await TestUtils.CreateTestApp();
            var rnd = new Random();
            var maxDataSize = 250;
            var data = TestUtils.GetRandomString(rnd.Next(maxDataSize)).ToUtfBytes();
            var typeTag = 16000UL;
            var xorUrl = await session.SequenceData.CreateSequenceDataAsync(
                data,
                null,
                typeTag,
                true);
            var (version, fetchedData) = await session.SequenceData.GetSequenceDataAsync(xorUrl);
            Assert.Zero(version);
            Assert.AreEqual(fetchedData, data);

            data = TestUtils.GetRandomString(rnd.Next(maxDataSize)).ToUtfBytes();
            var xorName = TestUtils.GenerateRandomXorName();
            xorUrl = await session.SequenceData.CreateSequenceDataAsync(
                data,
                xorName,
                typeTag,
                true);
            (version, fetchedData) = await session.SequenceData.GetSequenceDataAsync(xorUrl);
            Assert.Zero(version);
            Assert.AreEqual(data, fetchedData);

            data = TestUtils.GetRandomString(rnd.Next(maxDataSize)).ToUtfBytes();
            await session.SequenceData.AppendSequenceDataAsync(xorUrl, data);
            (version, fetchedData) = await session.SequenceData.GetSequenceDataAsync(xorUrl);
            Assert.AreEqual(1, version);
            Assert.AreEqual(data, fetchedData);
        }
    }
}
