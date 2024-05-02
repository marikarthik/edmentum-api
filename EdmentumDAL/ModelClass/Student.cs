using System.ComponentModel.DataAnnotations;

namespace EdmentumDAL.ModelClass
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string StudentName { get; set; }
        public int  StudentId{ get; set; }
    }
}
