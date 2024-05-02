using EdmentumBLL.DTO;
using EdmentumDAL;
using EdmentumDAL.ModelClass;

namespace EdmentumBLL.Manager
{
    public class TutorManager
    {
        private readonly EdmentumContext _context;

        public TutorManager(EdmentumContext context)
        {
            _context = context;
        }

        public void AddTutor(TutorDTO tutorDto)
        {
            // Map DTO to entity
            var tutor = new Tutor
            {
                TutorName = tutorDto.TutorName,
                TutorId = tutorDto.TutorId
            };

            // Adding tutor to context and save changes
            _context.Tutors.Add(tutor);
            _context.SaveChanges();
        }

        public IEnumerable<TutorDTO> GetAllTutors()
        {
            // Retrieve all tutors from the context and map them to DTOs
            var tutors = _context.Tutors.ToList();
            return tutors.Select(t => new TutorDTO
            {
                Id = t.Id,
                TutorName = t.TutorName,
                TutorId = t.TutorId
            });
        }
    }
}
