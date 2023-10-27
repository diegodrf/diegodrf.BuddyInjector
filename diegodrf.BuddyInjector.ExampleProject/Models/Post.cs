namespace diegodrf.BuddyInjector.ExampleProject.Models;

public record Post(
    int Id, 
    string Title, 
    string Body, 
    int UserId);