﻿using System.IO;
using NUnit.Framework;
using SquishIt.Framework.Resolvers;

namespace SquishIt.Tests
{
    [TestFixture]
    public class EmbeddedResourceResolverTest
    {
        const string cssContent = @"li {
    margin-bottom:0.1em;
    margin-left:0;
    margin-top:0.1em;
}

th {
    font-weight:normal;
    vertical-align:bottom;
}

.FloatRight {
    float:right;
}
                                
.FloatLeft {
    float:left;
}";

        [Test]
        public void CanResolveResource_Standard()
        {
            var resourcePath = "SquishIt.Tests://EmbeddedResource.Embedded.css";

            var embeddedResourceResolver = new StandardEmbeddedResourceResolver();

            var path = embeddedResourceResolver.Resolve(resourcePath);

            Assert.AreEqual(cssContent, File.ReadAllText(path));

            EmbeddedResourceResolver.ClearTempFiles();
            
            Assert.False(File.Exists(path));
        }

        [Test]
        public void CanResolveResource_Standard_Reuses_Previous_Temp_File()
        {
            var resourcePath = "SquishIt.Tests://EmbeddedResource.Embedded.css";

            var embeddedResourceResolver = new StandardEmbeddedResourceResolver();

            var path = embeddedResourceResolver.Resolve(resourcePath);
            var path2 = embeddedResourceResolver.Resolve(resourcePath);

            Assert.AreEqual(cssContent, File.ReadAllText(path));
            Assert.AreEqual(cssContent, File.ReadAllText(path2));

            Assert.AreEqual(path, path2);

            EmbeddedResourceResolver.ClearTempFiles();

            Assert.False(File.Exists(path));
        }

        [Test]
        public void CanResolveResource_Root()
        {
            var resourcePath = "SquishIt.Tests://RootEmbedded.css";

            var embeddedResourceResolver = new RootEmbeddedResourceResolver();

            var path = embeddedResourceResolver.Resolve(resourcePath);

            Assert.AreEqual(cssContent, File.ReadAllText(path));

            EmbeddedResourceResolver.ClearTempFiles();

            Assert.False(File.Exists(path));
        }

        [Test]
        public void CanResolveResource_Root_Reuses_Previous_Temp_File()
        {
            var resourcePath = "SquishIt.Tests://RootEmbedded.css";

            var embeddedResourceResolver = new RootEmbeddedResourceResolver();

            var path = embeddedResourceResolver.Resolve(resourcePath);
            var path2 = embeddedResourceResolver.Resolve(resourcePath);

            Assert.AreEqual(cssContent, File.ReadAllText(path));
            Assert.AreEqual(cssContent, File.ReadAllText(path2));

            Assert.AreEqual(path, path2);

            EmbeddedResourceResolver.ClearTempFiles();

            Assert.False(File.Exists(path));
        }
    }
}
