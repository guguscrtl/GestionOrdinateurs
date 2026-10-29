using GestionOrdinateurs.API;
using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace GestionOrdinateurs.Tests;

public class UnitTest1
{
    private ParcInformatiqueContext CreerContexte()
    {
        var options =
            new DbContextOptionsBuilder<ParcInformatiqueContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        return new ParcInformatiqueContext(options);
    }

    // Test de création d'une réparation
    [Fact]
    public async Task SimulerReparation_CreeUneReparation()
    {
        using var context = CreerContexte();
        var journal = new JournalService();
        var service = new ReparationService(context, journal);

        var ordinateur = new Ordinateur { Id = 1, Nom = "PC Test", Etat = EtatOrdinateur.EnPanne, Stock = 1, Prix = 500 };
        context.Ordinateurs.Add(ordinateur);
        await context.SaveChangesAsync();

        await service.SimulerReparationAsync(ordinateur);

        context.Reparations.Should().ContainSingle(r => r.OrdinateurId == 1); // Vérification qu'une réparation a été créée
    }
}
