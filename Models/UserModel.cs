using Google.Cloud.Firestore;

[FirestoreData]
public class UserModel
{
    [FirestoreProperty]
    public required string Name { get; set; }

    [FirestoreProperty]
    public int TotalWalks { get; set; }
}
