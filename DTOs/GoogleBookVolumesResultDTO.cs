namespace DTOs;

public class GoogleBookVolumesResultDTO
{
    public int TotalItems { get; set; }
    public List<GoogleBookVolumeDTO> Items { get; set; }

}

public class GoogleBookVolumeDTO
{
    public string Id { get; set; }
    public VolumeInfo VolumeInfo { get; set; }

}

public class VolumeInfo
{
    public string Title { get; set; }
    public int PageCount { get; set; }
    public string PublishedDate { get; set; }
}
