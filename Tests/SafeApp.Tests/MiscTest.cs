using NUnit.Framework;
using SafeApp.MockAuthBindings;

namespace SafeApp.Tests
{
    [TestFixture]
    internal class MiscTest
    {
        [Test]
        public void IsMockAuthenticationBuildTest()
            => Assert.That(Authenticator.IsMockBuild(), Is.True);

        [Test]
        public void IsMockSafeAppBuildTest()
            => Assert.That(Session.IsMockBuild(), Is.True);
    }
}
