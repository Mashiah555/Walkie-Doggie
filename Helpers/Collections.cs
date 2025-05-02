using Walkie_Doggie.Services;

namespace Walkie_Doggie.Helpers;

public static class Collections
{
    private static readonly FirebaseService _db = new FirebaseService();

    public static ICollection<string> Users { get; private set; } = new List<string>();

    static Collections()
    {
        InitializeAsync().Wait();
    }

    private static async Task InitializeAsync()
    {
        await NetworkService.NetworkCheck();

        Users = await _db.GetAllUsernamesAsync();
    }
}
