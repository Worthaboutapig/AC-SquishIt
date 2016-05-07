using System;
using System.Threading;
using System.Web;
using Moq;
using NUnit.Framework;
using SquishIt.Framework.Utilities;

namespace SquishIt.Tests
{
    [TestFixture]
    public class CriticalRenderingSectionTest
    {
        [TestCase(AspNetHostingPermissionLevel.Unrestricted)]
        public void UseMutex(AspNetHostingPermissionLevel permissionLevel)
        {
            var testDir = Guid.NewGuid().ToString();

            var trustLevel = new Mock<ITrustLevel>();
            trustLevel.SetupGet(tl => tl.IsFullTrust).Returns(() => permissionLevel == AspNetHostingPermissionLevel.Unrestricted);

            var filePathMutextProvider = new Mock<IFilePathMutexProvider>();
            filePathMutextProvider.Setup(mp => mp.GetMutexForPath(testDir)).Returns(new Mutex());

            using(new CriticalRenderingSection(trustLevel.Object, filePathMutextProvider.Object, testDir))
            {
                //do something
            }

            trustLevel.VerifyAll();
            filePathMutextProvider.VerifyAll();
        }

        [TestCase(AspNetHostingPermissionLevel.High)]
        [TestCase(AspNetHostingPermissionLevel.Medium)]
        [TestCase(AspNetHostingPermissionLevel.Low)]
        [TestCase(AspNetHostingPermissionLevel.Minimal)]
        [TestCase(AspNetHostingPermissionLevel.None)]
        public void DontUseMutex(AspNetHostingPermissionLevel permissionLevel)
        {
            var testDir = Guid.NewGuid().ToString();

            var trustLevel = new Mock<ITrustLevel>();
            trustLevel.SetupGet(tl => tl.IsFullTrust).Returns(() => permissionLevel == AspNetHostingPermissionLevel.Unrestricted);
            //just want to be sure nothing is called on this
            var filePathMutextProvider = new Mock<IFilePathMutexProvider>(MockBehavior.Strict);

            using(new CriticalRenderingSection(trustLevel.Object,filePathMutextProvider.Object, testDir))
            {
                //do something
            }

            trustLevel.VerifyAll();
            filePathMutextProvider.VerifyAll();
        }
    }
}
