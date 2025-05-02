using Google.Cloud.Firestore;

[FirestoreData]
public class FeedModel
{
    [FirestoreProperty]
    public required string FeederName { get; set; }  // User who fed the dog

    [FirestoreProperty]
    public Timestamp FeedTime { get; set; } // Firestore Timestamp (date & time)

    [FirestoreProperty]
    public int FeedAmount { get; set; }      // Amount of food given

    [FirestoreProperty]
    public string? Notes { get; set; }       // Optional notes about the feeding

    public static FeedModel Default => new FeedModel
    {
        FeederName = string.Empty,
    };
}