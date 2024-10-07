using System.Diagnostics;

namespace Common;

public static class Utils
{
    public static string StudentNameFromFile(string filename)
    {
        return filename.Replace(".cs", "");
    }
}
