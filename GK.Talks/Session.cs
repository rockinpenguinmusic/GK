namespace GK.Talks
{
    public record Session(string Title, string Description)
    {
        public bool Approved { get; set; } = false;
    };
}