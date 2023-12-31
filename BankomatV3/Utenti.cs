//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace BankomatV3
{
    using System;
    using System.Collections.Generic;
    
    public partial class Utenti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Utenti()
        {
            this.ContiCorrentes = new HashSet<ContiCorrente>();
        }
    
        public long Id { get; set; }
        public long IdBanca { get; set; }
        public string NomeUtente { get; set; }
        public string Password { get; set; }
        public bool Bloccato { get; set; }
        public Nullable<int> Tentativi { get; set; }

        public Dictionary<Utenti, int> _tentativiPerUtente = new Dictionary<Utenti, int>();
        public int _tentativiDiAccessoPermessi = 3;
        private int _tentativiDiAccessoErrati = 0;

        public virtual Banche Banche { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContiCorrente> ContiCorrentes { get; set; }

        public void IncrementaTentativo(BancomatEntities2 ctx)
        {
            if (_tentativiPerUtente.ContainsKey(this))
        {
            _tentativiPerUtente[this]++;
        }
        else
        {
            _tentativiPerUtente[this] = 1;
        }

                if (_tentativiPerUtente[this] >= _tentativiDiAccessoPermessi)
                {
                    Bloccato = true;
                ctx.SaveChanges();
            }
}


public int TentativiDiAccessoResidui
        {
            get
            {
                return _tentativiDiAccessoPermessi - _tentativiDiAccessoErrati;
            }
        }

        public int TentativiDiAccessoErrati
        {
            get => _tentativiDiAccessoErrati;
            set
            {
                _tentativiDiAccessoErrati = value;
                if (_tentativiDiAccessoErrati >= _tentativiDiAccessoPermessi)
                {
                    Bloccato = true;
                }
            }
        }
    }
}


