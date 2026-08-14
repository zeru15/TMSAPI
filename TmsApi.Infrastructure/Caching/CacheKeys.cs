namespace TmsApi.Infrastructure.Caching;

public static class CacheKeys
{
    private const string SchemaVersion = "v3";

    public static string Course(string code)
        => $"{SchemaVersion}:course:{code}";

    public static string CoursesAll
        => $"{SchemaVersion}:courses:all";

    public const string CoursesTag = "courses";
}