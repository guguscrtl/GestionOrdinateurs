namespace GestionOrdinateurs.API;

public class OrdinateurAjouteEventArgs : EventArgs
{
    public string Message { get; init; } = "Evenement d'ajout";
}
