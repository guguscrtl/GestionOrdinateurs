namespace GestionOrdinateurs.Shared;

public record Caracteristiques
{
    public int RamGo { get; set; }
    public int StockageGo { get; set; }
    public TypeOrdinateur TypeOrdi { get; set; }

    public Caracteristiques() { }

    public Caracteristiques(int ramGo, int stockageGo, TypeOrdinateur typeOrdi)
    {
        RamGo = ramGo;
        StockageGo = stockageGo;
        TypeOrdi = typeOrdi;
    }
}
