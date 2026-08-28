//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Authorization;

namespace ItemMaster.Client.Pages.Authentication
{
    public partial class RedirectToLogin
    {
        //[Inject]
        //public NavigationManager NavigationManager { get; set; }
        //private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        //protected override async void OnInitialized()
        //{
        //    //   NavigationManager.NavigateTo("pages/authentication/login");
        //    // NavigationManager. NavigateTo(
        //    // $"authentication/login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
        //    //  NavigationManager.NavigateTo($"authentication/login?returnUrl={NavigationManager.Uri}");



        //    var authenticationState = await AuthenticationStateTask;

        //    if (authenticationState?.User?.Identity is null || !authenticationState.User.Identity.IsAuthenticated)
        //    {
        //        var returnUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);

        //        if (string.IsNullOrWhiteSpace(returnUrl))
        //            NavigationManager.NavigateTo("/pages/authentication/login", true);
        //        else
        //            NavigationManager.NavigateTo($"/pages/authentication/login?returnUrl={returnUrl}", true);
        //    }

        //}
    }
}
