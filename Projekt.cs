using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WissensManagement;

namespace Wissensmanagement
{
    [Serializable]
    public class Projekt
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
            sb.AppendLine($"{name}, Kunde {kunde}, Projektleiter {projektleiter.GetNameVorname()}, " +
                $"Projektmitarbeiter {projektmitarbeiter.GetNameVorname()} - ID {ID}");

            if(tags != null && tags.Count > 0)
            {
                sb.AppendLine("    Tags: ");
                foreach (Tag tag in tags)
                {
                    sb.AppendLine($"    {tag.TagID} - {tag.TagName}");
                }
            }
            if (informationen != null && informationen.Count > 0)
            {
                sb.AppendLine("     informationen:");
                foreach (Information info in informationen)
                {
                    sb.AppendLine("     " + info.ID.ToString());
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
                    if (info.Tags != null && info.Tags.Count > 0)
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
}
