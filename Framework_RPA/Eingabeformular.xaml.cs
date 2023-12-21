using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace Framework_RPA
{
    /// <summary>
    /// Interaktionslogik für Eingabeformular.xaml
    /// </summary>
    public partial class Eingabeformular : Window
    {
        public Eingabeformular()
        {
            InitializeComponent();
            Bereicheladen();
        }

        private void btnSpeichern_Click(object sender, RoutedEventArgs e)
        {
            string pfadTestfall = Path.Combine(RegistrySettings.getRegistry("Pfad_Testfälle"), cbBereich.Text, txtTestfall.Text);
            if (!Directory.Exists(pfadTestfall))
            {
                Directory.CreateDirectory(pfadTestfall);
            }
            TestcaseXML testfall = new TestcaseXML();
            testfall.Bezeichnung = txtTestfall.Text;
            testfall.Bereich = cbBereich.Text;
            testfall.Argument1 = txtArgument1.Text;
            testfall.Argument2 = txtArgument2.Text;
            testfall.Argument3 = txtArgument3.Text;
            testfall.AblagePfad = pfadTestfall;
            if (File.Exists(Path.Combine(pfadTestfall,testfall.xmlFile)))
            {
                if(MessageBox.Show("Ein Testfall mit dieser Bezeichnung ist bereits vorhanden!\r\nSoll die Datei überschrieben werden?","Fehler",MessageBoxButton.YesNo,MessageBoxImage.Exclamation) == MessageBoxResult.No)
                {
                    this.Close();
                    return;
                }
            }
            XML.SerializeTestcase(testfall);
            this.Close();
        }

        public void GetDatafromXML(string filename)
        {
            TestcaseXML testfall = XML.DeserializeTestcase(filename);

            txtTestfall.Text = testfall.Bezeichnung;
            cbBereich.Text = testfall.Bereich;
            txtArgument1.Text = testfall.Argument1;
            txtArgument2.Text = testfall.Argument2;
            txtArgument3.Text = testfall.Argument3;
        }

        private void btnAbbrechen_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Bereicheladen()
        {
            var alleBereiche = Directory.EnumerateDirectories(RegistrySettings.getRegistry("Pfad_Testfälle"));
            foreach (string bereich in alleBereiche)
            {
                cbBereich.Items.Add(Path.GetFileName(bereich));
            }
        }
    }
}
