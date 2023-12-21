using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace Framework_RPA
{
    public class XSLTTransformer
    {
        public void CreateHTML(string pfadErgebnisse, string dateTimeNow)
        {
            var directoryInfo = new DirectoryInfo(pfadErgebnisse);
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (file.Name != "Ergebnis_" + dateTimeNow + ".xml")
                {
                    XMLtoHTML(file, Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "XSLT", "Ergebnis.xslt"), Path.GetFileNameWithoutExtension(file.Name));
                }
                else if (file.Name == "Ergebnis_" + dateTimeNow + ".xml")
                {
                    XMLtoHTML(file, Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "XSLT", "GesamtErgebnis.xslt"), Path.GetFileNameWithoutExtension(file.Name));
                }
            }
        }

        private void XMLtoHTML(FileInfo file, string xsltFile, string filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file.FullName);

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xsltFile);

            xslt.Transform(doc, XmlWriter.Create(Path.Combine(file.DirectoryName, filename + ".html"), xslt.OutputSettings));
        }
    }
}
