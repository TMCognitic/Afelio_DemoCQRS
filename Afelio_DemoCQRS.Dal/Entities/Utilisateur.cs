using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afelio_DemoCQRS.Dal.Entities
{
#nullable disable
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public DateOnly Anniversaire { get; set; }
        public string Passwd { get; set; }
    }
#nullable enable
}
