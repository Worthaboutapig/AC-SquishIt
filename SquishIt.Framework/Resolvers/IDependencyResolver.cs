namespace SquishIt.Framework.Resolvers
{
    public interface IDependencyResolver
    {
        IFileResolver Get<T>() where T : IFileResolver;
    }
}