using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MaterCodeTest.Models;
using MaterCodeTest.Data;
using Microsoft.EntityFrameworkCore;

namespace MaterCodeTest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PatientAdmitionController : ControllerBase
    {
        //define database context
        private readonly ApiContext _context;

        public PatientAdmitionController(ApiContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        //Get Admition
        [HttpGet]
        [ActionName("GetById")]
        public IActionResult GetById( int id)
        {
            var result = _context.Admitions.Find(id);

            //if admition doesnt exist
            if (result == null)
                return new JsonResult(NotFound());

            //return admition
            return new JsonResult(Ok(result));
        }

        //Get Beds
        [HttpGet]
        public async Task<IEnumerable<Bed>> GetBeds() => await _context.Beds.Include(x => x.Admition).Include(x => x.Patient).ThenInclude(x => x.Comments).ThenInclude(x => x.StaffMember).ToListAsync();

        //Get all Admittions
        [HttpGet]
        public async Task<IEnumerable<PatientAdmition>> GetAllAdmittions() => await _context.Admitions.ToListAsync();

        // Admit a patient
        [HttpPost]
        public async Task<IActionResult> CreateEdit(PatientAdmition admition) 
        {
            //if id is 0, new admition, add to database
            if (admition.AdmitionId == 0)
            {
                await _context.Admitions.AddAsync(admition);

                //find and update relative data
                var bed = _context.Beds.Find(admition.BedId);
                bed.Vaccancy = Vaccancy.InUse;
                bed.Admition = admition;

                var patient = _context.Patients.Include(x => x.Comments).Where(x => x.PatientURN == admition.PatientURN).FirstOrDefault();
                bed.Patient = patient;

                //create comment for patient
                Comment comment = new Comment()
                {
                    PatientURN = patient.PatientURN,
                    Patient = patient,
                    //for simplicity, assign staffID 1, this value would be taken from current user info in a realworld example
                    StaffId = 1,
                    LoggedDate = DateTime.Now,
                    LoggedComment = "Admitted"
                };
                patient.Comments.Add(comment);


                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = admition.AdmitionId }, admition);
            }
            //else, check for patient and alter if found
            else
            {
                _context.Entry(admition).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }

        //Discharge
        [HttpPost]
        public async Task<IActionResult> Discharge(int id)
        {
            var admition = _context.Admitions.Find(id);
            var bed = _context.Beds.Find(admition.BedId);
            var patient = _context.Patients.Include(x => x.Comments).Where(x => x.PatientURN == admition.PatientURN).FirstOrDefault();
            //if admition or patient doesnt exist
            if (admition == null || patient == null)
                return NotFound();

            //else update record status to discharged
            admition.Status = Status.Discharged;
            
            //free bed
            bed.Vaccancy = Vaccancy.Free;
            bed.Patient = null;
            bed.Admition = null;

            //create comment for patient
            Comment comment = new Comment()
            {
                PatientURN = admition.PatientURN,
                Patient = patient,
                //for simplicity, assign staffID 1, this value would be taken from current user info in a realworld example
                StaffId = 1,
                LoggedDate = DateTime.Now,
                LoggedComment = "Discharged"
            };
            patient.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(admition);
        }
    }

}
