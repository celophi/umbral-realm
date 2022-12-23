namespace UmbralRealm.Core.Utilities
{
    /// <summary>
    /// Used to wrap <see cref="Activator"/>
    /// </summary>
    public class ActivatorWrapper
    {
        /// <summary>
        /// Creates an instance of the specified type using that type's parameterless constructor.
        /// </summary>
        /// <param name="type">The type of object to create.</param>
        /// <returns></returns>
        public virtual object? CreateInstance(Type type) => Activator.CreateInstance(type);
    }
}
