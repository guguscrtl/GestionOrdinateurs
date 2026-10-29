namespace GestionOrdinateurs.API;

using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

// Record pour l'historique des réparations
public record HistoriqueReparationRecord(
    int OrdinateurId,
    string NomOrdinateur,
    string Technicien,
    string TypeReparation,
    DateTime DateIntervention
);

public class ReparationService
{
    private readonly ParcInformatiqueContext _context;
    private readonly JournalService _journal;

    public ReparationService(ParcInformatiqueContext context, JournalService journal)
    {
        _context = context;
        _journal = journal;
    }

    public async Task SimulerReparationAsync(Ordinateur ordinateur)
    {
        // Création objet reparation
        Reparation test = new Reparation
        {
            UnOrdinateur = ordinateur,
            TypeReparation = "Remplacement SSD",
            DateDebut = DateTime.Now,
            Statut = "En cours",
            Technicien = "Jean Dupont"
        };
        var reparation = await _context.Reparations.AddAsync(test);
        await Task.Delay(2000);
        reparation.Entity.Statut = "Terminée";
        await _context.SaveChangesAsync();

        _journal.Ecrire($"{DateTime.Now:dd/MM/yyyy} - {ordinateur.Nom} réparé");
        Console.WriteLine($"{ordinateur.Nom} réparé");
    }

    public async Task<List<Reparation>> ListerReparationsAsync()
    {
        return await _context.Reparations
            .Include(r => r.UnOrdinateur)
            .ToListAsync();
    }

    // Retourne l'historique des reparations sous forme de records
    public async Task<List<HistoriqueReparationRecord>> GetHistoriqueReparationsAsync()
    {
        var reparations = await _context.Reparations
            .Include(r => r.UnOrdinateur)
            .ToListAsync();

        return reparations.Select(r => new HistoriqueReparationRecord(
            r.UnOrdinateur.Id,
            r.UnOrdinateur.Nom,
            r.Technicien,
            r.TypeReparation,
            r.DateDebut
        )).ToList();
    }
}
