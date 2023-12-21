using System;
using System.Diagnostics;
using System.IO;

namespace Framework_RPA
{
    public class Testdurchlauf
    {
        static public void RunTest(ProtokollXML protokollXML, Testfall testfall, string dateTimeNow, string histAuswahl)
        {
            //Erstellt die ErgebnisXML damit der Softwareroboter diese befüllen kann
            protokollXML.PathXMLFile = Path.Combine(RegistrySettings.getRegistry("Pfad_Ergebnisse"), dateTimeNow, testfall.Bezeichnung + ".xml");
            protokollXML.CreateResultXML(testfall);
            
            //Startet Softwareroboter
            Process RPA_Software = new Process();
            RPA_Software.StartInfo.FileName = RegistrySettings.getRegistry("RPA_Software");
            
            if (testfall.SkriptID != "")
            {
                RPA_Software.StartInfo.Arguments =  RegistrySettings.getRegistry("Argument_SkriptID") + " " + testfall.SkriptID;
            }
            else
            {
                RPA_Software.StartInfo.Arguments = "\"" + RegistrySettings.getRegistry("Argument_SkriptPfad") + " " + testfall.SkriptPfad + "\"";
            }
            RPA_Software.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            RPA_Software.Start();
            RPA_Software.WaitForExit(Convert.ToInt32(RegistrySettings.getRegistry("Max_Dauer")));

            //Prüft ob der Softerroboter den Prozess beendet hat
            if (!RPA_Software.HasExited)
            {
                RPA_Software.Kill();
                protokollXML.SetTotalResult("Error");
            }
            RPA_Software.Close();

            //Fügt die letzten Einträge in die ErgebnisXML ein
            protokollXML.SetEnd();
            protokollXML.SetSingleResults();

            protokollXML.SetHistoryResult(histAuswahl);
        }
    }
}
