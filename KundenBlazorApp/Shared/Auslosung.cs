using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KundenBlazorApp.Shared
{
    public class ZiehungAuslosungDTO
    {
        public string? Titel { get; set; }

        public int? KundeList { get; set; }
        public List<int>? WinKundeList { get; set; }
    }
    
    public class ZiehungAuslosung
    {
        public int Id { get; set; }
        public string? Titel { get; set; }

        public KundeList? KundeList { get; set; }
        public List<Kunde>? WinKundeList { get; set; }
        public DateTime? Date { get; set; }
    }
}
