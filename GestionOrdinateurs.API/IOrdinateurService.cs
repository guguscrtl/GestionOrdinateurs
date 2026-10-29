using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API.Services;

public interface IOrdinateurService
{
    event EventHandler<OrdinateurAjouteEventArgs> OrdinateurAjoute;
    event EventHandler<OrdinateurSupprimeEventArgs> OrdinateurSupprime;
    event EventHandler<OrdinateurMisAJourEventArgs> OrdinateurMisAJour;

    void AjouterOrdinateur(Ordinateur ordinateur);
    void SupprimerOrdinateur(int id);
    void ModifierOrdinateur(int id, Ordinateur ordinateur);
    Dictionary<int, Ordinateur> ListerOrdinateurs();
    Ordinateur GetOrdinateurById(int id);
    List<Ordinateur> GetOrdinateursDispo();
    List<Ordinateur> GetOrdinateursByPrix();
    string Diagnostiquer(Ordinateur ordinateur);
    (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations) CalculerStatistiques();
}
