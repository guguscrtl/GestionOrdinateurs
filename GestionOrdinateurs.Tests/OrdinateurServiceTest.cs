using GestionOrdinateurs.API;
using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace GestionOrdinateurs.Tests;

public class OrdinateurServiceTest
{
    private ParcInformatiqueContext CreerContexte()
    {
        var options =
            new DbContextOptionsBuilder<ParcInformatiqueContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        return new ParcInformatiqueContext(options);
    }

    private JournalService CreerJournalService() => new JournalService();

    // Test pour la méthode AjouterOrdinateur
    [Fact]
    public void AjouterOrdinateur_AjouteLOrdinateurAuParc()
    {
        using var context = CreerContexte();
        var service = new OrdinateurService(context, CreerJournalService());
        var ordinateur = new Ordinateur(100, "Laptop Test", EtatOrdinateur.EnService, 2, 1000);

        service.AjouterOrdinateur(ordinateur); // Ajout de l'ordinateur au parc

        context.Ordinateurs.Should().ContainSingle(o => o.Id == 100); // Vérification que l'ordinateur a été ajouté au parc
    }

    // Test pour la méthode SupprimerOrdinateur Existant
    [Fact]
    public void SupprimerOrdinateur_Existant_RetireLOrdinateurDuParc()
    {
        using var context = CreerContexte();
        var service = new OrdinateurService(context, CreerJournalService());
        var ordinateur = new Ordinateur(101, "PC Test", EtatOrdinateur.EnService, 1, 500);
        context.Ordinateurs.Add(ordinateur); // Ajout de l'ordinateur au parc pour le test
        context.SaveChanges();

        context.Ordinateurs.Should().Contain(o => o.Id == 101); // Vérification que l'ordinateur est présent avant la suppression

        service.SupprimerOrdinateur(101); // Suppression de l'ordinateur

        context.Ordinateurs.Should().NotContain(o => o.Id == 101); // Vérification que l'ordinateur a été retiré du parc
    }

    // Test pour la méthode SupprimerOrdinateur Inexistant
    [Fact]
    public void SupprimerOrdinateur_Inexistant_NeModifiePasLeParc()
    {
        using var context = CreerContexte();
        var service = new OrdinateurService(context, CreerJournalService());
        context.Ordinateurs.Add(new Ordinateur(102, "PC Test", EtatOrdinateur.EnPanne, 1, 500));
        context.SaveChanges();

        int nombreAvant = context.Ordinateurs.Count();
        context.Ordinateurs.Should().NotContain(o => o.Id == 999); // Vérification que l'ordinateur n'existe pas dans le parc

        service.SupprimerOrdinateur(999); // Suppression d'un ordinateur inexistant

        context.Ordinateurs.Count().Should().Be(nombreAvant); // Vérification que le nombre d'ordinateurs reste inchangé
    }
}
