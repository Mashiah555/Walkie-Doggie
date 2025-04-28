using Google.Cloud.Firestore;
using Walkie_Doggie.Helpers;

namespace Walkie_Doggie.Database;

public class UserImplementation : AbstractCRUD<UserModel, string>, Interfaces.IUser
{
    public UserImplementation(FirestoreDb dbContext) 
        : base(dbContext, "Users") {}

    public async Task<IEnumerable<string>> GetAllUsernamesAsync()
    {
        return (await base.GetAllAsync())
            .Select(user => user.Name).ToList();
    }

    public Task<QuerySnapshot> GetUsersSnapshot()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> HasUserAsync(string name)
    {
        return await base.GetAsync(name) != null;
    }

    public async Task IncrementWalkCountAsync(string name)
    {
        UserModel? user = await base.GetAsync(name);
        if (user == null) return;

        user.TotalWalks++;
        await base.UpdateAsync(user);
    }
}
