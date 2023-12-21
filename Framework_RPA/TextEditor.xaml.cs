using System;
using System.IO;
using System.Windows;

namespace Framework_RPA
{
    /// <summary>
    /// Interaktionslogik für TextEditor.xaml
    /// </summary>
    public partial class TextEditor : Window
    {
        public string filename;
        public TextEditor()
        {
            InitializeComponent();
        }
        private void btnSpeichern_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(filename);
            File.WriteAllText(filename, rtfBox.GetRTF());
            this.Close();
        }

        private void btnAbbrechen_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void loadRTFFile()
        {
            if (File.Exists(filename))
            {
                string rtfText = File.ReadAllText(filename);
                rtfBox.SetRTF(rtfText);
            }
        }

        private void DockPanel_Loaded(object sender, RoutedEventArgs e)
        {
            loadRTFFile();
        }
    }
}
