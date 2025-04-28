using Google.Cloud.Firestore;
using Walkie_Doggie.Helpers;

namespace Walkie_Doggie.Database;

public class WalkImplementation : AbstractCRUD<WalkModel, int>, Interfaces.IWalk
{
    public WalkImplementation(FirestoreDb dbContext)
        : base(dbContext, "Walks") { }

    public async Task<WalkModel> GetLastWalkAsync(string? username = null)
    {
        IEnumerable<WalkModel> walks = await base.GetAllAsync();
        if (!string.IsNullOrEmpty(username))
            walks = walks.Where(walk => walk.WalkerName == username);

        WalkModel? latestWalk = walks
            .OrderByDescending(walk => walk.WalkTime).FirstOrDefault();

        return latestWalk ??
            throw new Exception("There are no saved walks yet!");
    }

    public override async Task AddAsync(WalkModel item)
    {
        await base.AddAsync(item);

        // Increment the walk count for the dog
        UserImplementation userImplementation = new UserImplementation(_db);
        await userImplementation.IncrementWalkCountAsync(item.WalkerName);
    }
}
