using Microsoft.AspNetCore.Http.HttpResults;

namespace WebShop.Web;

public class CatalogApiClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<CatalogItem>> GetAllItemsAsync()
    {
        var response = await _httpClient.GetAsync("/api/Catalog");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CatalogItem>>() ?? [];
    }

    public async Task<CatalogItem?> GetItemByIdAsync(int itemId)
    {
        var response = await _httpClient.GetAsync($"/api/Catalog/{itemId}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CatalogItem>();
        }
        return null;
    }

    public async Task<FileContentHttpResult> GetImageUrlAsync(int itemId)
    {
        var response = await _httpClient.GetAsync($"/api/Catalog/{itemId}/image");
        response.EnsureSuccessStatusCode();
        var bytes = await response.Content.ReadAsByteArrayAsync();
        return TypedResults.File(bytes, "image/webp");
    }
}
