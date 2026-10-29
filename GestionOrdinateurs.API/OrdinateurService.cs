namespace GestionOrdinateurs.API;

using GestionOrdinateurs.API.Services;
using GestionOrdinateurs.Shared;

public class OrdinateurService : IOrdinateurService
{
    private readonly ParcInformatiqueContext _context;
    private readonly JournalService _journal;

    public event EventHandler<OrdinateurAjouteEventArgs>? OrdinateurAjoute;
    public event EventHandler<OrdinateurSupprimeEventArgs>? OrdinateurSupprime;
    public event EventHandler<OrdinateurMisAJourEventArgs>? OrdinateurMisAJour;

    public OrdinateurService(ParcInformatiqueContext context, JournalService journal)
    {
        _context = context;
        _journal = journal;
    }

    public void AjouterOrdinateur(Ordinateur ordinateur)
    {
        _context.Ordinateurs.Add(ordinateur);
        _context.SaveChanges();

        _journal.Ecrire($"{DateTime.Now:dd/MM/yyyy} - {ordinateur.Nom} ajouté");
        OrdinateurAjoute?.Invoke(this, new OrdinateurAjouteEventArgs());
    }

    public void SupprimerOrdinateur(int id)
    {
        var ordinateur = _context.Ordinateurs.FirstOrDefault(o => o.Id == id);
        if (ordinateur != null)
        {
            _context.Ordinateurs.Remove(ordinateur);
            _context.SaveChanges();

            _journal.Ecrire($"{DateTime.Now:dd/MM/yyyy} - {ordinateur.Nom} supprimé");
            OrdinateurSupprime?.Invoke(this, new OrdinateurSupprimeEventArgs());
        }
    }

    public void ModifierOrdinateur(int id, Ordinateur ordinateurModifie)
    {
        var ordinateur = _context.Ordinateurs.FirstOrDefault(o => o.Id == id);
        if (ordinateur != null)
        {
            _context.Entry(ordinateur).CurrentValues.SetValues(ordinateurModifie);
            _context.SaveChanges();

            _journal.Ecrire($"{DateTime.Now:dd/MM/yyyy} - {ordinateur.Nom} modifié");
            OrdinateurMisAJour?.Invoke(this, new OrdinateurMisAJourEventArgs());
        }
    }

    public Dictionary<int, Ordinateur> ListerOrdinateurs()
    {
        return _context.Ordinateurs.ToDictionary(o => o.Id, o => o);
    }

    public Ordinateur GetOrdinateurById(int id)
    {
        return _context.Ordinateurs.FirstOrDefault(o => o.Id == id);
    }

    public List<Ordinateur> GetOrdinateursDispo()
    {
        return _context.Ordinateurs.Where(o => o.Stock > 0).ToList();
    }

    public List<Ordinateur> GetOrdinateursByPrix()
    {
        return _context.Ordinateurs.AsEnumerable().OrderByDescending(o => o.Prix).ToList(); // AsNumerable permet le tri par prix
    }

    public string Diagnostiquer(Ordinateur ordinateur)
    {
        return ordinateur switch
        {
            { Etat: EtatOrdinateur.HorsService } => "Remplacement",
            { Etat: EtatOrdinateur.Lent, Caracteristiques: { RamGo: < 8 } } => "Ajouter RAM",
            _ => "Aucune action"
        };
    }

    public (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations) CalculerStatistiques()
    {
        var total = _context.Ordinateurs.Count();
        var enStock = _context.Ordinateurs.Count(o => o.Stock > 0);
        var valeur = _context.Ordinateurs.Sum(o => o.Prix * o.Stock);
        var nbRep = _context.Reparations.Count();

        return (total, enStock, valeur, nbRep);
    }
}
