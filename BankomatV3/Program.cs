using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankomatV3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ctx = new BancomatEntities2();
            InterfacciaUtente interfacciaUtente = new InterfacciaUtente();

            interfacciaUtente.Esegui(ctx);
        }
    }
}
