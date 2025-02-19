using Google.Cloud.Firestore;

[FirestoreData]
public class DogModel
{
    [FirestoreProperty]
    public required string DogName { get; set; }

    [FirestoreProperty]
    public Timestamp DogBirthDate { get; set; }

    [FirestoreProperty]
    public required string DogBreed { get; set; }

    [FirestoreProperty]
    public int DogWeight { get; set; }

    [FirestoreProperty]
    public int FeedAmount { get; set; }
}
