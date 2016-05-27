//using System.Web;
//using Moq;
//using NUnit.Framework;
//using SquishIt.Framework.Utilities;
//using SquishIt.Framework.Web;

//namespace SquishIt.Tests
//{
//    [TestFixture]
//    public class DebugStatusReaderTest
//    {
//        [Test]
//        public void ForceRelease()
//        {
//            //shouldn't touch anything on these
//            var httpContext = new Mock<IHttpContext>(MockBehavior.Strict);
//            var machineConfigReader = new Mock<IMachineConfigReader>(MockBehavior.Strict);
//            var trustLevel = new TrustLevel();

//            var reader = new DebugStatusReader(machineConfigReader.Object, httpContext.Object, trustLevel);
//            reader.ForceRelease();
//            Assert.IsFalse(reader.IsDebuggingEnabled());

//            httpContext.VerifyAll();
//            machineConfigReader.VerifyAll();
//        }

//        [Test]
//        public void ForceDebug()
//        {
//            //shouldn't touch anything on these
//            var httpContext = new Mock<IHttpContext>(MockBehavior.Strict);
//            var machineConfigReader = new Mock<IMachineConfigReader>(MockBehavior.Strict);
//            var trustLevel = new TrustLevel();

//            var reader = new DebugStatusReader(machineConfigReader.Object, httpContext.Object, trustLevel);
//            reader.ForceDebug();
//            Assert.IsTrue(reader.IsDebuggingEnabled());

//            httpContext.VerifyAll();
//            machineConfigReader.VerifyAll();
//        }

//        [TestCase(false)]
//        [TestCase(true)]
//        public void Predicate(bool predicateReturn)
//        {
//            //shouldn't touch anything on these
//            var httpContext = new Mock<IHttpContext>(MockBehavior.Strict);
//            var machineConfigReader = new Mock<IMachineConfigReader>(MockBehavior.Strict);
//            var trustLevel = new TrustLevel();
//            var reader = new DebugStatusReader(machineConfigReader.Object, httpContext.Object, trustLevel);
//            reader.ForceRelease();
//            Assert.AreEqual(predicateReturn, reader.IsDebuggingEnabled(() => predicateReturn));

//            httpContext.VerifyAll();
//            machineConfigReader.VerifyAll();
//        }

//        [Test]
//        public void NullHttpContext()
//        {
//            var machineConfigReader = new Mock<IMachineConfigReader>(MockBehavior.Strict);
//            var trustLevel = new TrustLevel();

//            var reader = new DebugStatusReader(machineConfigReader.Object, null, trustLevel);
//            Assert.IsFalse(reader.IsDebuggingEnabled());

//            machineConfigReader.VerifyAll();
//        }

//        [Test]
//        public void DebuggingExplicitlyDisabledInConfiguration()
//        {
//            var httpContext = new Mock<IHttpContext>();
//            var machineConfigReader = new Mock<IMachineConfigReader>(MockBehavior.Strict);
//            var trustLevel = new TrustLevel();

//            httpContext.SetupGet(hc => hc.IsDebuggingEnabled).Returns(false);

//            var reader = new DebugStatusReader(machineConfigReader.Object, httpContext.Object, trustLevel);
//            Assert.IsFalse(reader.IsDebuggingEnabled());

//            machineConfigReader.VerifyAll();
//        }

//        [TestCase(true)]
//        [TestCase(false)]
//        public void DebuggingExplicitlyEnabledInConfiguration_CheckMachineConfig(bool configReaderValue)
//        {
//            var httpContext = new Mock<IHttpContext>();
//            var machineConfigReader = new Mock<IMachineConfigReader>(MockBehavior.Strict);
//            var trustLevel = new TrustLevel();

//            httpContext.SetupGet(hc => hc.IsDebuggingEnabled).Returns(true);
//            machineConfigReader.SetupGet(mcr => mcr.IsNotRetailDeployment).Returns(configReaderValue);

//            var reader = new DebugStatusReader(machineConfigReader.Object, httpContext.Object, trustLevel);
//            Assert.AreEqual(configReaderValue, reader.IsDebuggingEnabled());

//            machineConfigReader.VerifyAll();
//        }

//        [TestCase(AspNetHostingPermissionLevel.Medium)]
//        [TestCase(AspNetHostingPermissionLevel.Low)]
//        [TestCase(AspNetHostingPermissionLevel.Minimal)]
//        [TestCase(AspNetHostingPermissionLevel.None)]
//        public void IgnoreMachineConfigReader_Untrusted(AspNetHostingPermissionLevel permissionLevel)
//        {
//            var httpContext = new Mock<IHttpContext>(MockBehavior.Strict);
//            httpContext.SetupGet(hc => hc.IsDebuggingEnabled).Returns(true);

//            var trustLevel = new Mock<ITrustLevel>(MockBehavior.Strict);
//            trustLevel.SetupGet(tl => tl.CurrentTrustLevel).Returns(permissionLevel);
//            trustLevel.SetupGet(tl => tl.IsFullTrust).Returns(() => permissionLevel == AspNetHostingPermissionLevel.Unrestricted);
//            trustLevel.SetupGet(tl => tl.IsHighOrUnrestrictedTrust).Returns(() => permissionLevel == AspNetHostingPermissionLevel.High || permissionLevel == AspNetHostingPermissionLevel.Unrestricted);

//            //shouldn't touch anything on this
//            var machineConfigReader = new Mock<IMachineConfigReader>(MockBehavior.Strict);

//            var reader = new DebugStatusReader(machineConfigReader.Object, httpContext.Object, trustLevel.Object);
//            Assert.IsTrue(reader.IsDebuggingEnabled());

//            machineConfigReader.VerifyAll();
//        }

//        [TestCase(AspNetHostingPermissionLevel.Unrestricted)]
//        [TestCase(AspNetHostingPermissionLevel.High)]
//        public void UseMachineConfigReader_Trusted(AspNetHostingPermissionLevel permissionLevel)
//        {
//            var httpContext = new Mock<IHttpContext>(MockBehavior.Strict);
//            httpContext.SetupGet(hc => hc.IsDebuggingEnabled).Returns(true);

//            var trustLevel = new Mock<ITrustLevel>(MockBehavior.Strict);
//            trustLevel.SetupGet(tl => tl.CurrentTrustLevel).Returns(permissionLevel);
//            trustLevel.SetupGet(tl => tl.IsFullTrust).Returns(() => permissionLevel == AspNetHostingPermissionLevel.Unrestricted);
//            trustLevel.SetupGet(tl => tl.IsHighOrUnrestrictedTrust).Returns(() => permissionLevel == AspNetHostingPermissionLevel.High || permissionLevel == AspNetHostingPermissionLevel.Unrestricted);

//            var machineConfigReader = new Mock<IMachineConfigReader>(MockBehavior.Strict);
//            machineConfigReader.SetupGet(tl => tl.IsNotRetailDeployment).Returns(false);

//            var reader = new DebugStatusReader(machineConfigReader.Object, httpContext.Object, trustLevel.Object);
//            Assert.IsFalse(reader.IsDebuggingEnabled());

//            machineConfigReader.VerifyAll();
//        }
//    }
//}