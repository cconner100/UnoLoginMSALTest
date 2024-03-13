using CommunityToolkit.Mvvm.DependencyInjection;

namespace UnoLoginMSALTest;

public sealed partial class MainPage : Page
{
    readonly IAuthenticationService authService;
    readonly ITokenCache tokens;
    public MainPage()
    {
        InitializeComponent();
        authService = Ioc.Default.GetRequiredService<IAuthenticationService>();
        tokens = Ioc.Default.GetRequiredService<ITokenCache>();
    }

    async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        var dispatcher = new Dispatcher(this);
        // Try to just get the login to work for now
        var login = await authService.LoginAsync(dispatcher: dispatcher);
        if(login is true)
        {
            var mytoken = await tokens.AccessTokenAsync();
        }
    }
}
