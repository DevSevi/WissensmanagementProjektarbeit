namespace WissemsManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Projektleiter pl = new Projektleiter("Séverin", "Kiener");
            Console.WriteLine(pl.GetNameVorname());
            Console.ReadKey();
        }
    }

    [Serializable]
    abstract class Person
    {
        string Name { get; set; }
        string Vorname { get; set; }

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
        string Name { get; set; }
        string Kunde { get; set; }
        string Anforderungen { get; set; }

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
        int ID { get; set; }
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
        string Titel { get; set; }
        string Inhalt { get; set; }
    }
}
