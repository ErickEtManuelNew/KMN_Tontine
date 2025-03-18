namespace KMN_Tontine.Application.DTOs;

public class TransactionDTO
{
    public int Id { get; set; }
    public string MembreId { get; set; }
    public int CompteId { get; set; }
    public decimal Montant { get; set; }
    public string TypeTransaction { get; set; } // "Crédit" ou "Débit"
    public DateTime DateTransaction { get; set; }
}
