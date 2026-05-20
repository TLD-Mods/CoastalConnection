using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CoastalConnection;

public static class EmbeddedFiles
{
    private const string FilePrefix = "CoastalConnection.";

    private static Assembly Assembly => typeof(EmbeddedFiles).Assembly;

    public static IReadOnlyList<string> Names { get; } = Assembly
        .GetManifestResourceNames()
        .Select(t => t.Substring(FilePrefix.Length, t.Length - FilePrefix.Length - 3))
        .ToArray();

    /// <summary>
    /// Load a file from the embedded resources.
    /// </summary>
    /// <param name="fileName">The name of the file with its extension.</param>
    /// <returns>The loaded data.</returns>
    public static byte[] Load(string fileName)
    {
        if (TryLoad(fileName, out byte[]? data))
        {
            return data;
        }
        throw new FileNotFoundException(fileName);
    }

    public static bool TryLoad(string fileName, [NotNullWhen(true)] out byte[]? data)
    {
        using Stream? stream = Assembly.GetManifestResourceStream(FilePrefix + fileName);
        if (stream is null)
        {
            data = null;
            return false;
        }

        MemoryStream memoryStream = new();
        stream.CopyTo(memoryStream);
        data = memoryStream.ToArray();
        return true;
    }
}