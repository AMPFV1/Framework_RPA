using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Framework_RPA
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync();
            RegistrySettings.setDefaultValues();

            Hauptbereiche rootTF = new Hauptbereiche() { Bezeichnung = "Testfälle", ImageSource = "/Images/Testfälle.png", FontWeight = "Bold", IsSelected = true };
            Hauptbereiche rootTD = new Hauptbereiche() { Bezeichnung = "Testdurchlauf", ImageSource = "/Images/testen.ico", FontWeight = "Bold" };
            Hauptbereiche rootTE = new Hauptbereiche() { Bezeichnung = "Testergebnisse", ImageSource = "/Images/Ergebnisse.png", FontWeight = "Bold" };


            trvÜbersicht.Items.Add(rootTF);
            trvÜbersicht.Items.Add(rootTD);
            trvÜbersicht.Items.Add(rootTE);

            Testfall root = new Testfall { Bezeichnung = "Testfälle", FontWeight = "Bold", ImageSource = "/Images/folder_close.png", Checkbox = "Collapsed" };
            trgTestdurchlauf.Items.Add(root);

            LadeErgebnisse();
            ladeTestfälle();
            LadeHistorischeErgebnisse();
            GetSystemInformation();

        }

        private void GetSystemInformation()
        {
            Paragraph p = tbSystemInfo.Document.Blocks.FirstBlock as Paragraph;
            p.Margin = new Thickness(0);
            tbSystemInfo.AppendText("\n------------------User Information------------------\n");
            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool bIsAdministrator = principal.IsInRole(WindowsBuiltInRole.Administrator);
            tbSystemInfo.AppendText("PC Name:".PadRight(25) + Environment.UserDomainName + "\n");
            tbSystemInfo.AppendText("User Name:".PadRight(25) + Environment.UserName + "\n");
            tbSystemInfo.AppendText("Ist Administrator? ".PadRight(25) + (bIsAdministrator ? "Ja\n" : "Nein\n"));
            tbSystemInfo.AppendText("----------------------------------------------------\n");

            tbSystemInfo.AppendText("\n------------Betriebssystem Information--------------\n");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectCollection objCol = searcher.Get();
            object freePhysicalMemory = null;
            foreach (ManagementObject mObject in objCol)
            {
                tbSystemInfo.AppendText("Betriebssystem:".PadRight(25) + mObject["Caption"] + "\n");
                tbSystemInfo.AppendText("Version:".PadRight(25) + mObject["Version"] + "\n");
                freePhysicalMemory = mObject["FreePhysicalMemory"];
            }
            tbSystemInfo.AppendText("----------------------------------------------------\n");

            tbSystemInfo.AppendText("\n-------------Arbeitsspeicher Information------------\n");
            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            objCol = searcher.Get();
            foreach (ManagementObject mObject in objCol)
            {
                tbSystemInfo.AppendText("Freier Speicher:".PadRight(25) + freePhysicalMemory + "\n");
                tbSystemInfo.AppendText("Speicherkapazität:".PadRight(25) + mObject["TotalPhysicalMemory"] + "\n");
            }
            tbSystemInfo.AppendText("----------------------------------------------------\n");

            tbSystemInfo.AppendText("\n----------------Laufwerk Information----------------\n");
            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            objCol = searcher.Get();
            foreach (ManagementObject mObject in objCol)
            {
                tbSystemInfo.AppendText("Laufwerk:".PadRight(25) + mObject["Caption"] + "\n");
                tbSystemInfo.AppendText("Beschreibung:".PadRight(25) + mObject["Description"] + "\n");
                tbSystemInfo.AppendText("Freier Speicher:".PadRight(25) + mObject["FreeSpace"] + "\n");
                tbSystemInfo.AppendText("Speicherkapazität:".PadRight(25) + mObject["Size"] + "\n");
            }
            tbSystemInfo.AppendText("----------------------------------------------------\n");
        }

        private void LadeHistorischeErgebnisse()
        {
            cbHistory.Items.Clear();
            cbHistory.Items.Add("kein Ergebnis");
            var directoryInfo = new DirectoryInfo(RegistrySettings.getRegistry("Pfad_Ergebnisse"));
            foreach (var folder in directoryInfo.GetDirectories())
            {
                cbHistory.Items.Add(folder.Name);
            }
            cbHistory.SelectedIndex = 0;
        }

        private void LadeErgebnisse()
        {
            Hauptbereiche root = (Hauptbereiche)trvÜbersicht.Items.GetItemAt(2);
            root.Items.Clear();
            var directoryInfo = new DirectoryInfo(RegistrySettings.getRegistry("Pfad_Ergebnisse"));
            string pattern = @"Ergebnis_\d{8}_\d{6}.html";
            Regex regex = new Regex(pattern);
            List<string> folder = new List<string>();
            foreach (FileInfo file in directoryInfo.GetFiles("*.html", SearchOption.AllDirectories).OrderBy(f => f.LastWriteTime).ToList())
            {
                if (regex.IsMatch(file.Name))
                {
                    Hauptbereiche rootTF = new Hauptbereiche() { Bezeichnung = file.Name.Split('_')[1], ImageSource = "/Images/folder_close.png", FontWeight = "Bold" };
                    if (!folder.Contains(file.Name.Split('_')[1]))
                    {
                        rootTF.Items.Add(new Hauptbereiche() { Bezeichnung = Path.GetFileNameWithoutExtension(file.Name), fullPathHTML = file.FullName, ImageSource = "/Images/ErgebnisHtml.png", FontWeight = "Normal", IsTestergebnis = true });
                        root.Items.Add(rootTF);
                        folder.Add(file.Name.Split('_')[1]);
                    }
                    else
                    {
                        Hauptbereiche datumroot = root.Items[folder.IndexOf(file.Name.Split('_')[1])];
                        datumroot.Items.Add(new Hauptbereiche() { Bezeichnung = Path.GetFileNameWithoutExtension(file.Name), fullPathHTML = file.FullName, ImageSource = "/Images/ErgebnisHtml.png", FontWeight = "Normal", IsTestergebnis = true });
                    }
                }
            }
        }

        private void ladeTestfälle()
        {
            trgAllgemein.Items.Clear();
            var rootDirectoryInfo = new DirectoryInfo(RegistrySettings.getRegistry("Pfad_Testfälle"));
            trgAllgemein.Items.Add(createTestfälleStruktur(rootDirectoryInfo));
            Testfall testfall = (Testfall)trgAllgemein.Items.GetItemAt(0);
            testfall.IsSelected = true;
        }

        private Testfall createTestfälleStruktur(DirectoryInfo directoryInfo)
        {
            Testfall root = new Testfall { Bezeichnung = directoryInfo.Name, FontWeight = "Bold", AblagePfad = directoryInfo.FullName, Directory = directoryInfo, ImageSource = "/Images/folder_close.png", Checkbox = "Collapsed" };
            //Auflistung der Bereiche
            foreach (var bereiche in directoryInfo.GetDirectories())
            {
                Testfall bereich = new Testfall { Bezeichnung = bereiche.Name, FontWeight = "Bold", AblagePfad = bereiche.FullName, Directory = bereiche, ImageSource = "/Images/folder_close.png", Checkbox = "Collapsed" };
                //Auflistung der TestFälle
                foreach (var testfallOrdner in bereiche.GetDirectories())
                {
                    //TestfallXML auslesen
                    foreach (var file in testfallOrdner.GetFiles())
                    {
                        if (file.Extension.EndsWith("xml") && !file.Name.StartsWith("test."))
                        {
                            TestcaseXML testfallXML = XML.DeserializeTestcase(Path.Combine(testfallOrdner.FullName, file.Name));
                            bereich.Items.Add(new Testfall
                            {
                                Bezeichnung = Path.GetFileNameWithoutExtension(file.Name),
                                ImageSource = "/Images/Testfall.png",
                                FontWeight = "Normal",
                                AblagePfad = testfallOrdner.FullName,
                                Checkbox = "Visible",
                                SkriptID = testfallXML.SkriptID,
                                SkriptPfad = testfallXML.SkriptPfad,
                                Bereich = testfallXML.Bereich
                            });

                        }
                    }
                }
                root.Items.Add(bereich);
            }
            return root;
        }

        async void InitializeAsync()
        {
            string UserDataFolder = UserDataFolder = Path.Combine(Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%"), "Microsoft", "Edge", "User Data");
            var env = await CoreWebView2Environment.CreateAsync(null, UserDataFolder);
            //await wvBrowserErgebnisse.EnsureCoreWebView2Async(env);

            string appPfad = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Console.WriteLine(Path.Combine(appPfad, "defaultPage.html"));
            wvBrowserErgebnisse.Source = new Uri(Path.Combine(appPfad, "defaultPage.html"));
        }

        private void wvBrowserErgebnisse_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            if (wvBrowserErgebnisse.CoreWebView2.Settings.AreDefaultContextMenusEnabled == true)
            {
                wvBrowserErgebnisse.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
                wvBrowserErgebnisse.CoreWebView2.Settings.AreDevToolsEnabled = true;
                wvBrowserErgebnisse.CoreWebView2.Settings.IsZoomControlEnabled = false;
                wvBrowserErgebnisse.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = true;
                wvBrowserErgebnisse.CoreWebView2.Settings.IsPasswordAutosaveEnabled = false;
                wvBrowserErgebnisse.CoreWebView2.Settings.IsStatusBarEnabled = false;
                wvBrowserErgebnisse.CoreWebView2.Settings.IsBuiltInErrorPageEnabled = false;
                wvBrowserErgebnisse.CoreWebView2.Settings.IsGeneralAutofillEnabled = false;
                wvBrowserErgebnisse.CoreWebView2.Settings.IsWebMessageEnabled = false;
            }
        }

        private void trvÜbersicht_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView auswahlBereich = (TreeView)sender;
            string pattern = @"^\d{8}$";
            Regex regex = new Regex(pattern);
            if (((Hauptbereiche)auswahlBereich.SelectedItem).Bezeichnung == "Testergebnisse" || regex.IsMatch(((Hauptbereiche)auswahlBereich.SelectedItem).Bezeichnung))
            {
                return;
            }
            foreach (TabItem fenster in tcHauptfenster.Items)
            {
                fenster.Visibility = Visibility.Collapsed;
            }
            btnTestStarten.IsEnabled = false;
            rbTestfallBearbeiten.IsEnabled = false;
            rbTestfallNeu.IsEnabled = false;
            rtbDaten.IsEnabled=false;
            rtbBeschreibung.IsEnabled=false;
            if (((Hauptbereiche)auswahlBereich.SelectedItem).Bezeichnung == "Testfälle")
            {
                tcHauptfenster.SelectedIndex = 0;
                TabItem fenster = (TabItem)tcHauptfenster.SelectedItem;
                fenster.Visibility = Visibility.Visible;
                rbTestfallBearbeiten.IsEnabled = true;
                rbTestfallNeu.IsEnabled = true;
                rtbDaten.IsEnabled = true;
                rtbBeschreibung.IsEnabled = true;
                return;
            }
            else if (((Hauptbereiche)auswahlBereich.SelectedItem).Bezeichnung == "Testdurchlauf")
            {
                //ladeTestfälleDurchlauf();
                tcHauptfenster.SelectedIndex = 1;
                TabItem fenster = (TabItem)tcHauptfenster.SelectedItem;
                fenster.Visibility = Visibility.Visible;
                btnTestStarten.IsEnabled = true;
                LadeHistorischeErgebnisse();
                return;
            }
            else if (((Hauptbereiche)auswahlBereich.SelectedItem).IsTestergebnis)
            {
                tcHauptfenster.SelectedIndex = 2;
                TabItem fenster = (TabItem)tcHauptfenster.SelectedItem;
                fenster.Visibility = Visibility.Visible;
                wvBrowserErgebnisse.Source = new Uri(((Hauptbereiche)auswahlBereich.SelectedItem).fullPathHTML);
                return;
            }
        }

        private void rbTestfallNeu_Click(object sender, RoutedEventArgs e)
        {
            Eingabeformular eingabeformular = new Eingabeformular();
            eingabeformular.ShowDialog();
            ladeTestfälle();
        }

        private void rbBeschreibung_Click(object sender, RoutedEventArgs e)
        {
            Testfall testfall = (Testfall)trgAllgemein.SelectedItem;
            if (testfall != null)
            {
                TextEditor editor = new TextEditor();
                Console.WriteLine(testfall.AblagePfad);
                Console.WriteLine(testfall.rtfFile);
                editor.filename = testfall.rtfFile;
                editor.ShowDialog();
                loadTestfallbeschreibung();
            }
            else
            {
                MessageBox.Show("Sie müssen einen Testfall auswählen!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void rbDaten_Click(object sender, RoutedEventArgs e)
        {
            Testfall testfall = (Testfall)trgAllgemein.SelectedItem;
            if (testfall != null)
            {
                string dataFile = testfall.dataFile;
                if (File.Exists(dataFile))
                {
                    Process.Start(dataFile);
                }
                else
                {
                    // Create an excel object
                    DataFile file = new DataFile();

                    // Create a new workbook with a single sheet
                    file.NewdataFile();

                    // Saving the file in a speicifed path
                    file.SaveAs(dataFile);

                    // Closing the file
                    file.Close();
                    Process.Start(dataFile);
                }
            }
            else
            {
                MessageBox.Show("Sie müssen einen Testfall auswählen!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void trgAllgemein_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            loadTestfallbeschreibung();
        }

        private void loadTestfallbeschreibung()
        {
            Testfall testfall = (Testfall)trgAllgemein.SelectedItem;
            if (testfall != null)
            {
                string rtfFile = testfall.rtfFile;
                if (File.Exists(rtfFile))
                {
                    string rtfText = File.ReadAllText(rtfFile);
                    var documentBytes = Encoding.UTF8.GetBytes(rtfText);
                    using (var reader = new MemoryStream(documentBytes))
                    {
                        reader.Position = 0;
                        rtbTestbeschreibung.SelectAll();
                        rtbTestbeschreibung.Selection.Load(reader, DataFormats.Rtf);
                    }
                }
                else
                {
                    rtbTestbeschreibung.Document.Blocks.Clear();
                    rtbTestbeschreibung.AppendText("Keine Beschreibung vorhanden!");
                }
            }
        }

        #region Testfallauswahl
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            TreeViewItem tvi = FindParent<TreeViewItem>(checkBox);
            tvi.IsSelected = true;
            Testfall root = (Testfall)trgTestdurchlauf.Items.GetItemAt(0);
            root.Items.Add(((Testfall)trgAllgemein.SelectedItem));
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            TreeViewItem tvi = FindParent<TreeViewItem>(checkBox);
            tvi.IsSelected = true;
            Testfall root = (Testfall)trgTestdurchlauf.Items.GetItemAt(0);
            root.Items.Remove(((Testfall)trgAllgemein.SelectedItem));
        }

        private static T FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            if (parent == null) return null;
            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }
        #endregion

        private void rbTestfallBearbeiten_Click(object sender, RoutedEventArgs e)
        {
            Testfall testfall = ((Testfall)trgAllgemein.SelectedItem);
            if (testfall != null)
            {
                Eingabeformular eingabeformular = new Eingabeformular();
                eingabeformular.GetDatafromXML(testfall.xmlFile);
                eingabeformular.ShowDialog();
                ladeTestfälle();
            }
            else
            {
                MessageBox.Show("Sie müssen einen Testfall auswählen!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnTestStarten_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            string dateTimeNow = DateTime.Now.ToString().Replace(".", "").Replace(":", "").Replace(" ", "_");
            string ErgebnisPfad = Path.Combine(RegistrySettings.getRegistry("Pfad_Ergebnisse"), dateTimeNow);
            if (!Directory.Exists(ErgebnisPfad))
            {
                Directory.CreateDirectory(ErgebnisPfad);
            }
            Testfall root = (Testfall)trgTestdurchlauf.Items[0];
            ProtokollXML protokollXML = new ProtokollXML();
            foreach (Testfall testfall in root.Items)
            {

                TestcaseXML testfallXML = new TestcaseXML();
                testfallXML.AblagePfad = testfall.AblagePfad;
                testfallXML.DatenPfad = testfall.dataFile;
                testfallXML.SkriptID = testfall.SkriptID;
                testfallXML.SkriptPfad = testfall.SkriptPfad;
                testfallXML.Bereich = testfall.Bereich;
                testfallXML.Bezeichnung = testfall.Bezeichnung;
                testfallXML.ErgebnisXML = Path.Combine(ErgebnisPfad, testfall.Bezeichnung + ".xml");
                XML.SerializeTestcase(testfallXML);
                XML.CreateStartXML(Path.Combine(testfall.AblagePfad, testfall.Bezeichnung + ".xml"));
                Testdurchlauf.RunTest(protokollXML, testfall, dateTimeNow, cbHistory.Text);

            }
            TextRange textSystemInfo = new TextRange(tbSystemInfo.Document.ContentStart, tbSystemInfo.Document.ContentEnd);
            TextRange textAnmerkung = new TextRange(rtbAnmerkungen.Document.ContentStart, rtbAnmerkungen.Document.ContentEnd);
            protokollXML.CreateOverviewResults(dateTimeNow, textSystemInfo.Text, textAnmerkung.Text);
            XSLTTransformer transformer = new XSLTTransformer();
            transformer.CreateHTML(ErgebnisPfad, dateTimeNow);
            LadeErgebnisse();
            this.WindowState = WindowState.Maximized;
        }

        private void rbbtnTestfälle_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(RegistrySettings.getRegistry("Pfad_Testfälle"));
        }

        private void rbbtnErgebnisse_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(RegistrySettings.getRegistry("Pfad_Ergebnisse"));
        }
    }
    class TreeViewLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TreeViewItem item = (TreeViewItem)value;
            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
            return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }


    public class Hauptbereiche
    {
        public Hauptbereiche()
        {
            this.Items = new ObservableCollection<Hauptbereiche>();
        }

        public string Bezeichnung { get; set; }
        public string ImageSource { get; set; }
        public string FontWeight { get; set; }
        public string fullPathHTML { get; set; }
        public bool IsTestergebnis { get; set; }
        public bool IsSelected { get; set; }

        public ObservableCollection<Hauptbereiche> Items { get; set; }
    }
}
