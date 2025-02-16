namespace Walkie_Doggie.Helpers;

public sealed record Configs
{
    public string DogName {  get { return "לואי"; } }
    public DateTime BirthDate { get { return new DateTime(2024, 1, 13); } }
    public string Breed { get { return "שיצו"; } }
}
