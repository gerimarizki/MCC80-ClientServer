using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Education
    {
        public Guid Guid { get; set; }


        public string Major { get; set; }


        public string Degree { get; set; }

 
        public float GPA { get; set; }

  
        public Guid UniversityGuid { get; set; }

   
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }


    }
}
