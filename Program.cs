using System.Diagnostics;
using System.Text.Json;
using System.Text;

namespace WissensManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Projekt> projekte = LoadProjects();
            HauptMenu(projekte);
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

        static List<Projekt> LoadProjects()
        {
            string jsonstring = File.ReadAllText(@"C:\Daten\projekte.json");
            List<Projekt> projekte = JsonSerializer.Deserialize<List<Projekt>>(jsonstring);
            if (projekte == null)
            {
                return new List<Projekt>();
            }
            return projekte;
        }

        static void HauptMenu(List<Projekt> projekte)
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Willkommen zum Wissensmanagement");
            Console.WriteLine("Bitte wählen Sie eine Option:");
            Console.WriteLine("1: Projekt erstellen");
            Console.WriteLine("2: Projekt bearbeiten");
            Console.WriteLine("3: Projekt löschen");
            Console.WriteLine("4: Projekte anzeigen");
            Console.WriteLine("5: Speichern und beenden");
            Console.WriteLine("6: Beenden ohne zu speichern");

            var keyInfo = Console.ReadKey();
            int auswahl;
            if (int.TryParse(keyInfo.KeyChar.ToString(), out auswahl))
            {
                switch (auswahl)
                {
                    case 1:
                        Projekt projektNeu = ProjektErstellenMenu();
                        projekte.Add(projektNeu);
                        HauptMenu(projekte);
                        break;
                    case 2:
                        // noch erweitern
                        ProjektBearbeitenMenu(projekte[0]);
                        HauptMenu(projekte);
                        break;
                    case 3:
                        ProjektLoeschenMenu(projekte);
                        HauptMenu(projekte);
                        break;
                    case 4:
                        Console.WriteLine(ProjekteAnzeigenMenu(projekte));
                        HauptMenu(projekte);
                        break;
                    case 5:
                        SaveProjects(projekte);
                        break;
                    case 6:
                        break;
                    default:
                        Console.WriteLine("Ungültige Eingabe");
                        HauptMenu(projekte);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Ungültige Eingabe");
                HauptMenu(projekte);
            }
        }

        static Projekt ProjektErstellenMenu()
        {
            Console.WriteLine("Projektname angeben");
            string name = Console.ReadLine();
            Console.WriteLine("Kunde angeben");
            string kunde = Console.ReadLine();
            Console.WriteLine("Anforderungen angeben");
            string anforderungen = Console.ReadLine();

            Console.WriteLine("Projektleiter angeben");
            Console.WriteLine("Vorname angeben");
            string PLVorname = Console.ReadLine();
            Console.WriteLine("Name angeben");
            string PLName = Console.ReadLine();
            Projektleiter pl = new Projektleiter(PLVorname, PLName);

            Console.WriteLine("Projektmitarbeiter angeben");
            Console.WriteLine("Vorname angeben");
            string PMVorname = Console.ReadLine();
            Console.WriteLine("Name angeben");
            string PMName = Console.ReadLine();
            Projektmitarbeiter pm = new Projektmitarbeiter(PMVorname, PMName);

            Projekt projekt = new Projekt(name, kunde, anforderungen, pl, pm);

            Console.WriteLine("Projekt erfolgreich angelegt");

            return projekt;
        }

        static string ProjekteAnzeigenMenu(List<Projekt> projekte)
        {
            if (projekte.Count == 0)
            {
                return "Keine Projekte vorhanden";
            }
            StringBuilder sb = new StringBuilder();
            foreach (Projekt projekt in projekte)
            {
                sb.AppendLine(projekt.GetProjektInfos());
            }
            return sb.ToString();
        }

        // noch erweitern
        static void ProjektBearbeitenMenu(Projekt projekt)
        {
            projekt.AddInformation(new Text("Test", "Test"));
            //return projekt;
        }

        static void ProjektLoeschenMenu(List<Projekt> projekte)
        {
            Console.WriteLine("Welches Projekt löschen?");
            var keyInfo = Console.ReadKey();
            int auswahl;
            if (int.TryParse(keyInfo.KeyChar.ToString(), out auswahl))
            {
                projekte.RemoveAt(auswahl - 1);
                Console.WriteLine("Projekt gelöscht!");
            }
            else
            {
                Console.WriteLine("Ungültige Eingabe");
            }
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
        public Guid ID { get; set; }

        public Projektleiter projektleiter { get; set; }

        public Projektmitarbeiter projektmitarbeiter { get; set; }

        private List<Information> informationen { get; set; }

        public Projekt(string name, string kunde, string anforderungen, Projektleiter projektleiter, Projektmitarbeiter projektmitarbeiter)
        {
            this.name = name;
            this.kunde = kunde;
            this.anforderungen = anforderungen;
            this.projektleiter = projektleiter;
            this.projektmitarbeiter = projektmitarbeiter;
            ID = Guid.NewGuid();
            informationen = new List<Information>();
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
            StringBuilder sb = new StringBuilder();
            sb.Append($"Projekt {name} - ID {ID}, Kunde {kunde}, Projektleiter {projektleiter.GetNameVorname()}, " +
                $"Projektmitarbeiter {projektmitarbeiter.GetNameVorname()}");
            if (informationen.Count > 0)
            {
                sb.AppendLine("informationen:");
                foreach (Information info in informationen)
                {
                    sb.Append(info.ID.ToString());
                }
            }

            return sb.ToString();
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
        public List<Tag> tags { get; set; }
        public Information()
        {
        }
        public int ID { get; set; }

        public void AddTag(Tag tag)
        {
            if(tags != null && tags.Count > 3)
            {
                throw new Exception("Maximal 3 Tags erlaubt");
            }
            tags.Add(tag);
        }

        public void ClearTags()
        {
            tags.Clear();
        }
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

    class Tag
    {
        public string TagID { get; set; }
        public string TagName { get; set; }

        public Tag(string TagID, string TagName)
        {
            this.TagID = TagID;
            this.TagName = TagName;
        }

    }
}
