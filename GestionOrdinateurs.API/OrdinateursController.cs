using GestionOrdinateurs.API.Services;
using GestionOrdinateurs.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionOrdinateurs.API.Controllers;

// tous les endpoints nécessitent un token valide
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdinateursController : ControllerBase
{
    void AfficherMessageAjout(object? sender, OrdinateurAjouteEventArgs e)
    {
        Console.WriteLine($"{e.Message} : nouvel ordinateur ajouté");
    }

    void AfficherMessageSuppression(object? sender, OrdinateurSupprimeEventArgs e)
    {
        Console.WriteLine($"{e.Message} : un ordinateur a été supprimé");
    }

    void AfficherMessageMiseAJour(object? sender, OrdinateurMisAJourEventArgs e)
    {
        Console.WriteLine($"{e.Message} : un ordinateur a été modifié");
    }

    private readonly ILogger<OrdinateursController> _logger;
    private readonly IOrdinateurService _ordinateurService;
    private readonly IConfiguration _configuration;
    private readonly ParcInformatiqueContext _context;

    public OrdinateursController(ILogger<OrdinateursController> logger, IOrdinateurService ordinateurService, IConfiguration configuration, ParcInformatiqueContext context)
    {
        _logger = logger;
        _ordinateurService = ordinateurService;
        _configuration = configuration;
        _context = context;

        _ordinateurService.OrdinateurAjoute += AfficherMessageAjout;
        _ordinateurService.OrdinateurSupprime += AfficherMessageSuppression;
        _ordinateurService.OrdinateurMisAJour += AfficherMessageMiseAJour;
    }

    [HttpPost]
    public void AjouterOrdinateur(Ordinateur ordinateur)
    {
        _ordinateurService.AjouterOrdinateur(ordinateur);
        _logger.LogInformation($"Ordinateur ajouté : {ordinateur.Id}");
    }

    [HttpDelete("{id}")]
    public void SupprimerOrdinateur(int id)
    {
        _ordinateurService.SupprimerOrdinateur(id);
        _logger.LogInformation($"Ordinateur supprimé : {id}");
    }

    [HttpPut("{id}")]
    public void ModifierOrdinateur(int id, Ordinateur ordinateurModifie)
    {
        _ordinateurService.ModifierOrdinateur(id, ordinateurModifie);
        _logger.LogInformation($"Ordinateur modifié : {id}");
    }

    [HttpGet]
    public Dictionary<int, Ordinateur> ListerOrdinateurs()
    {
        _logger.LogInformation("Liste des ordinateurs récupérée");
        return _ordinateurService.ListerOrdinateurs();
    }

    [HttpGet("{id}")]
    public IActionResult GetOrdinateurById(int id)
    {
        var ordinateur = _ordinateurService.GetOrdinateurById(id);
        if (ordinateur == null)
        {
            return NotFound();
        }
        _logger.LogInformation($"Ordinateur récupéré : {id}");
        return Ok(ordinateur);
    }

    [HttpGet("/api/ordinateurs/info")]
    public IActionResult GetInfo()
    {
        var appName = _configuration["Application:Name"];
        var appVersion = _configuration["Application:Version"];
        _logger.LogInformation("Informations sur l'application récupérées");
        return Ok(new { Name = appName, Version = appVersion });
    }

    [HttpGet("/api/ordinateurs/disponibles")]
    public IActionResult GetOrdinateursDispo()
    {
        var ordinateurs = _ordinateurService.GetOrdinateursDispo();
        _logger.LogInformation("Ordinateurs disponibles récupérés");
        return Ok(ordinateurs);
    }

    [HttpGet("/api/ordinateurs/prix")]
    public IActionResult GetOrdinateursByPrix()
    {
        var ordinateurs = _ordinateurService.GetOrdinateursByPrix();
        _logger.LogInformation("Ordinateurs triés par prix récupérés");
        return Ok(ordinateurs);
    }

    [HttpGet("{id}/diagnostic")]
    public IActionResult GetDiagnostic(int id)
    {
        var ordinateur = _ordinateurService.GetOrdinateurById(id);
        if (ordinateur == null) return NotFound();
        var diagnostic = _ordinateurService.Diagnostiquer(ordinateur);
        return Ok(diagnostic);
    }

    [HttpGet("statistiques")]
    public IActionResult GetStatistiques()
    {
        var (total, enStock, valeurStock, nbReparations) = _ordinateurService.CalculerStatistiques();
        return Ok(new { TotalOrdinateur = total, OrdiDisponibles = enStock, ValeurStock = valeurStock, NbReparations = nbReparations });
    }

    [HttpGet("inventaire/{nom}")]
    public IActionResult Chercher(string nom)
    {
        var inventaire = new InventaireOrdinateurs();
        inventaire.Ordinateurs = _context.Ordinateurs.ToList();

        var ordinateur = inventaire[nom]; // chercher un ordinateur dans l'inventaire par son nom

        return Ok(ordinateur);
    }
}
