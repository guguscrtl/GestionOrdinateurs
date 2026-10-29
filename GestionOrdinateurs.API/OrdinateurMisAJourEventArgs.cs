namespace GestionOrdinateurs.API;

public class OrdinateurMisAJourEventArgs : EventArgs
{
    public string Message { get; init; } = "Evenement mise à jour";
}
