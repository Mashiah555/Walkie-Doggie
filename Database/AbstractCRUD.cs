using Google.Cloud.Firestore;
using Walkie_Doggie.Helpers;
using Walkie_Doggie.Interfaces;

namespace Walkie_Doggie.Database;

public abstract class AbstractCRUD<TModel, TVariable> : ICRUD<TModel, TVariable>
    where TModel : class
{
    protected readonly FirestoreDb _db;
    protected readonly string _collection;
    public AbstractCRUD(FirestoreDb dbContext, string collectionName)
    {
        _db = dbContext;
        _collection = collectionName;
    }

    public virtual async Task AddAsync(TModel item)
    {
        await NetworkService.NetworkCheck();

        await _db
            .Collection(_collection)
            .Document(Converters.ToDocumentName(item))
            .SetAsync(item);
    }

    public virtual async Task<TModel?> GetAsync(TVariable id)
    {
        await NetworkService.NetworkCheck();

        var snapshot = await _db
            .Collection(_collection)
            .Document(id!.ToString())
            .GetSnapshotAsync();

        return snapshot.Exists ? 
            snapshot.ConvertTo<TModel>() : null;
    }

    public virtual async Task<IEnumerable<TModel>> GetAllAsync()
    {
        await NetworkService.NetworkCheck();

        var snapshot = await _db
            .Collection(_collection)
            .GetSnapshotAsync();

        return snapshot == null ? new List<TModel>() : 
            snapshot.Documents.Select(doc => doc.ConvertTo<TModel>());
    }

    public virtual async Task<bool> UpdateAsync(TModel item)
    {
        try
        {
            await AddAsync(item);
        }
        catch
        {
            return false;
        }
        return true;
    }

    public virtual async Task<bool> DeleteAsync(TVariable id)
    {
        await NetworkService.NetworkCheck();

        try
        {
            await _db
                .Collection(_collection)
                .Document(id!.ToString())
                .DeleteAsync();
        }
        catch
        {
            return false;
        }
        return true;
    }

    public virtual async Task DeleteAllAsync()
    {
        foreach (var item in await GetAllAsync())
        {
            await _db
                .Collection(_collection)
                .Document(Converters.ToDocumentName(item))
                .DeleteAsync();
        }
    }
}
