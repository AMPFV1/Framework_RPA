using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace Framework_RPA
{
    public class Testfall
    {
        public Testfall()
        {
            this.Items = new ObservableCollection<Testfall>();
        }
        public string Bezeichnung { get; set; }
        public string Bereich { get; set; }
        public string SkriptID { get; set; }
        public string SkriptPfad { get; set; }
        public string AblagePfad { get; set; }
        public string ImageSource { get; set; }
        public string FontWeight { get; set; }
        public bool IsAuswahl { get; set; }
        public bool IsSelected { get; set; }
        public string Checkbox { get; set; }
        public DirectoryInfo Directory { get; set; }
        public string rtfFile
        {
            get
            {
                return Path.Combine(AblagePfad, Bezeichnung + ".rtf");
            }
        }
        public string xmlFile
        {
            get
            {
                return Path.Combine(AblagePfad, Bezeichnung + ".xml");
            }
        }
        public string dataFile
        {
            get
            {
                return Path.Combine(AblagePfad, Bezeichnung + ".xlsx");
            }
        }
        public ObservableCollection<Testfall> Items { get; set; }
    }
}
