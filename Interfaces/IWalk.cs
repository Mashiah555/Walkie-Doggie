using Google.Cloud.Firestore;

namespace Walkie_Doggie.Interfaces;

public interface IWalk : ICRUD<WalkModel, int>
{
    Task<WalkModel> GetLastWalkAsync(string? username = null);
    Task AddAsync(string walkerName, DateTime walkTime, bool isPooped,
        string? notes = null, string? inDebtName = null, bool? isPayback = false);
}
