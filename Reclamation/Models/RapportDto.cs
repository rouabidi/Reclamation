using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // Assurez-vous d'avoir cette directive pour IFormFile

namespace Reclamation.Models
{
    public class RapportDto
    {
        [Required]
        public DateTime DateRapport { get; set; }

        [Required]
        public int TotalTicketsVendus { get; set; }

        [Required]
        public decimal RevenusTotaux { get; set; }

        [Required]
        [MaxLength(100)]
        public string Responsable { get; set; } = string.Empty;

        // Propriété pour gérer l'upload d'image
        public IFormFile? ImageFile { get; set; } // Remplacez "ImageFileName" par "ImageFile"
    }
}
