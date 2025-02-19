using Google.Cloud.Firestore;

[FirestoreData]
public class DebtModel
{
    [FirestoreProperty]
    public required string WalkerName { get; set; }

    [FirestoreProperty]
    public required string InDebtName { get; set; }
}
