namespace diegodrf.BuddyInjector.ExtensionMethods;

public static class TypeReader
{
    public static string GetFormattedTypeName(this Type t)
    {
        if (!t.GenericTypeArguments.Any()) return t.Name;

        var genericTypes = t.GenericTypeArguments.Select(x => x.Name);
        return $"{t.Name}<{string.Join(", ", genericTypes)}>";
    }
}