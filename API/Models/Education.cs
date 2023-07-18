﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Education : BaseEntity
    {

        public string Major { get; set; }


        public string Degree { get; set; }

 
        public float GPA { get; set; }

  
        public Guid UniversityGuid { get; set; }


    }
}
