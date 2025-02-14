using Google.Cloud.Firestore;

[FirestoreData]
public class FeedModel
{
    [FirestoreProperty]
    public required string FeederName { get; set; }  // User who fed the dog

    [FirestoreProperty]
    public Timestamp FeedTime { get; set; } // Firestore Timestamp (date & time)

    [FirestoreProperty]
    public string? Notes { get; set; }       // Optional notes about the feeding
}