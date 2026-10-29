namespace GestionOrdinateurs.API;

public class StockService
{
    private readonly object _verrou = new object(); // Verrou protégeant l'accès à StockSSD

    public int StockSSD { get; set; } = 10; // Stock initial de SSD

    public void ConsommerSSD()
    {
        // Un seul thread à la fois peut lire puis modifier le stock
        lock (_verrou)
        {
            var stock = StockSSD; // Lecture du stock actuel
            Task.Delay(10).Wait(); // Délai avant de diminuer le stock (simule un traitement)
            StockSSD = stock - 1; // Écriture du nouveau stock
        }
    }
}
