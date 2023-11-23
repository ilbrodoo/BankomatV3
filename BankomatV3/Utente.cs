using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankomatV3
{
    public class Utente
    {
        public Utente(string username , string password , bool bloccato , int idBancaCorrente)
        {
            UsernName = username;
            Password = password;
            Bloccato = bloccato;
            IdBanca = idBancaCorrente;
        }
        public string UsernName { get; set; }
        public string Password { get; set; }
        public int Tentativi { get; set; }
        public int IdBanca { get; set; }
        public bool Bloccato { get; set; }
        public DateTime DataBlocco { get; set; }

        public bool ControllaSeBloccato()
        {
            if (Bloccato)
            {
                if ((DateTime.Now - DataBlocco).TotalDays < 30) { return true; }
               
            }
            return false;
        }
        public bool CheckPassword (string password)
        {
            if (ControllaSeBloccato())
            {
                return false;
            }
            if (Password == password)
            {
                return true;
            }
            Tentativi +=1;
            if(Tentativi == 3)
            {
                Bloccato=true;
            }
            return false;
        }

    }
    
}
