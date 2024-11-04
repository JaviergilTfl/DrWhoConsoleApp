namespace DrWhoConsoleApp.Models;

public partial class Author
{
    public int AuthorId { get; set; }

    public string? AuthorName { get; set; }

    public virtual ICollection<Episode> Episodes { get; set; } = new List<Episode>();
}
