using GestionOrdinateurs.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionOrdinateurs.API.Controllers;

// réservé aux admins uniquement
[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/reparations")]
public class ReparationController : ControllerBase
{
    private readonly ILogger<ReparationController> _logger;
    private readonly ReparationService _reparService;
    private readonly IOrdinateurService _ordinateurService;
    private readonly StockService _stockService;

    public ReparationController(ILogger<ReparationController> logger, ReparationService reparService, IOrdinateurService ordinateurService, StockService stockService)
    {
        _logger = logger;
        _reparService = reparService;
        _ordinateurService = ordinateurService;
        _stockService = stockService;
    }

    [HttpGet("GetStockSSD")]
    public int GetStockSSD()
    {
        Parallel.For(
            0,
            10,
            i => _stockService.ConsommerSSD());

        return _stockService.StockSSD;
    }

    [HttpPost("SimulerReparation")]
    public async Task<IActionResult> SimulerReparation()
    {
        var listOrdinateurs = _ordinateurService.ListerOrdinateurs().Values.ToList();

        if (listOrdinateurs.Count == 0)
            return BadRequest("Aucun ordinateur disponible pour la simulation.");

        // On prend 3 pc parmis la liste pour tester la reparation
        foreach (var ordinateur in listOrdinateurs.Take(3))
        {
            await _reparService.SimulerReparationAsync(ordinateur);
        }
        _logger.LogInformation("Reparations terminee");

        return Ok();
    }

    [HttpGet("ListerReparations")]
    public async Task<ActionResult<List<Reparation>>> ListerReparations()
    {
        var reparations = await _reparService.ListerReparationsAsync();
        _logger.LogInformation("Affichage des listes des reparations par ordi");
        return Ok(reparations);
    }

    [HttpGet("historique")]
    public async Task<IActionResult> GetHistorique()
    {
        var historique = await _reparService.GetHistoriqueReparationsAsync();
        _logger.LogInformation("Affichage de l'historique des reparations");
        return Ok(historique);
    }
}
