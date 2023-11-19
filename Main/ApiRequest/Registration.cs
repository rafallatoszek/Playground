namespace Main.ApiRequest;

internal static class Registration
{
    public static void AddApiRequestModule(this IServiceCollection services) => services.AddHttpClient("weather", client =>
    {
        client.BaseAddress = new Uri("http://playground-api:8080");
    }).AddStandardResilienceHandler();
}
