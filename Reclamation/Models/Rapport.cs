using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Reclamation.Models
{
    public class Rapport
    {
        [Key] // Définit cette propriété comme clé primaire
        public int Id { get; set; }

        [Required] // Rend cette propriété obligatoire
        public DateTime DateRapport { get; set; }

        [Required] // Rend cette propriété obligatoire
        public int TotalTicketsVendus { get; set; }

        [Required] // Rend cette propriété obligatoire
        [Precision(18, 2)] // Définit la précision pour les nombres décimaux (18 chiffres, 2 après la virgule)
        public decimal RevenusTotaux { get; set; }

        [Required] // Rend cette propriété obligatoire
        [MaxLength(100)] // Limite la longueur de la chaîne à 100 caractères
        public string Responsable { get; set; } = string.Empty; // Initialisation par défaut pour éviter les nulls

        [MaxLength(100)] // Limite la longueur du nom de fichier image à 100 caractères
        public string ImageFileName { get; set; } = string.Empty; // Nom du fichier image
    }
}
