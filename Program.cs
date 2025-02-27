using System.Diagnostics;
using System.Text.Json;
using System.Text;

namespace WissensManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Projekt> projekte = new List<Projekt>();

            Projektleiter pl = new Projektleiter("Séverin", "Kiener");
            Projektmitarbeiter projektmitarbeiter = new Projektmitarbeiter("Luca", "Zaugg");
            Projekt projekt = new Projekt("Testprojekt", "KundeXY", "ein Muss", pl, projektmitarbeiter);
            Bild bild = new Bild("Bild1", "URL1");

            projekt.AddInformation(bild);
            Projekt projekt2 = new Projekt("Hiss für alle", "Hiss AG", "Hiss für alle", pl, projektmitarbeiter);
            projekte.Add(projekt);
            projekte.Add(projekt2);

            Console.WriteLine("Test");

            SaveProjects(projekte);

            foreach(Projekt proj in projekte)
            {
                Console.WriteLine(proj.GetProjektInfos());
            }

            Console.ReadKey();
        }

        static void SaveProjects(List<Projekt> projekte)
        {
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };
            string jsonstring = JsonSerializer.Serialize(projekte, jsonSerializerOptions);
            File.WriteAllText(@"C:\Daten\projekte.json", jsonstring);
        }
    }

    [Serializable]
    abstract class Person
    {
        public string name { get; set; }
        public string vorname { get; set; }

        public Person(string vorname, string name)
        {
            this.name = name;
            this.vorname = vorname;
        }

        public string GetNameVorname()
        {
            return $"{vorname} {name}";
        }
    }

    [Serializable]
    class Projektleiter : Person
    {
        public Projektleiter(string vorname, string name) : base(vorname, name)
        {
        }
    }

    [Serializable]
    class Projektmitarbeiter : Person
    {
        public Projektmitarbeiter(string vorname, string name) : base(vorname, name)
        {
        }
    }

    [Serializable]
    class Projekt
    {
        public string name { get; set; }
        public string kunde { get; set; }
        public string anforderungen { get; set; }

        public Projektleiter projektleiter { get; set; }

        public Projektmitarbeiter projektmitarbeiter { get; set; }

        public List<Information> informationen { get; set; }

        public Projekt(string name, string kunde, string anforderungen, Projektleiter projektleiter, Projektmitarbeiter projektmitarbeiter)
        {
            this.name = name;
            this.kunde = kunde;
            this.anforderungen = anforderungen;
            this.projektleiter = projektleiter;
            this.projektmitarbeiter = projektmitarbeiter;
        }

        public void AddInformation(Information information)
        {
            informationen.Add(information);
        }

        public string GetName()
        {
            return name;
        }

        public string GetKunde()
        {
            return kunde;
        }

        public string GetProjektInfos()
        {
            string ausgabe = $"Projekt {name}, Kunde {kunde}, Projektleiter {projektleiter.GetNameVorname()}, " +
                $"Projektmitarbeiter {projektmitarbeiter.GetNameVorname()}";

            return ausgabe;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetKunde(string kunde)
        {
            this.kunde = kunde;
        }
    }

    [Serializable]
    abstract class Information
    {
        public Information()
        {
        }
        public int ID { get; set; }
        public void Kommentieren()
        {

        }

        public void Ergaenzen()
        {

        }
    }


    class Text : Information
    {
        public Text(string Titel, string Inhalt)
        {
            this.Titel = Titel;
            this.Inhalt = Inhalt;
        }
        public string Titel { get; set; }
        public string Inhalt { get; set; }
    }

    class Bild : Information
    {
        public Bild(string Titel, string URL)
        {
            this.Titel = Titel;
            this.URL = URL;
        }
        public string Titel { get; set; }
        public string URL { get; set; }
    }

    class Dokument : Information
    {
        public Dokument(string Titel, string URL)
        {
            this.Titel = Titel;
            this.URL = URL;
        }
        public string Titel { get; set; }
        public string URL { get; set; }
    }
}
