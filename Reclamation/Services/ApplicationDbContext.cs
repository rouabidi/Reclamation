using Microsoft.EntityFrameworkCore;
using Reclamation.Models;

namespace Reclamation.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        // Définir le DbSet pour le modèle Rapport
        public DbSet<Rapport> Rapports { get; set; }
    }
}
