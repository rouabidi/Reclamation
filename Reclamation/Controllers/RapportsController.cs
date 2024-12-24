using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reclamation.Models;
using Reclamation.Services;
using System.IO;

namespace Reclamation.Controllers
{
    public class RapportsController : Controller
    {//ajout constructeur
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public RapportsController(ApplicationDbContext context , IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        //// Méthode Index pour afficher tous les rapports
        public IActionResult Index()
        {
            // Récupère tous les rapports de la base de données et les trie par Id décroissant
            var rapports = context.Rapports.OrderByDescending(r => r.Id).ToList();

            // Passe les rapports triés à la vue
            return View(rapports);
        }
        public IActionResult Create()
        {
            return View();
        }
        // /////////////////Traiter le formulaire de création (POST)
        [HttpPost]
        public IActionResult Create(RapportDto rapportDto)
        {
            // Vérifie si le fichier image est nul
            if (rapportDto.ImageFile == null)
            {
                // Ajoute une erreur de validation si le fichier image est requis
                ModelState.AddModelError("ImageFile", "Le fichier image est requis");
            }

            // Vérifie si les données sont valides
            if (!ModelState.IsValid)
            {
                return View(rapportDto); // Retourne à la vue avec les erreurs
            }
            //
            //
            // Générer un nom unique pour le fichier en utilisant la date et l'heure actuelles
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") +
                                 Path.GetExtension(rapportDto.ImageFile!.FileName);

            // Construire le chemin complet pour enregistrer l'image
            string imageFullPath = environment.WebRootPath + "/rapports/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
{
                rapportDto.ImageFile.CopyTo(stream);
            }
            //
            //// Créer un nouvel objet Rapport à partir des données fournies
            Rapport rapport = new Rapport
            {
                DateRapport = rapportDto.DateRapport,
                TotalTicketsVendus = rapportDto.TotalTicketsVendus,
                RevenusTotaux = rapportDto.RevenusTotaux,
                Responsable = rapportDto.Responsable,
                ImageFileName = newFileName // Nom de l'image précédemment généré
            };

            // Ajouter le rapport à la base de données
            context.Rapports.Add(rapport);

            // Sauvegarder les changements dans la base de données
            context.SaveChanges();


            // Redirige vers l'index après l'ajout
            return RedirectToAction("Index","Rapports");

        }
    }
}

