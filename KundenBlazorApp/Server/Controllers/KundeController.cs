using KundenBlazorApp.Server.Data;
using KundenBlazorApp.Server.Models;
using KundenBlazorApp.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;

namespace KundenBlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[TypeFilter(typeof(SampleExceptionFilter))]
    //[Authorize]
    public class KundeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<KundeController> logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public KundeController(ApplicationDbContext context, ILogger<KundeController> logger,
            UserManager<ApplicationUser> userManager)
        {
            this.logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Kunde>> GetStrana(int id)
        {
            if (_context.Kunden == null)
            {
                return NotFound();
            }
            var strana = await _context.Kunden.FindAsync(id);

            if (strana == null)
            {
                return NotFound();
            }
            return Ok(strana);
        }

        // POST api/<StranaController>
        [HttpPost]
        public async Task<ActionResult> Post(KundeDTO newstrana)
        {
            if (_context.Kunden == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Kunden'  is null.");
            }
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return Problem("'User'  is null. Kunde Post");
            }

            try
            {
                _context.Kunden.Add(
                    new Kunde()
                    {
                        Name1 = newstrana.Name1,
                        Name2 = newstrana.Name2,
                        Name3 = newstrana.Name3,
                        Numer = newstrana.Numer,
                        Region = newstrana.Region,
                        CreatDate = DateTime.UtcNow,
                        User = user
                    });
                //_context.Entry(strana).CurrentValues.SetValues(newstrana);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return Problem(detail: GetError(ex.InnerException?.Message));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutKunde(int id, KundeDTO newstrana)
        {
            if (id != newstrana.Id)
            {
                return BadRequest("id != newstrana.Id");
            }

            Kunde kunde = await _context.Kunden
                .FirstOrDefaultAsync(t => t.Id == id);
            if (kunde == null)
            {
                return BadRequest("'Kunde' is null. PutKunde");
            }

            _context.Entry(kunde).CurrentValues.SetValues(newstrana);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return Problem(detail: GetError(ex.InnerException?.Message));
            }
            return NoContent();
        }

        [HttpPost("kunden")]
        public async Task<ActionResult<KundenListBackDTO>> GetStranas(
            KundenListDTO dto)
        {
            if (_context.Kunden == null)
            {
                return NotFound();
            }
            var list = await _context.Kunden.ToListAsync();

            if (list == null)
            {
                return NotFound("no Kunden");
            }
            if (!string.IsNullOrEmpty(dto.SelectedName))
            {
                list = list
                    .Where(p => p.Name1.Contains(dto.SelectedName, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
            }
            int kTotal = list.Count();

            list = list
                .Skip((dto.CurrentPage - 1) * dto.ItemsPerPage)
                .Take(dto.ItemsPerPage)
                .ToList();

            list = dto.SortState switch
            {
                StranaSortStats.NameDesc => list.OrderByDescending(s => s.Name1).ToList(),
                StranaSortStats.NUTS2Asc => list.OrderBy(s => s.Numer).ToList(),
                StranaSortStats.NUTS2Desc => list.OrderByDescending(s => s.Numer).ToList(),
                _ => list.OrderBy(s => s.Name1).ToList(),
            };

            KundenListBackDTO result = new()
            {
                Kunden = list,
                CurrentPage = dto.CurrentPage,
                Total = kTotal
            };
            return Ok(result);
        }


        string GetError(string mess)
        {
            string str = "Ошибка сохранения в базу";
            if (string.IsNullOrEmpty(mess))
                return str;
            Regex regex = new(@"\((\w*)\)");
            MatchCollection matches = regex.Matches(mess);
            if (matches.Count > 0)
                str = $"Такое уже есть в базе: {matches[0].Value.Trim(')').TrimStart('(')}";
            return str;
        }


        /* ==================== LISTS ==================== */
        [HttpGet("liste")]
        public async Task<ActionResult<List<KundeList>>> GetListe()
        {
            if (_context.KundeListe == null)
            {
                return NotFound("no KundeListe");
            }
            
            var liste = await _context.KundeListe
                .Include(k=>k.List)
                .ToListAsync();

            if (liste == null)
            {
                return NotFound("liste");
            }
            return Ok(liste);
        }

        [HttpGet("list/{id}")]
        public async Task<ActionResult> GetList(int id)
        {
            if (_context.KundeListe == null)
            {
                return NotFound("no KundeListe");
            }

            var list = await _context.KundeListe
                .Where(k => k.Id == id)
                .Include(k => k.List)
                .FirstOrDefaultAsync();

            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }
        

        [HttpPost("list")]
        public async Task<ActionResult> PostList(KundeList list)
        {
            if (_context.KundeListe == null)
            {
                return NotFound();
            }
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return Problem("'User'  is null. PostList");
            }
            try
            {
                if (list.List != null)
                {
                    foreach (var kunde in list.List)
                    {
                        kunde.CreatDate = DateTime.UtcNow;
                        kunde.User = user;
                    }
                }
                list.Date = DateTime.UtcNow;
                list.User = user;

                _context.KundeListe.Add(list);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return Problem(detail: GetError(ex.InnerException?.Message));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return Ok();
        }

        [HttpPut("list/{id}")]
        public async Task<IActionResult> PutList(int id, KundeListDTO dto)
        {
            //return Content($"- {nameof(KundeController)}.{nameof(PutList)}");
            if (id != dto.Id)
            {
                return BadRequest("id != newstrana.Id");
            }

            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return Problem("'User'  is null. PutList");
            }

            KundeList kunde = await _context.KundeListe
                .Include(k=>k.List)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (kunde == null)
            {
                return BadRequest("'KundeList' is null. Put");
            }

            foreach (var item in dto.DeleteList)
            {
                var kund = kunde.List
                    .FirstOrDefault(c => c.Id == item);
                if (kund is not null)
                    kunde.List.Remove(kund);
            }
            foreach (var item in dto.AddList)
            {
                item.CreatDate = DateTime.Now;
                item.User = user;
                kunde.List.Add(item);
            }
            //_context.Entry(kunde).CurrentValues.SetValues(newstrana);
            _context.Update(kunde);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return Problem(detail: GetError(ex.InnerException?.Message));
            }
            return NoContent();
        }

        /* ==================== Auslosung ==================== */
        [HttpGet("auslosung")]
        public async Task<ActionResult<List<ZiehungAuslosung>>> GetAuslosungen()
        {
            if (_context.ZiehungAuslosungen == null)
            {
                return NotFound("no ZiehungAuslosungen");
            }
            var liste = await _context.ZiehungAuslosungen
                .Include(k => k.WinKundeList)
                .ToListAsync();

            if (liste == null)
            {
                return NotFound("GetAuslosungen");
            }
            return Ok(liste);
        }

        [HttpPost("auslosung")]
        public async Task<ActionResult> PostAuslosung(ZiehungAuslosungDTO dto)
        {
            if (_context.ZiehungAuslosungen == null)
            {
                return NotFound();
            }
            ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return Problem("'User'  is null. PostAuslosung");
            }
            try
            {
                if (dto.WinKundeList != null)
                {
                    ZiehungAuslosung az = new ZiehungAuslosung();
                    az.Titel = dto.Titel;
                    var kundelist = _context.KundeListe
                        .FirstOrDefault(c => c.Id == dto.KundeList);
                    if (kundelist is null)
                        return Problem("'PostAuslosung': no such KundeList");
                    az.KundeList = kundelist;

                    List<Kunde> wk = new List<Kunde>();
                    wk = new List<Kunde>();
                    foreach (var knd in dto.WinKundeList)
                    {
                        var kunde = _context.Kunden
                            .FirstOrDefault(c => c.Id == knd);
                        if (kunde is null)
                            return Problem("'PostAuslosung': no such Kunde");
                        wk.Add(kunde);
                    }
                    az.WinKundeList = wk;
                    az.Date = DateTime.UtcNow;

                    _context.ZiehungAuslosungen.Add(az);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return BadRequest("dto.WinKundeList != nul");
                }
            }
            catch (DbUpdateException ex)
            {
                return Problem(detail: GetError(ex.InnerException?.Message));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            return Ok();
        }

    }
}
