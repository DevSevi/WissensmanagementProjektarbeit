using System.Diagnostics;
using System.Text.Json;
using System.Text;

namespace WissemsManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Projekt> projekte = new List<Projekt>();

            Projektleiter pl = new Projektleiter("Séverin", "Kiener");
            Console.WriteLine(pl.GetNameVorname());
            Console.ReadKey();

            Projekt projekt = new Projekt("Testprojekt", "KundeXY", "ein Muss");
            Projekt projekt2 = new Projekt("Hiss für alle", "Hiss AG", "Hiss für alle");
            projekte.Add(projekt);
            projekte.Add(projekt2);
            Serialisieren(projekte);
        }

        static void Serialisieren(List<Projekt> projekte)
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
        public string Name { get; set; }
        public string Vorname { get; set; }

        public Person(string vorname, string name)
        {
            Name = name;
            Vorname = vorname;
        }

        public string GetNameVorname()
        {
            return $"{Vorname} {Name}";
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
        public string Name { get; set; }
        public string Kunde { get; set; }
        public string Anforderungen { get; set; }

        public Projekt(string name, string kunde, string anforderungen)
        {
            Name = name;
            Kunde = kunde;
            Anforderungen = anforderungen;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetKunde()
        {
            return Kunde;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetKunde(string kunde)
        {
            Kunde = kunde;
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
