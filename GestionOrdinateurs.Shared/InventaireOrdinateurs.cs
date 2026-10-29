namespace GestionOrdinateurs.Shared;

public class InventaireOrdinateurs
{
    public List<Ordinateur> Ordinateurs { get; set; } = new();

    public Ordinateur? this[int id] => Ordinateurs.FirstOrDefault(o => o.Id == id);

    public Ordinateur? this[string nom] => Ordinateurs.FirstOrDefault(o => o.Nom == nom);
}
