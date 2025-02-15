namespace CaseStudy.Api.Helpers;

public class PathHelper
{
    public static string GetFullPath(string path, string? basePath = null)
    {
        if (String.IsNullOrWhiteSpace(basePath))
        {
            basePath = AppContext.BaseDirectory;
        }
        var p = Path.GetFullPath(Path.Combine(basePath, path));
        return p.Replace("\\","/");
    }
}
