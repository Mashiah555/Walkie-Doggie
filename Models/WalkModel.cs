using Google.Cloud.Firestore;

[FirestoreData]
public class WalkModel
{
    [FirestoreProperty]
    public string WalkerName { get; set; }  // User who took the walk

    [FirestoreProperty]
    public Timestamp WalkTime { get; set; } // Firestore Timestamp (date & time)

    [FirestoreProperty]
    public bool IsPooped { get; set; }      // Whether the dog pooped

    [FirestoreProperty]
    public string Notes { get; set; }       // Optional notes about the walk
}