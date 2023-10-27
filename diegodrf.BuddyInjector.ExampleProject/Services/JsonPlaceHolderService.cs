using System.Net.Http.Json;
using diegodrf.BuddyInjector.ExampleProject.Models;

namespace diegodrf.BuddyInjector.ExampleProject.Services;

public class JsonPlaceHolderService : IJsonPlaceHolderService
{
    private readonly HttpClient _httpClient;

    public JsonPlaceHolderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<Post?> GetPostByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Post>($"https://jsonplaceholder.typicode.com/posts/{id}");
    }
}