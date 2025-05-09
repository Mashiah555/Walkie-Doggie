﻿using Google.Cloud.Firestore;
using Walkie_Doggie.Helpers;
using Walkie_Doggie.Interfaces;

namespace Walkie_Doggie.Database;

public class UserImplementation : GenericCRUD<UserModel, string>, Interfaces.IUser
{
    public UserImplementation(FirestoreDb dbContext) 
        : base(dbContext, "Users") {}

    public async Task<IEnumerable<string>> GetAllUsernamesAsync()
    {
        return (await base.GetAllAsync())
            .Select(user => user.Name).ToList();
    }

    public async Task<bool> HasUserAsync(string name)
    {
        return await base.GetAsync(name) != null;
    }

    public async Task IncrementWalkCountAsync(string name)
    {
        UserModel? user = await base.GetAsync(name);
        if (user == null) return;

        user.WalksCount++;
        await base.UpdateAsync(user);
    }

    public async Task AddAsync(string name)
    {
        await base.AddAsync(new UserModel
        {
            Name = name,
            WalksCount = 0,
            Theme = AppTheme.Unspecified
        });
    }

    public async Task<bool> UpdateAsync(string name, AppTheme theme)
    {
        UserModel? user = await GetAsync(name);
        if (user == null)
            return false;

        user.Theme = theme;
        await base.UpdateAsync(user);
        return true;
    }
}
