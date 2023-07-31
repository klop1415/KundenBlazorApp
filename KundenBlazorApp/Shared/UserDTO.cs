using System.ComponentModel.DataAnnotations;

namespace KundenBlazorApp.Shared
{
    public class UserDTO
    {
        public string? Name { get; set; }
        public string Password { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        public List<string> Roles { get; set; } = new();
        public string? Avatar { get; set; }
    }
    public class UpdateUserDTO
    {
        public string? Name { get; set; }
        public string? Password { get; set; }

        [MaxLength(12)]
        public string? Avatar { get; set; }
    }

    public class RegisterRequest
    {
        [Required(ErrorMessage = "Имя не введено!")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль не ввёл!")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "пароль не подтвержден")]
        [Compare(nameof(Password), ErrorMessage = "Эээ.. Пароли чёто не совпадют!")]
        public string PasswordConfirm { get; set; } = string.Empty;

        public string? Avatar { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = true;
    }
    public record UserWidthRoles(string userId, string userName, string[] roles);
}
