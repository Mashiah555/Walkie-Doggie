using Google.Cloud.Firestore;

[FirestoreData]
public class DogModel
{
    [FirestoreProperty]
    public required string DogName { get; init; }

    [FirestoreProperty]
    public Timestamp DogBirthdate { get; set; }

    [FirestoreProperty]
    public required string DogBreed { get; set; }

    [FirestoreProperty]
    public int DogWeight { get; set; }

    [FirestoreProperty]
    public int DefaultFeedAmount { get; set; }

    [FirestoreProperty]
    public int TotalWalks { get; set; }
}
