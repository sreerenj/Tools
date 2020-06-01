using System;
using System.Xml;
using System.Linq;
using System.IO;

namespace ExtractMetadataDiff
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 3)
            {
                Console.WriteLine("The required arguments doesn't exist!");
                return;
            }
            FileInfo fileInfo1 = new FileInfo(args[0]);
            FileInfo fileInfo2 = new FileInfo(args[1]);
            if(fileInfo1.Name != fileInfo2.Name)
            {
                Console.WriteLine("The file names doesn't match! Are you sure to continue? (Y/N)");
                var key = Console.ReadKey();
                if (key.Key != ConsoleKey.Y)
                    return;
            }
            XmlDocument newXmlDocument = new XmlDocument();
            newXmlDocument.Load(args[0]);

            XmlDocument oldXmlDocument = new XmlDocument();
            oldXmlDocument.Load(args[1]);
            
            XmlElement newXmlElement = newXmlDocument.DocumentElement;
            XmlElement oldXmlElement = oldXmlDocument.DocumentElement;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", String.Empty));
            XmlElement rootElement = xmlDocument.CreateElement("Culture");
            rootElement.SetAttribute("language", "ja");

            for (int i = 0; i < newXmlElement.ChildNodes.Count; i++)
            {
                if (newXmlElement.ChildNodes[i].LastChild.InnerText != oldXmlElement.ChildNodes[i].LastChild.InnerText)
                {
                    XmlElement newElement = xmlDocument.CreateElement("String");
                    newElement.InnerXml = newXmlElement.ChildNodes[i].InnerXml;
                    rootElement.AppendChild(newElement);
                }
                    
            }
            xmlDocument.AppendChild(rootElement);
            FileInfo destinationFile = new FileInfo(args[2]);
            string fileName = destinationFile.FullName;
            if(destinationFile.Exists)
            {
                Console.WriteLine($"The file {args[2]} already exists! Do you want to overwrite? (Y/N)");
                var key = Console.ReadKey();
                if(key.Key != ConsoleKey.Y)
                {
                    fileName = fileName.Replace(".xml", "_" + DateTime.Now.ToString("MMddyyyyHHmmss") + ".xml");
                }
            }
            xmlDocument.Save(fileName);
        }
    }
}
