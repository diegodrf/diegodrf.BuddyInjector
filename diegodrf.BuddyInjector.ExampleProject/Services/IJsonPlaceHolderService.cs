using diegodrf.BuddyInjector.ExampleProject.Models;

namespace diegodrf.BuddyInjector.ExampleProject.Services;

public interface IJsonPlaceHolderService
{
    Task<Post?> GetPostByIdAsync(int id);
}