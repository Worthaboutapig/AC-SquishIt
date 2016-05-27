namespace SquishIt.Framework
{
    /// <summary>
    /// Registers default dependencies into an external IoC container.
    /// </summary>
    public interface IDefaultsResolver
    {
        /// <summary>
        /// Registers default instances of dependencies into an external dependency resolver.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver to add the defaults to.</param>
        void RegisterDefaults(IDependencyResolver dependencyResolver);
    }
}