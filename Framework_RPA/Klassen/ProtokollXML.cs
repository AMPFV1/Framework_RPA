using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

namespace Framework_RPA
{
    public class ProtokollXML
    {
        private string pathXMLFile;

        public string PathXMLFile { get => pathXMLFile; set => pathXMLFile = value; }

        public void CreateResultXML(Testfall testfall)
        {
            SingleResult ergebnisXML = new SingleResult();
            ergebnisXML.Start = DateTime.Now.ToString();
            ergebnisXML.Bezeichnung = testfall.Bezeichnung;
            ergebnisXML.Bereich = testfall.Bereich;
            ergebnisXML.Fortschritt = String.Empty;
            ergebnisXML.Ergebnisse = String.Empty;
            XML.SerializeSingleResult(ergebnisXML, PathXMLFile);
        }

        public void SetEnd()
        {
            //XML laden
            XmlDocument doc = new XmlDocument();
            doc.Load(PathXMLFile);
            XmlNode root = doc.DocumentElement;

            //Neuen Eintrag erstellen
            XmlElement elem = doc.CreateElement("Ende");
            elem.InnerText = DateTime.Now.ToString();

            //Eintrag hinzugügen
            root.AppendChild(elem);

            //Neuen Eintrag erstellen
            XmlElement dauer = doc.CreateElement("Dauer");
            TimeSpan timeSpan = DateTime.Parse(elem.InnerText) - DateTime.Parse(root.SelectSingleNode("Start").InnerText);
            dauer.InnerText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            //Eintrag hinzugügen
            root.AppendChild(dauer);

            doc.Save(PathXMLFile);
        }

        public void SetSingleResults()
        {
            //XML laden
            XmlDocument doc = new XmlDocument();
            doc.Load(PathXMLFile);
            XmlNode root = doc.DocumentElement;
            XmlNodeList ergebnisse = root.SelectNodes("Ergebnisse/Ergebnis");
            int vCounter = 0;
            foreach (XmlElement ergebnis in ergebnisse)
            {
                if (ergebnis.Attributes["Soll"].Value.Trim() == ergebnis.Attributes["Ist"].Value.Trim())
                {
                    ergebnis.SetAttribute("Result", "OK");
                }
                else
                {
                    ergebnis.SetAttribute("Result", "FAIL");
                    vCounter++;
                }
            }
            doc.Save(PathXMLFile);
            if (vCounter > 0)
            {
                SetTotalResult("FAIL");
            }
            else
            {
                SetTotalResult("OK");
            }
        }

        public void CreateOverviewResults(string DatetimeNow, string systeminfo, string anmerkung)
        {
            string ergebnisAblage = Path.GetDirectoryName(PathXMLFile);
            var directoryInfo = new DirectoryInfo(ergebnisAblage).GetFiles().OrderBy(f => f.LastWriteTime).ToList();
            TotalResult GesamtErgebnis = new TotalResult();
            GesamtErgebnis.Systeminfo = systeminfo;
            GesamtErgebnis.Anmerkung = anmerkung;
            
            foreach (var file in directoryInfo)
            {
                if (file.Extension.EndsWith("xml"))
                {
                    //XML laden
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file.FullName);
                    XmlNode root = doc.DocumentElement;

                    if (root.SelectSingleNode("GesamtErgebnis") == null)
                    {
                        GesamtErgebnis.GesamtError++;
                    }
                    else
                    {
                        switch (root.SelectSingleNode("GesamtErgebnis").InnerText)
                        {
                            case "OK":
                                GesamtErgebnis.GesamtOK++;
                                break;
                            case "FAIL":
                                GesamtErgebnis.GesamtFail++;
                                break;
                            default:
                                GesamtErgebnis.GesamtError++;
                                break;
                        }
                    }
                    if (GesamtErgebnis.Gesamt == 0)
                    {
                        GesamtErgebnis.Start = root.SelectSingleNode("Start").InnerText;
                    }
                    GesamtErgebnis.Ende = root.SelectSingleNode("Ende").InnerText;
                    if (root.SelectSingleNode("HistorischesErgebnis") != null)
                    {
                        GesamtErgebnis.EinzelErgebnisse.Add(new SingleResultsforTotalResult { Bezeichnung = root.SelectSingleNode("Bezeichnung").InnerText, Ergebnis = root.SelectSingleNode("GesamtErgebnis").InnerText, HistZeitpunkt = root.SelectSingleNode("HistorischesErgebnis").Attributes["Zeitpunkt"].Value, HistErgebnis = root.SelectSingleNode("HistorischesErgebnis").Attributes["Ergebnis"].Value });
                    }
                    else
                    {
                        GesamtErgebnis.EinzelErgebnisse.Add(new SingleResultsforTotalResult { Bezeichnung = root.SelectSingleNode("Bezeichnung").InnerText, Ergebnis = root.SelectSingleNode("GesamtErgebnis").InnerText });
                    }
                    GesamtErgebnis.Gesamt++;
                }
            }
            TimeSpan timeSpan = DateTime.Parse(GesamtErgebnis.Ende) - DateTime.Parse(GesamtErgebnis.Start);
            GesamtErgebnis.Dauer = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            XML.SerializeTotalResult(GesamtErgebnis, Path.Combine(ergebnisAblage, "Ergebnis_" + DatetimeNow + ".xml"));
        }

        public void SetTotalResult(string ergebnis)
        {
            //XML laden
            XmlDocument doc = new XmlDocument();
            doc.Load(PathXMLFile);
            XmlNode root = doc.DocumentElement;

            //Neuen Eintrag erstellen
            XmlElement elem = doc.CreateElement("GesamtErgebnis");
            elem.InnerText = ergebnis;

            //Eintrag hinzugügen
            if (root.SelectSingleNode("GesamtErgebnis") == null)
            {
                root.AppendChild(elem);
                doc.Save(PathXMLFile);
            }
        }

        public void SetHistoryResult(string auswahl)
        {
            DirectoryInfo folder = null;
            if(auswahl == "kein Ergebnis")
            {
                return;
            }
            else
            {
                folder = new DirectoryInfo(Path.Combine(RegistrySettings.getRegistry("Pfad_Ergebnisse"), auswahl));
            }
            string fullPathTestfall = Path.Combine(folder.FullName, Path.GetFileName(PathXMLFile));
            if (File.Exists(fullPathTestfall))
            {
                //XML laden
                XmlDocument doc = new XmlDocument();
                doc.Load(fullPathTestfall);
                XmlNode histroot = doc.DocumentElement;
                string histErgebnis = "kein Ergebnis vorhanden!";
                if (histroot.SelectSingleNode("GesamtErgebnis") != null)
                {
                    histErgebnis = histroot.SelectSingleNode("GesamtErgebnis").InnerText;
                }
                

                doc.Load(PathXMLFile);
                XmlNode root = doc.DocumentElement;

                //Neuen Eintrag erstellen
                XmlElement elem = doc.CreateElement("HistorischesErgebnis");
                elem.SetAttribute("Zeitpunkt", folder.Name);
                elem.SetAttribute("Ergebnis", histErgebnis);

                root.AppendChild(elem);
                doc.Save(PathXMLFile);
            }
            else
            {
                //XML laden
                XmlDocument doc = new XmlDocument();
                doc.Load(PathXMLFile);
                XmlNode root = doc.DocumentElement;

                //Neuen Eintrag erstellen
                XmlElement elem = doc.CreateElement("HistorischesErgebnis");
                elem.SetAttribute("Zeitpunkt", folder.Name);
                elem.SetAttribute("Ergebnis", "kein Ergebnis vorhanden!");

                root.AppendChild(elem);
                doc.Save(PathXMLFile);
            }
        }
    }

    public class SingleResult
    {
        [XmlElement("Bezeichnung")]
        public string Bezeichnung { get; set; }

        [XmlElement("Bereich")]
        public string Bereich { get; set; }

        [XmlElement("Fortschritt")]
        public string Fortschritt { get; set; }

        [XmlElement("Ergebnisse")]
        public string Ergebnisse { get; set; }

        [XmlElement("Start")]
        public string Start { get; set; }

        [XmlElement("Ende")]
        public string Ende { get; set; }

        [XmlElement("Dauer")]
        public string Dauer { get; set; }
    }

    public class TotalResult
    {
        [XmlElement("GesamtOK")]
        public int GesamtOK { get; set; }

        [XmlElement("GesamtFail")]
        public int GesamtFail { get; set; }

        [XmlElement("GesamtError")]
        public int GesamtError { get; set; }

        [XmlElement("Gesamt")]
        public int Gesamt { get; set; }

        [XmlElement("Start")]
        public string Start { get; set; }

        [XmlElement("Ende")]
        public string Ende { get; set; }

        [XmlElement("Dauer")]
        public string Dauer { get; set; }

        [XmlElement("Systeminfo")]
        public string Systeminfo { get; set; }

        [XmlElement("Anmerkung")]
        public string Anmerkung { get; set; }

        [XmlArray("Ergebnisse")]
        public List<SingleResultsforTotalResult> EinzelErgebnisse { get; set; }

        public TotalResult()
        {
            EinzelErgebnisse = new List<SingleResultsforTotalResult>();
        }
    }

    public class SingleResultsforTotalResult
    {
        [XmlElement("Bezeichnung")]
        public string Bezeichnung { get; set; }

        [XmlElement("Ergebnis")]
        public string Ergebnis { get; set; }

        [XmlElement("HistErgebnis")]
        public string HistErgebnis { get; set; }

        [XmlElement("HistZeitpunkt")]
        public string HistZeitpunkt { get; set; }
    }
}
