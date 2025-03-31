using Google.Cloud.Firestore;

[FirestoreData]
public class WalkModel
{
    [FirestoreProperty]
    public required int WalkId { get; set; } // Serial ID for the walk

    [FirestoreProperty]
    public required string WalkerName { get; set; }  // User who took the walk

    [FirestoreProperty]
    public Timestamp WalkTime { get; set; } // Firestore Timestamp (date & time)

    [FirestoreProperty]
    public bool IsPooped { get; set; }      // Whether the dog pooped

    [FirestoreProperty]
    public string? InDebtName { get; set; }  // User who owes a favor

    [FirestoreProperty]
    public bool? IsPayback { get; set; }    // Whether the debt is a payback or a favor

    [FirestoreProperty]
    public string? Notes { get; set; }       // Optional notes about the walk
}