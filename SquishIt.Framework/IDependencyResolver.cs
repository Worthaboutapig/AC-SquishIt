namespace SquishIt.Framework
{
    /// <summary>
    /// Registers instances into an IoC container.
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// Registers an instance of a type into the dependency resolver.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instance">The instance to register.</param>
        void Register<T>(T instance) where T : class;
    }
}