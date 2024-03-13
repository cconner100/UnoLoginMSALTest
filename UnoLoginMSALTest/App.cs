using System.Diagnostics;

using CommunityToolkit.Mvvm.DependencyInjection;

namespace UnoLoginMSALTest;

public class App : Application
{
    protected Window? MainWindow { get; private set; }
    protected IHost? Host { get; private set; }

    readonly MsalSettings msalSettings = new()
    {
        UseBroker = false,
        ClientId = "3e2fe1b7-910e-46d2-aa0b-b1da7755d458",
        TenantName = "ziidmsapp",
        LoginPolicy = "B2C_1_si",
        ResetPasswordPolicy = "B2C_1_reset_password",
        RedirectUri = "http://localhost:5000",
        DesktopRedirectUri = "msal3e2fe1b7-910e-46d2-aa0b-b1da7755d458://auth",
        BundleName = "com.ZiiDMS.ZiiDMSApp",
        Scopes = ["https://ZiiDMSAPP.onmicrosoft.com/ZiiDMSWebApi/access_as_user"],
        CacheFileName = "msalcache.bin",
        CacheDir = Windows.Storage.ApplicationData.Current.LocalFolder.Path
    };
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            .Configure(host => host
#if DEBUG
                // Switch to Development environment when running in DEBUG
                .UseEnvironment(Environments.Development)
#endif
                .UseLogging(configure: (context, logBuilder) =>
                {
                    // Configure log levels for different categories of logging
                    logBuilder
                        .SetMinimumLevel(
                            context.HostingEnvironment.IsDevelopment() ?
                                LogLevel.Information :
                                LogLevel.Warning)

                        // Default filters for core Uno Platform namespaces
                        .CoreLogLevel(LogLevel.Warning);

                    // Uno Platform namespace filter groups
                    // Uncomment individual methods to see more detailed logging
                    //// Generic Xaml events
                    //logBuilder.XamlLogLevel(LogLevel.Debug);
                    //// Layout specific messages
                    //logBuilder.XamlLayoutLogLevel(LogLevel.Debug);
                    //// Storage messages
                    //logBuilder.StorageLogLevel(LogLevel.Debug);
                    //// Binding related messages
                    //logBuilder.XamlBindingLogLevel(LogLevel.Debug);
                    //// Binder memory references tracking
                    //logBuilder.BinderMemoryReferenceLogLevel(LogLevel.Debug);
                    //// DevServer and HotReload related
                    //logBuilder.HotReloadCoreLogLevel(LogLevel.Information);
                    //// Debug JS interop
                    //logBuilder.WebAssemblyLogLevel(LogLevel.Debug);

                }, enableUnoLogging: true)
                .UseConfiguration(configure: configBuilder =>
                    configBuilder
                        .EmbeddedSource<App>()
                        .Section<AppConfig>()
                )
                // Enable localization (see appsettings.json for supported languages)
                .UseLocalization()
                // Register Json serializers (ISerializer and ISerializer)
                .UseSerialization((context, services) => services
                    .AddContentSerializer(context)
                    .AddJsonTypeInfo(WeatherForecastContext.Default.IImmutableListWeatherForecast))
                .UseHttp((context, services) => services
                    // Register HttpClient
#if DEBUG
                    // DelegatingHandler will be automatically injected into Refit Client
                    .AddTransient<DelegatingHandler, DebugHttpHandler>()
#endif
                    .AddSingleton<IWeatherCache, WeatherCache>()
                    .AddRefitClient<IApiClient>(context))
                .UseAuthentication(auth =>
#if __MACCATALYST__
                    // We will use ODIC for the MacCatalyst platform
                    auth.AddOidc(odic =>
                    {
                        odic.Authority(msalSettings.AuthoritySignIn)
                        .ClientId(msalSettings.ClientId)
                        .Scope(string.Join(string.Empty, msalSettings.Scopes))
                        .RedirectUri("myprotocol://callback")
                        .PostLogoutRedirectUri("myprotocol://callback");
                    })
#else
                    // We will use MSAL for Windows WinUI3, iOS, Android and WebAssembly
                    auth.AddMsal(msal =>
                    {
                        msal.Scopes(msalSettings.Scopes)
                        .Builder(msalBuilder =>
                            msalBuilder
                            .WithClientId(msalSettings.ClientId)
                            .WithB2CAuthority(msalSettings.AuthoritySignIn)
                            .WithRedirectUri(msalSettings.RedirectUri)
                            .WithBroker(msalSettings.UseBroker)
#if DEBUG
                            .WithLogging((level, message, _) => { Debug.WriteLine($"MSAL: {level} {message} "); })
#endif
#if __IOS__
                            .WithIosKeychainSecurityGroup(msalSettings.BundleName)
#endif
                            );
                    })
#endif
                )
                .ConfigureServices((context, services) =>
                {
                    // TODO: Register your services
                    //services.AddSingleton<IMyService, MyService>();
                })
            );
        MainWindow = builder.Window;

#if DEBUG
        MainWindow.EnableHotReload();
#endif

        Host = builder.Build();
        Ioc.Default.ConfigureServices(Host.Services);

        // Do not repeat app initialization when the Window already has content,
        // just ensure that the window is active
        if (MainWindow.Content is not Frame rootFrame)
        {
            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = new Frame();

            // Place the frame in the current Window
            MainWindow.Content = rootFrame;
        }

        if (rootFrame.Content == null)
        {
            // When the navigation stack isn't restored navigate to the first page,
            // configuring the new page by passing required information as a navigation
            // parameter
            rootFrame.Navigate(typeof(MainPage), args.Arguments);
        }
        // Ensure the current window is active
        MainWindow.Activate();
    }
}
