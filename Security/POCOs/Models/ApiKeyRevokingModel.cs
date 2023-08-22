using System.ComponentModel.DataAnnotations;

namespace StellarStreamAPI.Security.POCOs.Models
{
    public class ApiKeyRevokingModel
    {
        [Required(ErrorMessage = "API key ID is required.")]
        public long KeyId { get; set; }
    }
}
