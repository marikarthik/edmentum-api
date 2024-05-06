using System.ComponentModel.DataAnnotations;

namespace EdmentumDAL.ModelClass
{
    public class Tutor
    {
        [Key]
        public int TutorId { get; set; }
        public string TutorName { get; set; }
    }
}
