using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Framework_RPA
{
    public class XML
    {
        static public void SerializeTestcase(TestcaseXML testfall)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TestcaseXML));
            using (TextWriter writer = new StreamWriter(testfall.xmlFile))
            {
                serializer.Serialize(writer, testfall);
            }
        }

        static public void CreateStartXML(string TestfallXMLPfad)
        {
            XmlWriter xmlWriter = XmlWriter.Create(Path.Combine(RegistrySettings.getRegistry("Pfad_Testfälle"), "test.xml"));

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Testfall");

            xmlWriter.WriteStartElement("Pfad");
            xmlWriter.WriteString(TestfallXMLPfad);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        static public void SerializeSingleResult(SingleResult ergebnisXML, string ablagepfad)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SingleResult));
            using (TextWriter writer = new StreamWriter(ablagepfad))
            {
                serializer.Serialize(writer, ergebnisXML);
            }
        }

        static public void SerializeTotalResult(TotalResult gesamtErgebnisXML, string ablagepfad)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TotalResult));
            using (TextWriter writer = new StreamWriter(ablagepfad))
            {
                serializer.Serialize(writer, gesamtErgebnisXML);
            }
        }

        static public TestcaseXML DeserializeTestcase(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TestcaseXML));
            using (Stream reader = new FileStream(filename, FileMode.Open))
            {
                return (TestcaseXML)serializer.Deserialize(reader);
            }
        }
    }
    public class TestcaseXML
    {
        [XmlElement("Bezeichnung")]
        public string Bezeichnung { get; set; }

        [XmlElement("Bereich")]
        public string Bereich { get; set; }

        [XmlElement("Argument1")]
        public string Argument1 { get; set; }

        [XmlElement("Argument2")]
        public string Argument2 { get; set; }

        [XmlElement("Argument3")]
        public string Argument3 { get; set; }

        [XmlElement("AblagePfad")]
        public string AblagePfad { get; set; }

        [XmlElement("DatenPfad")]
        public string DatenPfad { get; set; }

        [XmlElement("ErgebnisXML")]
        public string ErgebnisXML { get; set; }

        public string xmlFile
        {
            get
            {
                return Path.Combine(AblagePfad, Bezeichnung + ".xml");
            }
        }
    }
}
