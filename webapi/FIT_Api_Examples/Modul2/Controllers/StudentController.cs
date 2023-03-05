using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FIT_Api_Examples.Data;
using FIT_Api_Examples.Helper;
using FIT_Api_Examples.Helper.AutentifikacijaAutorizacija;
using FIT_Api_Examples.Modul0_Autentifikacija.Models;
using FIT_Api_Examples.Modul2.Models;
using FIT_Api_Examples.Modul2.ViewModels;
using FIT_Api_Examples.Modul3_MaticnaKnjiga.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIT_Api_Examples.Modul2.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

      

        [HttpGet]
        public ActionResult<List<Student>> GetAll(string ime_prezime)
        {
            var data = _dbContext.Student
                .Include(s => s.opstina_rodjenja.drzava)
                .Where(x => ime_prezime == null || (x.ime + " " + x.prezime).StartsWith(ime_prezime) || (x.prezime + " " + x.ime).StartsWith(ime_prezime))
                .OrderByDescending(s => s.id)
                .AsQueryable();
            return data.Take(100).ToList();
        }

        [HttpPost]
        public ActionResult Snimi([FromBody] StudentAddVM x)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("Korisnik nije logiran");

            Student s = _dbContext.Student.FirstOrDefault(s => s.id == x.id);
            if (s == null)
            {
                s = new Student
                {
                    created_time = DateTime.Now,
                };
                _dbContext.Add(s);
            }

            s.ime = x.ime;
            s.prezime = x.prezime;
            s.opstina_rodjenja_id = x.opstina_rodjenja_id;
            _dbContext.SaveChanges();
            s.broj_indeksa = "IB200" + _dbContext.Student.FirstOrDefault(a => a.id == s.id).id;
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public ActionResult Obrisi(Student s)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("Korisnik nije logiran");
            Student student = _dbContext.Student.FirstOrDefault(a=>a.id==s.id);
            if (student == null)
                return BadRequest("Student ne postoji");
            _dbContext.Remove(student);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
