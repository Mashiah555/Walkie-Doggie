using Walkie_Doggie.Services;

namespace Walkie_Doggie.Helpers;

public static class Collections
{
    public static IEnumerable<string> Usernames { get; private set; } = new List<string>();
    public static DogModel? Dog { get; set; }
    public static bool IsMessagePopedUp { get; set; } = false;

    static Collections()
    {
        InitializeAsync().Wait();
    }

    private static async Task InitializeAsync()
    {
        Usernames = await DbService.Users.GetAllUsernamesAsync();
        Dog = await DbService.Dogs.GetAsync();
    }
}
