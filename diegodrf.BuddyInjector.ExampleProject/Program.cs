using System.Net;
using diegodrf.BuddyInjector;
using diegodrf.BuddyInjector.ExampleProject.Services;
using RichardSzalay.MockHttp;

// First we instantiate the BuddyInjector calling the helper method RegisterAll().
// This method helps to start BuddyInjector with the most common injections for the tests.
// See the [InitialRegister] below.
var buddyInjector = new BuddyInjector().RegisterAll(InitialRegister);

// We initiated the BuddyInjector injecting the real HttpClient using the RegisterAll().
// But for this test we want to use a mock of HttpClient.
// To do this just need to call the register function again using the new instance.
// The Register function create a new injection if don't exists one or override the register
// if a previous one is registered already.
// It allow you to override only the needed injection for you unit test.
// Try run this program commenting and uncommenting this register to see the result. 
buddyInjector.RegisterSingleton<HttpClient>(() => MockHttpClient());

var jsonPlaceHolderService = buddyInjector.GetInstance<IJsonPlaceHolderService>();
var post = await jsonPlaceHolderService.GetPostByIdAsync(1);
Console.WriteLine(post); // Post { Id = 1, Title = Fake Title, Body = Fake Body, UserId = 1 }
return;

// InitialRegister is an Action that receives an BuddyInjector object and register the default instances for the tests.
void InitialRegister(BuddyInjector x)
{
    x.RegisterSingleton<HttpClient>(() => new HttpClient());
    x.RegisterTransient<IJsonPlaceHolderService>(
        () => new JsonPlaceHolderService(x.GetInstance<HttpClient>()));
}

HttpClient MockHttpClient()
{
    var messageHandler = new MockHttpMessageHandler();
    messageHandler
        .When(HttpMethod.Get, "https://jsonplaceholder.typicode.com/posts/1")
        .Respond(HttpStatusCode.OK, "application/json",
            "{\"id\": 1,\"title\":\"Fake Title\",\"body\":\"Fake Body\",\"userId\":1}");

    return messageHandler.ToHttpClient();
} 