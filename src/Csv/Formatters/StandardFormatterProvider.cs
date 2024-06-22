namespace Csv.Formatters;

public sealed class StandardFormatterProvider : ICsvFormatterProvider
{
    public static readonly StandardFormatterProvider Instance = new();

    StandardFormatterProvider()
    {
        Cache<sbyte>.value = SByteFormatter.Instance;
        Cache<byte>.value = ByteFormatter.Instance;
        Cache<short>.value = Int16Formatter.Instance;
        Cache<ushort>.value = UInt16Formatter.Instance;
        Cache<int>.value = Int32Formatter.Instance;
        Cache<uint>.value = UInt32Formatter.Instance;
        Cache<long>.value = Int64Formatter.Instance;
        Cache<ulong>.value = UInt64Formatter.Instance;
        Cache<string?>.value = StringFormatter.Instance;
        Cache<DateTime>.value = new DateTimeFormatter();
        Cache<TimeSpan>.value = new TimeSpanFormatter();
        Cache<Guid>.value = new GuidFormatter();
    }

    static class Cache<T>
    {
        public static ICsvFormatter<T>? value;
    }

    public ICsvFormatter<T>? GetFormatter<T>()
    {
        if (Cache<T>.value != null) goto RETURN;

        if (typeof(T).IsEnum)
        {
            Cache<T>.value = (ICsvFormatter<T>)Activator.CreateInstance(typeof(EnumFormatter<>).MakeGenericType(typeof(T)))!;
            goto RETURN;
        }

        if (typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            Cache<T>.value = (ICsvFormatter<T>)Activator.CreateInstance(typeof(NullableFormatter<>).MakeGenericType(typeof(T)))!;
            goto RETURN;
        }

    RETURN:
        return Cache<T>.value;
    }
}