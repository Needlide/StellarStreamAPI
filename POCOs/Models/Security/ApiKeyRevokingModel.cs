﻿using System.ComponentModel.DataAnnotations;

namespace StellarStreamAPI.POCOs.Models.Security
{
    public class ApiKeyRevokingModel
    {
        [Required(ErrorMessage = "API key ID is required.")]
        public string KeyId { get; set; }
    }
}
