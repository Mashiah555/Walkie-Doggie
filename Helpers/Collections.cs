using Walkie_Doggie.Services;

namespace Walkie_Doggie.Helpers;

public static class Collections
{
    public static IEnumerable<string> Usernames { get; private set; } = new List<string>();
    public static bool IsMessagePopedUp { get; set; } = false;

    //private static DogModel _dog;
    //public static DogModel Dog
    //{
    //    get => _dog;
    //    set => _dog = value;
    //}

    static Collections()
    {
        InitializeAsync().Wait();
    }

    private static async Task InitializeAsync()
    {
        Usernames = await DbService.Users.GetAllUsernamesAsync();
        //Dog = await DbService.Dogs.GetAsync();
    }
}
