namespace Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool Implements<T>(this System.Type type) =>
            typeof(T).IsAssignableFrom(type);
    }
}