﻿namespace API.DTOs.Accounts
{
    public class NewAccountDto
    {

        public Guid Guid { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public int OTP { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
