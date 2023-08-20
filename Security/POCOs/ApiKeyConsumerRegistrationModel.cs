﻿using System.ComponentModel.DataAnnotations;

namespace StellarStreamAPI.Security.POCOs
{
    public class ApiKeyConsumerRegistrationModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;
    }
}
