using DAL.Abstractions;

namespace DAL.Entities;

public class Category : IIdEntity<int>
{
    public Category()
    {
        BookVolumes = new List<BookVolume>();
    }

    public int Id { get; set; }
    public string Name { get; set; }

    public virtual List<BookVolume> BookVolumes { get; set; }
}
