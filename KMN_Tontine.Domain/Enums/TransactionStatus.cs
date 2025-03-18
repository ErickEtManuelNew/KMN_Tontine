using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Domain.Enums
{
    public enum TransactionStatus
    {
        EnAttente,  // Transaction créée, en attente de validation
        Valide,     // Confirmée par le système
        Rejetee,    // Rejetée par le gestionnaire
        Expiree     // Non validée à temps
    }
}
