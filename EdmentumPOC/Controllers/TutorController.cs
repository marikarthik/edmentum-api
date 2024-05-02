using EdmentumBLL.DTO;
using EdmentumBLL.Manager;
using Microsoft.AspNetCore.Mvc;

namespace EdmentumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TutorController : ControllerBase
    {
        private readonly TutorManager _tutorManager;

        public TutorController(TutorManager tutorManager)
        {
            _tutorManager = tutorManager;
        }

        [HttpPost]
        public IActionResult AddTutor(TutorDTO tutorDto)
        {
            try
            {
                _tutorManager.AddTutor(tutorDto);
                return Ok("Tutor added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetAllTutors()
        {
            try
            {
                var tutors = _tutorManager.GetAllTutors();
                return Ok(tutors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
