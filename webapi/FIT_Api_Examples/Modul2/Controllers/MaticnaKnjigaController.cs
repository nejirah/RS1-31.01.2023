using FIT_Api_Examples.Data;
using FIT_Api_Examples.Helper.AutentifikacijaAutorizacija;
using FIT_Api_Examples.Modul2.Models;
using FIT_Api_Examples.Modul2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIT_Api_Examples.Modul2.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class MaticnaKnjigaController:ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public MaticnaKnjigaController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpPost]
        public ActionResult Dodaj([FromBody] UpisaneGodineDodajVM x)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("Korisnik nije logiran");

            UpisaneGodine pronadena = _dbContext.UpisaneGodine.FirstOrDefault(s => s.GodinaStudija == x.godinaStudija);

            if (pronadena != null && x.obnova == false)
                return BadRequest("Godina vec postoji");

            UpisaneGodine godina = new UpisaneGodine
            {
                Id = x.id,
                DatumUpisa = x.datumUpisa,
                CijenaSkolarine = x.cijenaSkolarine,
                GodinaStudija = x.godinaStudija,
                Obnova = x.obnova,
                AkademskaGodinaId = x.akademskaGodinaId,
                StudentId = x.studentId,
                KorinikId = HttpContext.GetLoginInfo().korisnickiNalog.id
            };

            _dbContext.Add(godina);
            _dbContext.SaveChanges();
            return Ok();

        }

        //ViewModeli
        public class StudentPodaciVM
        {
            public int id { get; set; }
            public string ime { get; set; }
            public string prezime { get; set; }
            public List<PodaciGodinaStudentaVM> Godine { get; set; }
        }
        public class PodaciGodinaStudentaVM
        {
            public int id { get; set; }
            public int akademskaGodinaId { get; set; }
            public string akademskaGodinaOpis { get; set; }
            public int godinaStudija { get; set; }
            public bool obnova { get; set; }
            public DateTime datumUpisa { get; set; }
            public DateTime? datumOvjere { get; set; }
            public int korisnikId { get; set; }
            public string korisnik { get; set; }
        }

        [HttpGet]
        public ActionResult GetById(int id)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("Korisnik nije logiran");

            var godine = _dbContext.UpisaneGodine.Where(s => s.StudentId == id).Select(s=> new PodaciGodinaStudentaVM
            {
                id=s.Id,
                akademskaGodinaId=s.AkademskaGodinaId,
                akademskaGodinaOpis=s.AkademskaGodinaObjekat.opis,
                godinaStudija=s.GodinaStudija,
                datumUpisa=s.DatumUpisa,
                datumOvjere=s.DatumOvjere,
                korisnikId=s.KorinikId,
                korisnik=s.KorisnikObjekat.korisnickoIme,
                obnova=s.Obnova
            });

            var student = _dbContext.Student.Where(a => a.id == id).Select(p=> new StudentPodaciVM
            {
                id=p.id,
                ime=p.ime,
                prezime=p.prezime,
                Godine=godine.ToList()
            }).FirstOrDefault();

            return Ok(student);
        }

        [HttpPost]
        public ActionResult Update([FromBody] UpdateVM obj)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("Korisnik nije logiran");
            UpisaneGodine godina = _dbContext.UpisaneGodine.FirstOrDefault(s => s.Id == obj.id);
            if (godina == null)
                return BadRequest("Nije pronadena godina");
            godina.DatumOvjere = obj.datumOvjere;
            godina.Napomena = obj.napomena;
            _dbContext.SaveChanges();
            return Ok();
        }

    }
}
