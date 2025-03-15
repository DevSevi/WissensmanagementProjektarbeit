using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wissensmanagement
{
    [Serializable]
    public abstract class Information
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
            if (Tags != null && Tags.Count > 3)
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
    public class Text : Information
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
    public class Bild : Information
    {
        public Bild(string Titel, string URL, List<Tag> Tags) : base(Titel, Tags)
        {
            this.URL = URL;
        }
        public string URL { get; set; }
    }

    [Serializable]
    public class Dokument : Information
    {
        public Dokument(string Titel, string URL, List<Tag> Tags) : base(Titel, Tags)
        {
            this.URL = URL;
        }
        public string URL { get; set; }
    }

    [Serializable]
    public class Tag
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
