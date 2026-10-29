using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionOrdinateurs.Shared;

public enum EtatOrdinateur
{
    EnService,
    EnPanne,
    EnMaintenance,
    HorsService,
    Lent,
    Reparable
}

[Table("Ordinateur")]
public class Ordinateur
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public EtatOrdinateur Etat { get; set; }

    [DefaultValue(0)]
    public int Stock { get; set; }

    [DefaultValue(0)]
    public decimal Prix { get; set; }

    // indique si l'ordinateur a été vendu
    [DefaultValue(false)]
    public bool Vendu { get; set; } = false;

    public Caracteristiques Caracteristiques { get; set; }

    public List<Reparation> Reparations { get; set; } = new();

    public Ordinateur() { }

    public Ordinateur(int id, string nom, EtatOrdinateur etat, int stock, decimal prix)
    {
        Id = id;
        Nom = nom;
        Etat = etat;
        Stock = stock;
        Prix = prix;
    }

    public void afficherDetails()
    {
        Console.WriteLine($"ID: {Id}, Nom: {Nom}, Etat: {Etat}, Stock: {Stock}, Prix: {Prix}");
    }
}
