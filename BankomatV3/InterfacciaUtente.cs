using System.Collections.Generic;
using System;
using System.Linq;

namespace BankomatV3
{
     class InterfacciaUtente
    {
        enum Richiesta
        {
            SchermataDiBenvenuto,
            Login,
            MenuPrincipale,
            Versamento,
            ReportSaldo,
            Prelievo,
            Uscita
        };

        private Banche _bancaCorrente;
        
        
        private void StampaIntestazione(string titoloMenu)
        {
            Console.Clear();
            Console.WriteLine("**************************************************");
            Console.WriteLine("*              Bankomat Simulator               *");
            Console.WriteLine("**************************************************");
            Console.WriteLine("".PadLeft((50 - titoloMenu.Length) / 2)
                + titoloMenu);
            Console.WriteLine("--------------------------------------------------");
            return;
        }

            
        private int ScegliVoceMenu(int min, int max)
        {
            string rispostaUtente;


            Console.Write("Scelta: ");
            rispostaUtente = Console.ReadKey().KeyChar.ToString();
            if (!Int32.TryParse(rispostaUtente, out int scelta) ||
                !(min <= scelta && scelta <= max))
            {
                scelta = -1;
                Console.WriteLine("");
                Console.WriteLine($"Scelta non consentita - {rispostaUtente}");
                Console.Write("Premere un tasto per proseguire");
                Console.ReadKey();
            }
            return scelta;
        }

       
        private int SchermataDiBenvenuto(BancomatEntities1 ctx)
        {
            var countBanche = 0;
            int scelta = -1;
            while (scelta == -1)
            {
                StampaIntestazione("Selezione Banca");

                foreach (Banche banche in ctx.Banches)
                {
                    Console.WriteLine($"{banche.Id} - {banche.Nome}");
                    countBanche++;
                }

                scelta = ScegliVoceMenu(0, countBanche);
            }

            return scelta;

        }


        public void Esegui(BancomatEntities1 ctx)
        {
            int rispostaUtente = 0;
            Richiesta richiesta = Richiesta.SchermataDiBenvenuto;

            while (richiesta != Richiesta.Uscita)
            {
                switch (richiesta)
                {
                    case Richiesta.SchermataDiBenvenuto:
                        rispostaUtente = SchermataDiBenvenuto(ctx);

                        if (rispostaUtente == 0)
                        {
                            richiesta = Richiesta.Uscita;
                        }
                        else
                        {
                            _bancaCorrente = ctx.Banches.FirstOrDefault(b => b.Id == rispostaUtente);

                        }
                        
                        richiesta = Richiesta.Login;

                        break;
                    case Richiesta.Login:
                        if (Login(ctx))
                            richiesta = Richiesta.MenuPrincipale;
                        else
                            richiesta = Richiesta.SchermataDiBenvenuto;
                        break;
                    //case Richiesta.MenuPrincipale:
                    //    switch (MenuPrincipale())
                    //    {
                    //        case Banca.Funzionalita.Uscita:
                    //            richiesta = Richiesta.SchermataDiBenvenuto;
                    //            break;
                    //        case Banca.Funzionalita.Versamento:
                    //            richiesta = Richiesta.Versamento;
                    //            break;
                    //        case Banca.Funzionalita.ReportSaldo:
                    //            richiesta = Richiesta.ReportSaldo;
                    //            break;
                    //        case Banca.Funzionalita.Prelievo:
                    //            richiesta = Richiesta.Prelievo;
                    //            break;
                    //    }
                    //    break;
                    //case Richiesta.Versamento:
                    //    bool esito = Versamento();
                    //    if (esito && _bancaCorrente.Banche_Funzionalita
                    //        .ContainsValue(Banca.Funzionalita.ReportSaldo))
                    //        richiesta = Richiesta.ReportSaldo;
                    //    else
                    //        richiesta = Richiesta.MenuPrincipale;
                    //    break;
                    //case Richiesta.ReportSaldo:
                    //    ReportSaldo();
                    //    richiesta = Richiesta.MenuPrincipale;
                    //    break;
                    //case Richiesta.Prelievo:
                    //    Prelievo();
                    //    richiesta = Richiesta.MenuPrincipale;
                    //break;
                    default:
                        break;
                }
            }
        }
    }
}