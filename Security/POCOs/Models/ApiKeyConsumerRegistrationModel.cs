﻿using System.ComponentModel.DataAnnotations;

namespace StellarStreamAPI.Security.POCOs.Models
{
    public class ApiKeyConsumerRegistrationModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }
}