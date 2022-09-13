using System.ComponentModel.DataAnnotations;

namespace WebAPI_Student_Management.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set;}
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        
        // 0: Male, 1: Female
        public bool Gender { get; set; }
        public string Address { get; set; }
        public Grade Grade { get; set; }

    }
}
