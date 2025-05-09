using Google.Cloud.Firestore;

namespace Walkie_Doggie.Interfaces;

public interface IUser : ICRUD<UserModel, string>
{
    Task<bool> HasUserAsync(string name);
    Task<IEnumerable<string>> GetAllUsernamesAsync();
    Task IncrementWalkCountAsync(string name);
    Task AddAsync(string name);
    Task<bool> UpdateAsync(string name, AppTheme theme);
}
