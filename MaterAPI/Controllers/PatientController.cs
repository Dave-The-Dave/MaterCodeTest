using MaterCodeTest.Models;
using MaterCodeTest.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;

namespace MaterCodeTest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        //define database context
        private readonly ApiContext _context;

        public PatientController(ApiContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        //Get all
        [HttpGet]
        public async Task<IEnumerable<Patient>> Get() => await _context.Patients.Include(x => x.Comments).ToListAsync();


        //Get Patient
        [HttpGet]
        [ActionName("GetByURN")]
        public IActionResult GetByURN(string URN)
        {
            var result = _context.Patients.Include(x => x.Comments).ThenInclude(x => x.StaffMember).Where(x => x.PatientURN == URN);
            return result == null ? NotFound() : Ok(result);
        }

        //Get staff for testing demo data
        [HttpGet]
        public async Task<IEnumerable<Staff>> GetStaff() => await _context.Staff.ToListAsync();


        //Get Last Comment
        [HttpGet]
        public IActionResult GetLastComment(string URN)
        {
            var patient = _context.Patients.Include(x => x.Comments).Where(x => x.PatientURN == URN).FirstOrDefault();
            var result = patient.Comments.OrderByDescending(x => x.LoggedDate).FirstOrDefault();

            return result == null ? NotFound() : Ok(result);
        }
     
        //Add New Patient
        [HttpPost]
        public async Task<IActionResult> CreateEdit(Patient patient)
        {
            //check if patient exists
            var result = _context.Patients.Where(x => x.PatientURN == patient.PatientURN);
            if (result.Count() == 0)
            {
                //create comment for patient
                Comment comment = new Comment()
                {
                    PatientURN= patient.PatientURN,
                    Patient = patient,
                    //for simplicity, assign staffID 1, this value would be taken from current user info in a realworld example
                    StaffId = 1,
                    LoggedDate= DateTime.Now,
                    LoggedComment = "Created"
                };
                patient.Comments = new List<Comment> { comment };
                await _context.Patients.AddAsync(patient);

                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetByURN), new {patient.PatientURN }, patient);
            }
            //else, check for patient and alter if found
            else
            {
                _context.Entry(patient).State= EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }            
        }

        //Add New Comment
        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            //find patient and staff memember
            var patient = _context.Patients.Include(x => x.Comments).ThenInclude(x => x.StaffMember).Where(x => x.PatientURN == comment.PatientURN).FirstOrDefault();
            var staff = _context.Staff.Find(comment.StaffId);

            //if patient doesnt exist - real world example would check staff ID as well
            if (patient == null)
                return new JsonResult(NotFound());

            //else add new comment
            patient.Comments.Add(comment);
            _context.SaveChanges();

            return Ok(patient);
        }

    }
}
