﻿namespace EntertainmentAPI.Models.DTOs
{
    public class UserUpdateDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
