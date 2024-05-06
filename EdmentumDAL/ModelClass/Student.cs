using System.ComponentModel.DataAnnotations;

namespace EdmentumDAL.ModelClass
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string StudentName { get; set; }
    }
}
