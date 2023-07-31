using KundenBlazorApp.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace KundenBlazorApp.Client.Services
{
    public class CustomAuthProvider : AuthenticationStateProvider
    {
        private readonly IAuthService authService;
        private UserDTO? _currentUser;
        public event Action? OnUserChange;
        public CustomAuthProvider(IAuthService authService)
        {
            this.authService = authService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await GetCurrentUser();
                if (userInfo.IsAuthenticated)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, _currentUser.Name) }
                        .Concat(_currentUser.Roles.Select(c => new Claim(ClaimTypes.Role, c)));
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }
            //OnUserChange?.Invoke();
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public UserDTO CurrentUser
        {
            get
            {
                return _currentUser;
            }
        }
        public async Task<UserDTO> GetCurrentUser()
        {
            if (_currentUser != null && _currentUser.IsAuthenticated)
                return _currentUser;

            _currentUser = await authService.CurrentUserInfo();
            return _currentUser;
        }

        public async Task Logout()
        {
            await authService.Logout();
            _currentUser = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public async Task Login(LoginRequest loginParameters)
        {
            await authService.Login(loginParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public async Task Register(RegisterRequest registerParameters)
        {
            await authService.Register(registerParameters);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
