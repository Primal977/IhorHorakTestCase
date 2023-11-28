namespace DTOs;

public class CategoryDTO
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<GoogleBookVolumeDTO>? GoogleBookVolumes { get; set; }
}
