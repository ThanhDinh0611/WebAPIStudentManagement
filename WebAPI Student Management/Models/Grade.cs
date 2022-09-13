using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_Student_Management.Models
{
    public class Grade
    {
        [Key]
        public int StudentId { get; set; }
        public decimal English { get; set; }
        public decimal Informatic { get; set; }
        public decimal PE { get; set; }
    }
}
