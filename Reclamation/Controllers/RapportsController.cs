using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reclamation.Models;
using Reclamation.Services;
using System.IO;
using System.Net;

namespace Reclamation.Controllers
{
    public class RapportsController : Controller
    {//ajout constructeur
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public RapportsController(ApplicationDbContext context, IWebHostEnvironment environment)
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
            return RedirectToAction("Index", "Rapports");

        }
        //pour edit
        public IActionResult Edit(int id)
        {
            // Récupérer le rapport correspondant à l'ID
            var rapport = context.Rapports.Find(id);

            // Si le rapport est introuvable, rediriger vers l'action Index
            if (rapport == null)
                return RedirectToAction("Index", "Rapports");

            // Créer un DTO (Data Transfer Object) à partir du rapport
            var rapportDto = new RapportDto()
            {
                DateRapport = rapport.DateRapport,
                TotalTicketsVendus = rapport.TotalTicketsVendus,
                RevenusTotaux = rapport.RevenusTotaux,
                Responsable = rapport.Responsable,
                // Note : Pas de champ pour ImageFileName car le DTO n'en a pas besoin
            };
            //Ajouter les valeurs dans ViewData
            ViewData["RapportID"] = rapport.Id; // ID du rapport
            ViewData["ImageFileName"] = rapport.ImageFileName; // Nom du fichier image
            ViewData["DateRapport"] = rapport.DateRapport.ToString("MM/dd/yyyy"); // Date du rapport formatée

            // Retourner la vue avec le DTO pour pré-remplir les champs
            return View(rapportDto);
        }


        [HttpPost]
        public IActionResult Edit(int id, RapportDto rapportDto)
        {
            var rapport = context.Rapports.Find(id);

            if (rapport == null)
                return RedirectToAction("Index", "Rapports");

            if (!ModelState.IsValid)
            {
                ViewData["RapportId"] = rapport.Id; // ID du rapport
                ViewData["ImageFileName"] = rapport.ImageFileName; // Nom du fichier image du rapport
                ViewData["DateRapport"] = rapport.DateRapport.ToString("MM/dd/yyyy"); // Date formatée du rapport
                return View(rapportDto);
            }

            // Mise à jour du fichier image si un nouveau fichier image est fourni
            string newFileName = rapport.ImageFileName;
            if (rapportDto.ImageFile != null)
            {
                // Générer un nouveau nom de fichier basé sur la date actuelle
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(rapportDto.ImageFile.FileName);

                // Définir le chemin complet de l'image dans le répertoire 'rapports'
                string imageFullPath = environment.WebRootPath + "/rapports/" + newFileName;

                // Créer le fichier image et y copier le contenu du fichier téléchargé
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    rapportDto.ImageFile.CopyTo(stream);
                }

                // Supprimer l'ancienne image si elle existe

                string oldImageFullPath = environment.WebRootPath + "/rapports/" + rapport.ImageFileName;

                System.IO.File.Delete(oldImageFullPath);
            }
            // Mise à jour du rapport dans la base de données
            rapport.DateRapport = rapportDto.DateRapport;
            rapport.TotalTicketsVendus = rapportDto.TotalTicketsVendus;
            rapport.RevenusTotaux = rapportDto.RevenusTotaux;
            rapport.Responsable = rapportDto.Responsable;
            rapport.ImageFileName = newFileName; // Nouveau nom de fichier image

            // Sauvegarder les modifications dans la base de données
            context.SaveChanges();
            // Rediriger vers l'index après l'édition
            return RedirectToAction("Index", "Rapports");
        }
        ////////////delete
        public IActionResult Delete(int id)
        {
            // Récupère le rapport par son ID
            var rapport = context.Rapports.Find(id);

            // Si le rapport n'existe pas, redirige vers l'index des rapports
            if (rapport == null)
                return RedirectToAction("Index", "Rapports");

            // Construire le chemin complet pour l'image
            string imageFullPath = environment.WebRootPath+"/rapports/"+ rapport.ImageFileName;

            // Supprimer l'image du disque si elle existe
            System.IO.File.Delete(imageFullPath);
           

            // Supprimer le rapport de la base de données
            context.Rapports.Remove(rapport);

            // Sauvegarder les changements dans la base de données
            context.SaveChanges(true);

            // Redirige vers l'index après suppression
            return RedirectToAction("Index", "Rapports");
        }





    }

}