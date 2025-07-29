using Microsoft.AspNetCore.Http.HttpResults;
using WebShop.Web.Models;

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

    public async Task<List<CatalogItem>> GetItemsByIdsAsync(List<int> itemIds)
    {
        //TODO: change this method to use batch api endpoint
        var items = new List<CatalogItem>();
        foreach (var itemId in itemIds)
        {
            var item = await GetItemByIdAsync(itemId);
            if (item != null)
            {
                items.Add(item);
            }
        }
        return items;
    }

    public async Task<FileContentHttpResult> GetImageUrlAsync(int itemId)
    {
        var response = await _httpClient.GetAsync($"/api/Catalog/{itemId}/image");
        response.EnsureSuccessStatusCode();
        var bytes = await response.Content.ReadAsByteArrayAsync();
        return TypedResults.File(bytes, "image/webp");
    }
}
