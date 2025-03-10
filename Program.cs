using System.Diagnostics;
using System.Text.Json;
using System.Text;

namespace WissensManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Projekt> projekte;
            try
            {
                projekte = LoadProjects();
            }
            catch (Exception)
            {
                projekte = new List<Projekt>();
            }            
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
            Console.WriteLine("5: Information im Projekt nach Tag suchen");
            Console.WriteLine("6: Speichern und beenden");
            Console.WriteLine("7: Beenden ohne zu speichern");

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
                        ProjektBearbeitenMenu(projekte);
                        HauptMenu(projekte);
                        break;
                    case 3:
                        ProjektLoeschenMenu(projekte);
                        HauptMenu(projekte);
                        break;
                    case 4:
                        Console.WriteLine("\n" + ProjekteAnzeigenMenu(projekte));
                        HauptMenu(projekte);
                        break;
                    case 5:
                        ProjektTagSuchenMenu(projekte);
                        HauptMenu(projekte);
                        break;
                    case 6:
                        SaveProjects(projekte);
                        break;
                    case 7:
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
            Console.WriteLine("\nProjektname angeben");
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
            Console.WriteLine("Tags hinzufügen?");
            Console.WriteLine("1: Ja");
            Console.WriteLine("2: Nein");
            var keyInfo = Console.ReadKey();
            int auswahl;
            if (int.TryParse(keyInfo.KeyChar.ToString(), out auswahl))
            {
                switch (auswahl)
                {
                    case 1:
                        Console.WriteLine("Tag-ID angeben");
                        string tagID = Console.ReadLine();
                        Console.WriteLine("Tagname angeben");
                        string tagName = Console.ReadLine();
                        projekt.AddTag(new Tag(tagID, tagName));
                        break;
                    case 2:
                        break;
                    default:
                        Console.WriteLine("Ungültige Eingabe");
                        break;
                }
            }

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

        static void ProjektBearbeitenMenu(List<Projekt> projekte)
        {
            Console.WriteLine("\nWelches Projekt bearbeiten?");
            StringBuilder sb = new StringBuilder();
            int counter = 1;
            foreach (Projekt projekt in projekte)
            {
                sb.AppendLine(counter.ToString() + ": " + projekt.GetProjektInfos());
                counter++;
            }
            Console.WriteLine(sb.ToString());
            var keyInfoProjektNr = Console.ReadKey();
            int projektNr;
            if(int.TryParse(keyInfoProjektNr.KeyChar.ToString(), out projektNr))
            {
                Console.WriteLine($"\nProjekt {projektNr.ToString()} ausgewählt");
            }
            else
            {
                Console.WriteLine("Ungültige Eingabe");
            }
            Console.WriteLine("Auswahl Bearbeitungsmöglichkeiten:");
            Console.WriteLine("1: Tags hinzufügen (werden für Informationen benutzt)");
            Console.WriteLine("2: Information hinzufügen");
            Console.WriteLine("3: Information bearbeiten");
            var keyInfo = Console.ReadKey();
            int auswahl;
            if (int.TryParse(keyInfo.KeyChar.ToString(), out auswahl))
            {
                switch (auswahl)
                {
                    case 1:
                        TagHinzufuegenMenu(projekte[projektNr - 1]);
                        break;
                    case 2:
                        InformationHinzufuegenMenu(projekte[projektNr - 1]);
                        break;
                    case 3:
                        InformationBearbeitenMenu(projekte[projektNr - 1]);
                        break;
                    default:
                        Console.WriteLine("Ungültige Eingabe");
                        break;
                }
            }
        }

        static void TagHinzufuegenMenu(Projekt projekt)
        {
            Console.WriteLine("Tag-ID angeben");
            string tagID = Console.ReadLine();
            Console.WriteLine("Tagname angeben");
            string tagName = Console.ReadLine();
            projekt.AddTag(new Tag(tagID, tagName));
        }

        static void InformationHinzufuegenMenu(Projekt projekt)
        {
            if(projekt.tags.Count == 0)
            {
                Console.WriteLine("Keine Tags vorhanden, bitte zuerst Tags hinzufügen");
                return;
            }
            Console.WriteLine("Welche Art von Information hinzufügen?");
            Console.WriteLine("1: Text");
            Console.WriteLine("2: Bild");
            Console.WriteLine("3: Dokument");
            var keyInfo = Console.ReadKey();
            int auswahl;
            if (int.TryParse(keyInfo.KeyChar.ToString(), out auswahl))
            {
                switch (auswahl)
                {
                    case 1:
                        Text text = TextHinzufuegenMenu(projekt);
                        projekt.AddInformation(text);
                        break;
                    case 2:
                        Bild bild = BildHinzufuegenMenu(projekt);
                        projekt.AddInformation(bild);
                        break;
                    case 3:
                        Dokument dokument = DokumentHinzufuegenMenu(projekt);
                        projekt.AddInformation(dokument);
                        break;
                    default:
                        Console.WriteLine("Ungültige Eingabe");
                        break;
                }
            }
        }

        static void InformationBearbeitenMenu(Projekt projekt)
        {
            Console.WriteLine("Welche Information bearbeiten?");
            int counter = 1;
            if(projekt.informationen.Count == 0)
            {
                Console.WriteLine("Keine Informationen vorhanden");
                return;
            }
            foreach (Information info in projekt.informationen)
            {
                Console.WriteLine($"{counter.ToString()} {info.Titel}");
                counter++;
            }
            var keyInfo = Console.ReadKey();
            int auswahl;
            if (int.TryParse(keyInfo.KeyChar.ToString(), out auswahl))
            {
                if (projekt.informationen[auswahl - 1] is not Text)
                {
                    Console.WriteLine("Nur Texte können bearbeitet werden");
                }
                Text text = projekt.informationen[auswahl - 1] as Text;
                Console.WriteLine($"\nInformation bearbeiten: {text.Titel}");
                Console.WriteLine("Neuer Inhalt angeben");
                string inhalt = Console.ReadLine();
                text.ChangeInhalt(inhalt);
            }
            else
            {
                Console.WriteLine("Ungültige Eingabe");
            }
        }

        static Text TextHinzufuegenMenu(Projekt projekt)
        {
            Console.WriteLine("Titel angeben");
            string titel = Console.ReadLine();
            Console.WriteLine("Inhalt angeben");
            string inhalt = Console.ReadLine();
            List<Tag> tags = new List<Tag>();
            Console.WriteLine("Gewünschte Tag-ID angeben");
            foreach(Tag tag in projekt.tags)
            {
                Console.WriteLine(tag.GetTagInfos());
            }
            string tagID = Console.ReadLine();
            foreach(Tag tag in projekt.tags)
            {
                if(tag.TagID == tagID)
                {
                    tags.Add(tag);
                    Console.WriteLine($"Tag {tag.TagID} wurde hinzugefügt");
                }
            }

            Text text = new Text(titel, inhalt, tags);
            return text;
        }

        static Bild BildHinzufuegenMenu(Projekt projekt)
        {
            Console.WriteLine("Titel angeben");
            string titel = Console.ReadLine();
            Console.WriteLine("URL angeben");
            string url = Console.ReadLine();
            List<Tag> tags = new List<Tag>();
            Console.WriteLine("Gewünschte Tag-ID angeben");
            foreach (Tag tag in projekt.tags)
            {
                Console.WriteLine(tag.GetTagInfos());
            }
            string tagID = Console.ReadLine();
            foreach (Tag tag in projekt.tags)
            {
                if (tag.TagID == tagID)
                {
                    tags.Add(tag);
                    Console.WriteLine($"Tag {tag.TagID} wurde hinzugefügt");
                }
            }

            Bild bild = new Bild(titel, url, tags);
            return bild;
        }

        static Dokument DokumentHinzufuegenMenu(Projekt projekt)
        {
            Console.WriteLine("Titel angeben");
            string titel = Console.ReadLine();
            Console.WriteLine("URL angeben");
            string url = Console.ReadLine();
            List<Tag> tags = new List<Tag>();
            Console.WriteLine("Gewünschte Tag-ID angeben");
            foreach (Tag tag in projekt.tags)
            {
                Console.WriteLine(tag.GetTagInfos());
            }
            string tagID = Console.ReadLine();
            foreach (Tag tag in projekt.tags)
            {
                if (tag.TagID == tagID)
                {
                    tags.Add(tag);
                    Console.WriteLine($"Tag {tag.TagID} wurde hinzugefügt");
                }
            }
            Dokument dokument = new Dokument(titel, url, tags);
            return dokument;
        }
        static void ProjektLoeschenMenu(List<Projekt> projekte)
        {
            Console.WriteLine("Welches Projekt löschen?");
            var keyInfo = Console.ReadKey();
            int auswahl;
            if (int.TryParse(keyInfo.KeyChar.ToString(), out auswahl))
            {
                Console.WriteLine($"\nProjekt wird gelöscht: {projekte[auswahl - 1].GetProjektInfos()}");
                projekte.RemoveAt(auswahl - 1);
                Console.WriteLine("Projekt gelöscht!");
            }
            else
            {
                Console.WriteLine("Ungültige Eingabe");
            }
        }

        static void ProjektTagSuchenMenu(List<Projekt> projekte)
        {
            int counter = 1;
            foreach (Projekt projekt in projekte)
            {
                Console.WriteLine($"{counter.ToString()} {projekt.GetProjektInfos()}");
                counter++;
            }
            Console.WriteLine("Welches Projekt durchsuchen?");
            var keyInfo = Console.ReadKey();
            int auswahl;
            if (int.TryParse(keyInfo.KeyChar.ToString(), out auswahl))
            {
                Console.WriteLine($"\nProjekt durchsuchen: {projekte[auswahl - 1].GetProjektInfos()}");
                Console.WriteLine("Nach welchem Tag suchen");
                string tag = Console.ReadLine();
                List<Information> infos = projekte[auswahl - 1].SearchInformationMitTag(tag);
                if (infos.Count > 0)
                {
                    Console.WriteLine("Informationen gefunden:");
                    foreach (Information info in infos)
                    {
                        Console.WriteLine(info.Titel);
                    }
                }
                else
                {
                    Console.WriteLine("Keine Informationen gefunden");
                }
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

        public List<Information> informationen { get; set; }
        
        public List<Tag> tags { get; set; }
        public Projekt(string name, string kunde, string anforderungen, Projektleiter projektleiter, Projektmitarbeiter projektmitarbeiter, List<Tag> tags)
        {
            this.name = name;
            this.kunde = kunde;
            this.anforderungen = anforderungen;
            this.projektleiter = projektleiter;
            this.projektmitarbeiter = projektmitarbeiter;
            ID = Guid.NewGuid();
            informationen = new List<Information>();
            this.tags = tags;
        }
        public Projekt(string name, string kunde, string anforderungen, Projektleiter projektleiter, Projektmitarbeiter projektmitarbeiter)
        {
            this.name = name;
            this.kunde = kunde;
            this.anforderungen = anforderungen;
            this.projektleiter = projektleiter;
            this.projektmitarbeiter = projektmitarbeiter;
            ID = Guid.NewGuid();
            informationen = new List<Information>();
            tags = new List<Tag>();
        }

        // **Parameterloser Konstruktor für die Deserialisierung**
        public Projekt() { }

        public void AddInformation(Information information)
        {
            informationen.Add(information);
        }

        public void AddTag(Tag tag)
        {
            tags.Add(tag);
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
            if (informationen != null && informationen.Count > 0)
            {
                sb.AppendLine("informationen:");
                foreach (Information info in informationen)
                {
                    sb.Append(info.ID.ToString());
                }
            }

            return sb.ToString();
        }

        public List<Information> SearchInformationMitTag(string Tag)
        {
            List<Information> infosAusgabe = new List<Information>();
            if (informationen != null && informationen.Count > 0)
            {
                foreach (Information info in informationen)
                {
                    if(info.Tags != null && info.Tags.Count > 0)
                    {
                        foreach (Tag tag in info.Tags)
                        {
                            if (tag.TagName == Tag)
                            {
                                infosAusgabe.Add(info);
                            }
                        }
                    }

                }
            }
            return infosAusgabe;
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
        public string Titel { get; set; }
        public List<Tag> Tags { get; set; }
        public Information(string Titel, List<Tag> Tags)
        {
            Random rand = new Random();
            ID = rand.Next();
            this.Titel = Titel;
            this.Tags = Tags;
        }
        public int ID { get; set; }

        public void AddTag(Tag tag)
        {
            if(Tags != null && Tags.Count > 3)
            {
                throw new Exception("Maximal 3 Tags erlaubt");
            }
            Tags.Add(tag);
        }

        public void ClearTags()
        {
            Tags.Clear();
        }
    }

    [Serializable]
    class Text : Information
    {
        public Text(string Titel, string Inhalt, List<Tag> Tags) : base(Titel, Tags)
        {
            this.Inhalt = Inhalt;
        }
        public string Inhalt { get; set; }

        public void ChangeInhalt(string Inhalt)
        {
            string oldInhalt = this.Inhalt;
            this.Inhalt = Inhalt;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Alter Inhalt: {oldInhalt}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Neuer Inhalt: {this.Inhalt}");
            Console.ResetColor();
        }
    }

    [Serializable]
    class Bild : Information
    {
        public Bild(string Titel, string URL, List<Tag> Tags) : base(Titel, Tags)
        {
            this.URL = URL;
        }
        public string URL { get; set; }
    }

    [Serializable]
    class Dokument : Information
    {
        public Dokument(string Titel, string URL, List<Tag> Tags) : base(Titel, Tags)
        {
            this.URL = URL;
        }
        public string URL { get; set; }
    }

    [Serializable]
    class Tag
    {
        public string TagID { get; set; }
        public string TagName { get; set; }

        public Tag(string TagID, string TagName)
        {
            this.TagID = TagID;
            this.TagName = TagName;
        }

        public string GetTagInfos()
        {
            return $"Tag {TagID} - {TagName}";

        }
    }
}
