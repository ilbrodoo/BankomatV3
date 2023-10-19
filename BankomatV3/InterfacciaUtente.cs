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
        
        private Utenti _utenteCorrente;
        private Banche _bancaCorrente;
        private List<Banche_Funzionalita> _funzionalitaCorrenti;
        
       
        
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

       
        private int SchermataDiBenvenuto(BancomatEntities2 ctx)
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

        private bool Login(BancomatEntities2 ctx)
        {
            bool autenticato = false;

            Utenti credenziali = new Utenti();

            StampaIntestazione($"Login - {_bancaCorrente.Nome}");

            Console.Write("Nome utente: ");
            credenziali.NomeUtente = Console.ReadLine();
            Console.Write("Password: ");
            credenziali.Password = Console.ReadLine();
            int idBancaCorrente = Convert.ToInt32(_bancaCorrente.Id);

            EsitoLogin esitoLogin = _bancaCorrente.Login(credenziali, idBancaCorrente,  out Utenti utente, ctx);

            switch (esitoLogin)
            {
                case EsitoLogin.PasswordErrata:
                    utente.IncrementaTentativo(ctx);
                    int tentativiFatti = utente._tentativiPerUtente[utente];
                    int tentativiMancanti = utente._tentativiDiAccessoPermessi - tentativiFatti;

                    Console.WriteLine($"Password errata - Tentativi fatti: {tentativiFatti}, Tentativi mancanti per il blocco: {tentativiMancanti}");

                    if (tentativiMancanti == 0)
                    {
                        Console.WriteLine("*** Account utente bloccato ***");
                    }
                    Console.Write("Premere un tasto per proseguire");
                    Console.ReadKey();
                    break;
                case EsitoLogin.UtentePasswordErrati:
                    Console.WriteLine("Utente o password errati");
                    Console.Write("Premere un tasto per proseguire");
                    Console.ReadKey();
                    break;
                case EsitoLogin.AccountBloccato:
                    Console.WriteLine("*** Account utente bloccato ***");
                    Console.Write("Premere un tasto per proseguire");
                    Console.ReadKey();
                    break;
                case EsitoLogin.AccessoConsentito:
                    _utenteCorrente = utente;
                    autenticato = true;
                    break;
            }

            return autenticato;
        }


        private Funzionalita MenuPrincipale()
        {
            int scelta = -1;

            while (scelta == -1)
            {
                StampaIntestazione($"Menu principale - {_bancaCorrente.Nome}");

                foreach (var funzionalita in _bancaCorrente.Banche_Funzionalita)
                {
                    Console.WriteLine($"{funzionalita.Id} - {funzionalita.Funzionalita.Nome}");
                }
                Console.WriteLine("0 - Uscita");

                scelta = ScegliVoceMenu(0, _bancaCorrente.Banche_Funzionalita.Count);
            }

            return new Funzionalita();
        }
        public void Esegui(BancomatEntities2 ctx)
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
                            
                            
                        };
                        
                        richiesta = Richiesta.Login;

                        break;
                    case Richiesta.Login:
                        if (Login(ctx))
                            richiesta = Richiesta.MenuPrincipale;
                        else
                            richiesta = Richiesta.SchermataDiBenvenuto;
                        break;
                    case Richiesta.MenuPrincipale:
                        switch (MenuPrincipale())
                        {
                            //    case 
                            //        richiesta = Richiesta.SchermataDiBenvenuto;
                            //        break;
                            //    case Banca.Funzionalita.Versamento:
                            //        richiesta = Richiesta.Versamento;
                            //        break;
                            //    case Banca.Funzionalita.ReportSaldo:
                            //        richiesta = Richiesta.ReportSaldo;
                            //        break;
                            //    case Banca.Funzionalita.Prelievo:
                            //        richiesta = Richiesta.Prelievo;
                            //        break;
                        }
                        break;
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