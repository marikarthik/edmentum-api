using EdmentumBLL.DTO;
using EdmentumBLL.Manager;
using Microsoft.AspNetCore.Mvc;

namespace EdmentumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly StudentManager _studentManager;

        public StudentController(StudentManager studentManager)
        {
            _studentManager = studentManager;
        }

        [HttpPost]
        public IActionResult AddStudent(StudentDTO studentDto)
        {
            try
            {
                _studentManager.AddStudent(studentDto);
                return Ok("Student added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            try
            {
                var students = _studentManager.GetAllStudents();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
