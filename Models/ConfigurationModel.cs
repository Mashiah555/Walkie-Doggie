using Google.Cloud.Firestore;

[FirestoreData]
public class ConfigurationModel
{
    [FirestoreProperty]
    public required int LatestBuild { get; set; }
}
