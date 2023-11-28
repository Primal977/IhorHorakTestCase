using DTOs;

namespace API.Abstractions;

public interface IVolumeAPIClient
{
    Task<GoogleBookVolumesResultDTO> FindVolumesAsync(string query, int startIndex = 0, int maxResults = 10);
    Task<GoogleBookVolumeDTO> GetVolumeById(string id);
}
