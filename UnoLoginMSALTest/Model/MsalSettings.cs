namespace UnoLoginMSALTest;
public class MsalSettings : IMsalSettings
{
    public bool UseBroker { get; set; }
    public string ClientId { get; set; } = default!;
    public string TenantName { get; set; } = default!;
    public string RedirectUri { get; set; } = default!;
    public string DesktopRedirectUri { get; set; } = default!;
    public string LoginPolicy { get; set; } = default!;
    public string ResetPasswordPolicy { get; set; } = default!;
    public string BundleName { get; set; } = default!;

    public string[] Scopes { get; set; } = default!;
    public string CacheFileName { get; set; } = default!;
    public string CacheDir { get; set; } = default!;

    string AuthorityBase => $"https://{TenantName}.b2clogin.com/tfp/{TenantName}.onmicrosoft.com/";
    public string AuthoritySignIn => $"{AuthorityBase}{LoginPolicy}";
    public string AuthorityPasswordReset => $"{AuthorityBase}{ResetPasswordPolicy}";

#if __ANDROID__ || __IOS__ || WINDOWS
    public string AuthorityRedirectUri => DesktopRedirectUri;
#else
    public string AuthorityRedirectUri => RedirectUri;
#endif


#if __MACCATALYST__
    public string User { get; set; } = default!;
    public string Password { get; set; } = default!;
#endif
}
