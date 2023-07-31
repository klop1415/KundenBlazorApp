using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KundenBlazorApp.Shared
{
    public class Kunde
    {
        public int Id { get; set; }
        
        [StringLength(64)]
        [Display(Name = "Фамилия:")]
        [Required(ErrorMessage = "Введите фамилию")]
        public string Name1 { get; set; }
        
        [StringLength(32)]
        [Display(Name = "Имя:")]
        [Required(ErrorMessage = "Введите имя")]
        public string? Name2 { get; set; }
        public string? Name3 { get; set; }

        [StringLength(15)]
        [Display(Name = "Номер телефона:")]
        [Required(ErrorMessage = "Введите номер телефона")]
        public string Numer { get; set; }
        
        [StringLength(127)]
        public string? Region { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? CreatDate { get; set; }

        [JsonIgnore]
        public ApplicationUser? User { get; set; }
        
        public List<ZiehungAuslosung>? WinKundeLists { get; set; }

    }
    public class KundeDTO
    {
        public int Id { get; set; }

        [StringLength(64)]
        [Display(Name = "Фамилия:")]
        [Required(ErrorMessage = "Введите фамилию")]
        public string Name1 { get; set; }

        [StringLength(32)]
        [Display(Name = "Имя:")]
        [Required(ErrorMessage = "Введите имя")]
        public string Name2 { get; set; }
        public string? Name3 { get; set; }

        [StringLength(15)]
        [Display(Name = "Номер телефона:")]
        [Required(ErrorMessage = "Введите номер телефона")]
        public string Numer { get; set; }

        [StringLength(127)]
        public string? Region { get; set; }

    }

    public record KundenListDTO
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public StranaSortStats SortState { get; set; }
        public string? SelectedName { get; set; }
    }

    public record KundenListBackDTO
    {
        public List<Kunde> Kunden { get; set; } = new();
        public int CurrentPage { get; set; }
        public int Total { get; set; }
    }
    [Flags]
    public enum StranaSortStats : byte
    {
        [Display(Name = "фамилии по алфавиту")]
        NameAsc = 0b_0000_0000,
        [Display(Name = "фамилии в обратном порядке")]
        NameDesc = 0b_0000_0001,
        [Display(Name = "телефоны по возрастанию")]
        NUTS2Asc = 0b_0000_0010,
        [Display(Name = "телефоны в обратном порядке")]
        NUTS2Desc = 0b_0000_0100,
        [Display(Name = "по возрастанию кода")]
        KodAsc = 0b_0000_1000,
        KodDesc = 0b_0001_0000
    }
}
