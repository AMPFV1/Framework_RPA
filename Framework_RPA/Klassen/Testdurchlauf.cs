using System;
using System.Diagnostics;
using System.IO;
using System.Text;

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
            ProcessStartInfo RPAStartInfo = new ProcessStartInfo();
            StringBuilder argumenteList = new StringBuilder();
            RPAStartInfo.FileName = RegistrySettings.getRegistry("RPA_Software");
            
            if (testfall.Argument1 != "")
            {
                argumenteList.Append(testfall.Argument1 + " ");
            }
            if (testfall.Argument2 != "")
            {
                argumenteList.Append(testfall.Argument2 + " ");
            }
            if (testfall.Argument3 != "")
            {
                argumenteList.Append(testfall.Argument3 + " ");
            }
            RPAStartInfo.Arguments = argumenteList.ToString();
            RPAStartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            RPA_Software.StartInfo = RPAStartInfo;
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
