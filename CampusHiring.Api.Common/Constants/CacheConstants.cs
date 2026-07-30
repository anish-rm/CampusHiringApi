namespace CampusHiring.Api.Common.Constants;

public static class CacheConstants
{
    public const string AuthenticatedUserCachingPolicy = "AuthenticatedUserCachingPolicy";
    public const string AuthenticatedUserCachingPolicyTag = "auth-";

    public const int ShortDuration = 60;
    public const int MediumDuration = 300;
    public const int LongDuration = 900;
}
