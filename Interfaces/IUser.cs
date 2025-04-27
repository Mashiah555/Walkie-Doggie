using Google.Cloud.Firestore;

namespace Walkie_Doggie.Interfaces;

public interface IUser : ICRUD<UserModel, string>
{
    //Task AddAsync(string name);
    Task<bool> HasUserAsync(string name);
    Task<IEnumerable<string>> GetAllUsernamesAsync();

    // private helper methods:
    Task IncrementTotalWalks(string name);
    DocumentReference GetUserReference(string name);
    Task<QuerySnapshot> GetUsersSnapshot();
}
