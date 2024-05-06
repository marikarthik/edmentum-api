using EdmentumBLL.DTO;
using EdmentumDAL;
using EdmentumDAL.ModelClass;

namespace EdmentumBLL.Manager
{
    public class StudentManager
    {
        private readonly EdmentumContext _context;

        public StudentManager(EdmentumContext context)
        {
            _context = context;
        }
        public void AddStudent(StudentDTO studentDto)
        {
            // Map 
            var student = new Student
            {
                StudentName = studentDto.StudentName               
            };
            // Adding student to context and save changes
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public IEnumerable<StudentDTO> GetAllStudents()
        {
            // Retrieve all students from the context and map them to DTOs
            var students = _context.Students.ToList();
            return students.Select(s => new StudentDTO
            {
                StudentId = s.Id,
                StudentName = s.StudentName
            });
        }
    }
}
