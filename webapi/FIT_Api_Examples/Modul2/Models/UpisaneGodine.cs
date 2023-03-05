using FIT_Api_Examples.Modul0_Autentifikacija.Models;
using FIT_Api_Examples.Modul3_MaticnaKnjiga.Models;
using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT_Api_Examples.Modul2.Models
{
    public class UpisaneGodine
    {
        public int Id { get; set; }
        public DateTime DatumUpisa { get; set; }
        public int GodinaStudija { get; set; }
        public float CijenaSkolarine { get; set; }
        public bool Obnova { get; set; }
        public DateTime? DatumOvjere { get; set; }
        public string? Napomena { get; set; }

        [ForeignKey(nameof(AkademskaGodinaObjekat))]
        public int AkademskaGodinaId { get; set; }
        public AkademskaGodina AkademskaGodinaObjekat { get; set; }

        [ForeignKey(nameof(StudentObjekat))]
        public int StudentId { get; set; }
        public Student StudentObjekat { get; set; }

        [ForeignKey(nameof(KorisnikObjekat))]
        public int KorinikId { get; set; }
        public KorisnickiNalog KorisnikObjekat { get; set; }
    }
}
