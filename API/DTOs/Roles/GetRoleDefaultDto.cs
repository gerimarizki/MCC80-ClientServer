﻿namespace API.DTOs.Roles
{
    public class GetRoleDefaultDto
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set;}
    }
}
