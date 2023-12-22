using Microsoft.Win32;
using System.Collections.Generic;

namespace Framework_RPA
{
    public class RegistrySettings
    {
        /// <summary>
        /// Der Unterschlüssel des MentorAKS40 für die Settings
        /// Ist der MentorAKS40schlüssel und "Software\\Framework_RPA"
        /// </summary>
        private const string masterKeyApplication = "Software\\Framework_RPA";

        private static Dictionary<string, string> defaultValues = new Dictionary<string, string>();
        public static void setDefaultValues()
        {
            defaultValues.Clear();
            defaultValues.Add("Pfad_Ergebnisse", @"D:\Daten\Framework_RPA\Ergebnisse");
            defaultValues.Add("Pfad_Testfälle", @"D:\Daten\Framework_RPA\Testfälle");
            defaultValues.Add("RPA_Software", "");
            defaultValues.Add("Max_Dauer", "900000");
            SetAllRegitryKeys();
        }
        private static void SetAllRegitryKeys()
        {
            foreach(string key in defaultValues.Keys)
            {
                getRegistry(key);
            }
        }
        public static string getRegistry(string Key)
        {
            return GetRegistryValue(Key, defaultValues[Key], masterKeyApplication).ToString();
        }
        private static object GetRegistryValue(string SubKey, object DefaultValue, string MasterKey)
        {
            try
            {
                RegistryKey myKey = Registry.CurrentUser.OpenSubKey(MasterKey, true);
                if (myKey == null)
                {
                    myKey = Registry.CurrentUser.CreateSubKey(MasterKey);
                    myKey = Registry.CurrentUser.OpenSubKey(MasterKey, true);
                }

                if (myKey.GetValue(SubKey, null) == null)
                {
                    if (DefaultValue != null)
                    {
                        myKey.SetValue(SubKey, DefaultValue, RegistryValueKind.String);
                    }
                }
                object erg = myKey.GetValue(SubKey);
                myKey.Close();
                return erg;
            }
            catch
            {
                return "";
            }
        }
        public static void SetRegistryValue(string SubKey, object Value)
        {
            RegistryKey myKey = Registry.CurrentUser.OpenSubKey(masterKeyApplication, true);
            myKey.SetValue(SubKey, Value, RegistryValueKind.String);
        }
    }
}
