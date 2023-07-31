using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KundenBlazorApp.Shared
{
    public class KundeList
    {
        public int Id { get; set; }
        public List<Kunde>? List { get; set; }
        public DateTime? Date { get; set; }
        
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
    }

    public class KundeListDTO
    {
        public int Id { get; set; }
        public List<Kunde>? AddList { get; set; }
        public List<int>? DeleteList { get; set; }
    }

    public class WinKundeList
    {
        public int Id { get; set; }
        public List<Kunde>? List { get; set; }
    }
}
