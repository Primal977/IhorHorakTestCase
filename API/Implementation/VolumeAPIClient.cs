using API.Abstractions;
using Common;
using DTOs;

namespace API.Implementation
{
    public class VolumeAPIClient : BaseAPIClient<VolumeAPIClient>, IVolumeAPIClient
    {
        private const string volumeApiUrl = "https://www.googleapis.com";

        public VolumeAPIClient(HttpClient client) : base(client, volumeApiUrl)
        {

        }
        public async Task<GoogleBookVolumesResultDTO> FindVolumesAsync(string query, int startIndex = 0, int maxResults = 10)
        {
            return await Get<GoogleBookVolumesResultDTO>($"{volumeApiUrl}/books/v1/volumes?q={query}&startIndex={startIndex}&maxResults={maxResults}");
        }

        public async Task<GoogleBookVolumeDTO> GetVolumeById(string id)
        {
            return await Get<GoogleBookVolumeDTO>($"{volumeApiUrl}/books/v1/volumes/{id}");
        }
    }
}
