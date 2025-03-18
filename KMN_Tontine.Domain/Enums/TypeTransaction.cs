using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMN_Tontine.Domain.Enums
{
    public enum TypeTransaction
    {
        Cotisation,        // Un membre crédite son compte d’opération (via une promesse de paiement)
        Retrait,           // Un membre ou l’association retire des fonds
        Penalite,          // Débit pour retard ou non-paiement
        Versement,         // Paiement effectué pour solder une dette
        TransfertInterne,  // Déplacement d’argent entre comptes internes (ex: fonds de solidarité)
        DépenseAssociation // Débit du compte d’opération de l’association pour une dépense externe
    }
}
