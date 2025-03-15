using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wissensmanagement
{
    [Serializable]
    public abstract class Person
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
    public class Projektleiter : Person
    {
        public Projektleiter(string vorname, string name) : base(vorname, name)
        {
        }
    }

    [Serializable]
    public class Projektmitarbeiter : Person
    {
        public Projektmitarbeiter(string vorname, string name) : base(vorname, name)
        {
        }
    }
}
