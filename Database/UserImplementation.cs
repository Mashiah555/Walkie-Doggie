using Google.Cloud.Firestore;
using Walkie_Doggie.Helpers;
using static Java.Util.Jar.Attributes;

namespace Walkie_Doggie.Database;

class UserImplementation : Interfaces.IUser
{
    public async Task AddAsync(string name)
    {
        await AddAsync(new UserModel { Name = name});
    }
    public async Task AddAsync(UserModel user)
    {
        await NetworkService.NetworkCheck();

        await _firestoreDb!
            .Collection(UsersCollection)
            .Document(user.Name)
            .SetAsync(user);
    }

    public Task DeleteAll()
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<string>> GetAllUsernamesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UserModel?> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public DocumentReference GetUserReference(string name)
    {
        throw new NotImplementedException();
    }

    public Task<QuerySnapshot> GetUsersSnapshot()
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasUserAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task IncrementTotalWalks(string name)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(UserModel item)
    {
        throw new NotImplementedException();
    }
}
