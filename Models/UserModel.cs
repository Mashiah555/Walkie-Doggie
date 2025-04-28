using Google.Cloud.Firestore;

[FirestoreData]
public class UserModel
{
    [FirestoreProperty]
    public required string Name { get; set; }

    [FirestoreProperty]
    public int WalksCount { get; set; }

    [FirestoreProperty]
    public AppTheme Theme { get; set; }
}
