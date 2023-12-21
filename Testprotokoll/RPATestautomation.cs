using System;
using System.Xml;

namespace Testautomation
{
    public class RPATestautomation
    {
        public bool WriteProgress(string pathXMLFile, string text)
        {
            //XML laden
            XmlDocument doc = new XmlDocument();
            doc.Load(pathXMLFile);
            XmlNode root = doc.DocumentElement;
            XmlNode entries = root.SelectSingleNode("Fortschritt");

            //Neuen Eintrag erstellen
            XmlElement elem = doc.CreateElement("Step");
            elem.SetAttribute("TimeStamp", DateTime.Now.ToString());
            elem.InnerText = text;

            //Eintrag hinzugügen
            entries.AppendChild(elem);
            doc.Save(pathXMLFile);
            return true;
        }
        public bool WriteResult(string pathXMLFile, string text, string soll, string ist)
        {
            //XML laden
            XmlDocument doc = new XmlDocument();
            doc.Load(pathXMLFile);
            XmlNode root = doc.DocumentElement;
            XmlNode entries = root.SelectSingleNode("Ergebnisse");

            //Neuen Eintrag erstellen
            XmlElement elem = doc.CreateElement("Ergebnis");
            elem.SetAttribute("TimeStamp", DateTime.Now.ToString());
            elem.SetAttribute("Soll", soll);
            elem.SetAttribute("Ist", ist);
            elem.InnerText = text;

            //Eintrag hinzugügen
            entries.AppendChild(elem);
            doc.Save(pathXMLFile);
            return true;
        }

        public string GetValueFromTestcaseXML(string pathXMLFile, string value)
        {
            //XML laden
            XmlDocument doc = new XmlDocument();
            doc.Load(pathXMLFile);
            XmlNode root = doc.DocumentElement;
            return root.SelectSingleNode(value).InnerText;
        }
        public bool SetError(string pathXMLFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(pathXMLFile);
            XmlNode root = doc.DocumentElement;

            //Neuen Eintrag erstellen
            XmlElement elem = doc.CreateElement("GesamtErgebnis");
            elem.InnerText = "Error";

            //Eintrag hinzugügen
            root.AppendChild(elem);
            doc.Save(pathXMLFile);
            return true;
        }

        public void CloseRPA()
        {
            Environment.Exit(0);
        }
    }
}
