using Microsoft.AspNetCore.Authorization;
using ID = Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using KundenBlazorApp.Server.Data;
using KundenBlazorApp.Shared;

namespace KundenBlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ID.RoleManager<ID.IdentityRole> _roleManager;
        private readonly ID.UserManager<ApplicationUser> _userManager;
        private readonly ID.SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment env;
        public AuthController(IWebHostEnvironment env,
                      ID.UserManager<ApplicationUser> userManager,
                      ID.SignInManager<ApplicationUser> signInManager,
                      ID.RoleManager<ID.IdentityRole> roleManager)
        {
            this.env = env;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> Get()
        {
            return await _userManager.Users.ToListAsync();
        }

        [Authorize]
        [HttpGet("totaluserscount")]
        public ActionResult<int> GetTotal() => 1000;
        /*            _userManager.Users
                        //.Where(r => r.Name != "pichugina")
                        .Count();
        */
        [HttpGet("usersroles/{userid}")]
        public async Task<ActionResult<UserWidthRoles>> Get1(string userid)
        {
            var user = await _userManager.Users
                .Where(u => u.Id == userid)
                .FirstOrDefaultAsync();
            if (user is not null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                UserWidthRoles userWidthRoles = new(
                    user.Id,
                    user.UserName ?? "",
                    roles.Where(r => string.Compare(r, Role.pichugina.ToString()) != 0).ToArray()
                );
                return userWidthRoles;
            }
            return BadRequest();
        }

        [Authorize(Roles = "pichugina")]
        [HttpGet("usersroles")]
        public async Task<ActionResult<IEnumerable<UserWidthRoles>>> GetUsersRoles()
        {
            string nameSearch = HttpContext.Request.Query["nameSearch"];
            //string sortByColumnTitle = HttpContext.Request.Query["SortByColumnTitle"];
            //string sortByAscending = HttpContext.Request.Query["SortByAscending"];

            var users = await _userManager.Users
                .OrderBy(u=>u.UserName)
                .ToListAsync();

            List<UserWidthRoles> list = new();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                roles = roles.Where(r => r != Role.pichugina.ToString()).ToArray();

                UserWidthRoles userWidthRoles = new(
                    user.Id,
                    user.UserName,
                    roles.ToArray()
                );
                list.Add(userWidthRoles);
            }
            return list;
        }


        [Authorize(Roles = "pichugina")]
        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<ID.IdentityRole>>> GetRoles() => await
            _roleManager.Roles
                .Where(r => r.Name != "pichugina")
                .ToListAsync();

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
                return BadRequest("Неправильное имя, такого нету в базе");

            var singInResult =
                await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!singInResult.Succeeded)
                return BadRequest("Неправильный пароль");

            //var sdf = User.Identity.Name;

            var result = await _signInManager.PasswordSignInAsync(
                dto.UserName, dto.Password, dto.RememberMe, false);
            if (result.Succeeded)
            {
                return Ok();
            }

            //await _signInManager.SignInAsync(user, dto.RememberMe);

            return Ok();

            //string token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoienhjIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE2NzMzOTY3MTl9.zXPyZVMbeLnSdk4BGzoAQiy8KkbRXTWRId9hgY6ant0";
            //return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest parameters)
        {
            var user = new ApplicationUser();
            user.UserName = parameters.UserName;
            user.AvatarName = parameters.Avatar;

            var result = await _userManager.CreateAsync(user, parameters.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors.FirstOrDefault()?.Description);

            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            await _userManager.AddToRoleAsync(user, Role.user.ToString());

            return await Login(new LoginRequest
            {
                UserName = parameters.UserName,
                Password = parameters.Password
            });
        }

        [HttpPut("roles/{userid}")]
        public async Task<IActionResult> PutRoles(string userid, List<string> roles)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userid);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Where(r => r != Role.pichugina.ToString()).Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet("user")]
        public async Task<ActionResult<UserDTO>> CurrentUserInfo()
        {
            var nouserDTO = new UserDTO
            {
                IsAuthenticated = false,
                Name = "Неправильное имя"
            };

            if (User.Identity != null && User.Identity.Name != null)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                    return nouserDTO;

                List<string> claims = User
                    .FindAll(ClaimTypes.Role)
                    .Select(r => r.Value)
                    .ToList();

                return new UserDTO
                {
                    IsAuthenticated = User.Identity.IsAuthenticated,
                    Name = User.Identity.Name,
                    Avatar = user.AvatarName,
                    Roles = claims
                };
            }
            return nouserDTO;
        }

        async Task<string> SavePicture(byte[] data)
        {
            if (data == null) return string.Empty;
            string trustedFileNameForFileStorage = Path.GetRandomFileName();
            var path = Path.Combine(env.WebRootPath,
                "AvatPics",
                trustedFileNameForFileStorage);

            using (var fileStream = System.IO.File.Create(path))
            {
                await fileStream.WriteAsync(data);
            }
            return trustedFileNameForFileStorage;

        }

    }
}
