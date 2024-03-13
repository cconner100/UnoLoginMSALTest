namespace UnoLoginMSALTest;
public interface IMsalSettings
{
    /// <summary>
    /// Use a brokered authentication service.
    /// </summary>
    bool UseBroker { get; set; }
    /// <summary>
    /// The client id of the application.
    /// </summary>
    string ClientId { get; set; }
    /// <summary>
    /// The authority to use for authentication. e.g. com.company.appname
    /// </summary>
    string TenantName { get; set; }
    /// <summary>
    /// SPA redirect uri
    /// </summary>
    string RedirectUri { get; set; }
    /// <summary>
    /// Desktop redirect uri e.g msal{ClientIdSecret}://auth
    /// </summary>
    string DesktopRedirectUri { get; set; }
    /// <summary>
    /// The scopes to request.
    /// </summary>
    string[] Scopes { get; set; }
    /// <summary>
    /// Azure  AD B2C policy for login
    /// </summary>
    string LoginPolicy { get; set; }
    /// <summary>
    /// Azure AD B2C policy for reset password
    /// </summary>
    string ResetPasswordPolicy { get; set; }
    /// <summary>
    /// bundle name for iOS and Android
    /// </summary>
    string BundleName { get; set; }

    /// <summary>
    /// For WINUI3 cache file name
    /// </summary>
    string CacheFileName { get; set; }
    /// <summary>
    /// For WINUI3 cache directory
    /// </summary>
    string CacheDir { get; set; }

    // Below are the values used for login
    public string AuthoritySignIn { get; }
    public string AuthorityPasswordReset { get; }
    public string AuthorityRedirectUri { get; }

}
