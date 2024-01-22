using BlazorOidcAD;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

//var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//await builder.Build().RunAsync();
public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        //        builder.Services.AddMsalAuthentication(
        //options =>
        //{
        //   builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
        //    //builder.Configuration.Bind("Google",options.ProviderOptions.Authentication);    
        //    //options.ProviderOptions.DefaultAccessTokenScopes
        //    //    .Add("https://graph.microsoft.com/User.Read");
        //    //options.ProviderOptions.LoginMode = "redirect";
        //});
        // Get the isAuth value from local storage using Blazored.LocalStorage
        builder.Services.AddBlazoredLocalStorage();

        var host = builder.Build();

        // Get the ILocalStorageService instance from the IServiceProvider
        var localStorage = host.Services.GetRequiredService<ILocalStorageService>();

        // Get the isAuth value from local storage
        var isAuth = await localStorage.GetItemAsync<bool>("isAuth");
        if (isAuth)
        {
            builder.Services.AddOidcAuthentication(options =>
            {
                options.ProviderOptions.Authority = "https://accounts.google.com/";
                options.ProviderOptions.ClientId = "1032886300584-o3kossnc7fs0bcvvgcpnphhurbt30p4c.apps.googleusercontent.com";
                options.ProviderOptions.ResponseType = "token id_token";
                options.ProviderOptions.DefaultScopes.Add("profile");
                options.ProviderOptions.DefaultScopes.Add("email");
                options.ProviderOptions.DefaultScopes.Add("openid");
                options.ProviderOptions.RedirectUri = "https://localhost:7248/authentication/login-callback";
                options.ProviderOptions.PostLogoutRedirectUri = "https://localhost:7248/authentication/logout-callback";

                //options.ProviderOptions.Authority = "https://login.microsoftonline.com/2ed42162-0b0a-4972-b5e1-1c6006bd8df4";
                //options.ProviderOptions.ClientId = "f4878d83-cd8f-4b48-a110-ee66eb3d68e4";
            });
        }
        else
        {
            builder.Services.AddOidcAuthentication(options =>
            {
                options.ProviderOptions.Authority = "https://login.microsoftonline.com/2ed42162-0b0a-4972-b5e1-1c6006bd8df4";
                options.ProviderOptions.ClientId = "f4878d83-cd8f-4b48-a110-ee66eb3d68e4";
            });
        }
       
        await builder.Build().RunAsync();
    }
}



//this will works for both