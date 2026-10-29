namespace GestionOrdinateurs.API;

public class OrdinateurSupprimeEventArgs : EventArgs
{
    public String Message { get; init; } = "Evenement suppression";
}
