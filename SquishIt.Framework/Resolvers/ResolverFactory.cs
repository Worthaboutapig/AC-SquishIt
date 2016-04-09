using System;
using System.Collections.Generic;

namespace SquishIt.Framework.Resolvers
{
    public class ResolverFactory //: IDependencyResolver
    {
        private static readonly ITempPathProvider TempPathProvider = new TempPathProvider();

        static Dictionary<string, IFileResolver> resolvers = new Dictionary<string, IFileResolver>
                                                         {
                                                             {typeof (RootEmbeddedResourceResolver).FullName, new RootEmbeddedResourceResolver(TempPathProvider)},
                                                             {typeof (StandardEmbeddedResourceResolver).FullName, new StandardEmbeddedResourceResolver(TempPathProvider)},
                                                             {typeof (FileSystemResolver).FullName, new FileSystemResolver()},
                                                             {typeof (HttpResolver).FullName, new HttpResolver(TempPathProvider)}
                                                         };

        public static T Get<T>() where T : IFileResolver
        {
            return (T)resolvers[typeof (T).FullName];
        }
        
        internal static void SetContent(string key, IFileResolver resolver)
        {
            if (resolvers.ContainsKey(key))
            {
                resolvers[key] = resolver;
            }
            else
            {
                throw new InvalidOperationException("Invalid resolver type injected");
            }
        }

        internal static void Reset()
        {
            resolvers = new Dictionary<string, IFileResolver>
                        {
                            {typeof (RootEmbeddedResourceResolver).FullName, new RootEmbeddedResourceResolver(TempPathProvider)},
                            {typeof (StandardEmbeddedResourceResolver).FullName, new StandardEmbeddedResourceResolver(TempPathProvider)},
                            {typeof (FileSystemResolver).FullName, new FileSystemResolver()},
                            {typeof (HttpResolver).FullName, new HttpResolver(TempPathProvider)}
                        };
        }
    }
}