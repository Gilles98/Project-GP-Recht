using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Recht_DAL
{
    public static class DatabaseInitializer
    {
        public static void InsertDB()
        {
            using (RechtContext context = new RechtContext())
            {

                if (context.Juryleden.Any())
                {
                    return;
                }
                else
                {
                    context.Rechtbanken.Add(new Rechtbank() { Gemeente = "Geel", Straat = "Kleinhoefstraat", HuisNr = "4", Postcode = 2440, Naam = "Thomas More" });
                    context.Rechtbanken.Add(new Rechtbank() { Gemeente = "Berlaar", Straat = "Melkouwensteenweg", HuisNr = "36", Postcode = 2590, Naam = "Rechtbank  - Berlaar" });
                    context.SaveChanges();
                    context.Rechters.Add(new Rechter() { Voornaam = "Jos", Achternaam = "Peeters", RechtbankID = 1 });
                    context.Rechters.Add(new Rechter() { Voornaam = "Mark", Achternaam = "Van Hool", RechtbankID = 1 });
                    context.Rechters.Add(new Rechter() { Voornaam = "Gilles", Achternaam = "Gui", RechtbankID = 2 });
                    context.Rechters.Add(new Rechter() { Voornaam = "Tina", Achternaam = "Van Brabant", RechtbankID = 2 });

                    context.Juryleden.Add(new Jurylid() { Voornaam = "Francesca", Achternaam = "Liekens", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Mark", Achternaam = "Liekens", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Daniel", Achternaam = "Verbeek", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Cisca", Achternaam = "Dorkens", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Valerie", Achternaam = "Adams", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Marks", Achternaam = "Vertonghen", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Dimitrov", Achternaam = "Kacnopsky", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Joe", Achternaam = "Biden", Opgeroepen = false });

                    context.Juryleden.Add(new Jurylid() { Voornaam = "Jonathan", Achternaam = "Donaldson", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Cindy", Achternaam = "Verkasten", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Mario", Achternaam = "Van Damme", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Linda", Achternaam = "Schoeters", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Karel", Achternaam = "Jacobs", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Roger", Achternaam = "De Kelk", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Marco", Achternaam = "Paprizza", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Julia", Achternaam = "Stone", Opgeroepen = false });

                    context.Juryleden.Add(new Jurylid() { Voornaam = "Dönner", Achternaam = "Johansson", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Stan", Achternaam = "Markers", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Adam", Achternaam = "Van Damme", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Timmy", Achternaam = "Reginald", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Willy", Achternaam = "Willy", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Billy", Achternaam = "Van Kerk", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Danold", Achternaam = "Schuchter", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Do", Achternaam = "Reels", Opgeroepen = false });

                    context.Juryleden.Add(new Jurylid() { Voornaam = "Zane", Achternaam = "Marcus", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Danny", Achternaam = "Kakels", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Ronny", Achternaam = "Verleed", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Jacob", Achternaam = "Jacobsen", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Dan", Achternaam = "Streep", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Marielle", Achternaam = "Karels", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Sita", Achternaam = "Van Mechelen", Opgeroepen = false });
                    context.Juryleden.Add(new Jurylid() { Voornaam = "Aaron", Achternaam = "Simons", Opgeroepen = false });


                    context.Beklaagdes.Add(new Beklaagde() { Voornaam = "Darco", Achternaam = "Darius", Gemeente = "Berlaar", Straat = "Schuttershof", HuisNr = "3", Postcode = 2590 });
                    context.Beklaagdes.Add(new Beklaagde() { Voornaam = "Maria", Achternaam = "Van Denver", Gemeente = "Geel", Straat = "Acaciastraat", HuisNr = "5", Postcode = 2440 });

                    context.Aanklager.Add(new Aanklager() { Voornaam = "Danny", Achternaam = "Ronalds", Gemeente = "Geel", Straat = "Acaciastraat", HuisNr = "5", Postcode = 2440 });
                    context.Aanklager.Add(new Aanklager() { Voornaam = "Mars", Achternaam = "Vankaster", Gemeente = "Berlaar", Straat = "Marialaan", HuisNr = "12", Postcode = 2500 });
                    context.SaveChanges();
                }


            }
        }
    }
}
