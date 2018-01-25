using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace ConvertToCSVapp
{
    public partial class Form1 : Form
    {
        public String filePath = "";
        public Form1()
        {
            InitializeComponent();
        }

        public string FilePath_CSVToXML { get; private set; }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialogOpen = new OpenFileDialog();
                dialogOpen.Title = "C# Corner Open File Dialog";
                dialogOpen.InitialDirectory = @"c:\";
                dialogOpen.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                dialogOpen.FilterIndex = 2;
                dialogOpen.RestoreDirectory = true;
                if (dialogOpen.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = dialogOpen.FileName;
                    filePath = dialogOpen.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                    //XDocument xDocument = XDocument.Load(filePath);

                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);
                string result;
                using (var stringWriter = new StringWriter())
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    doc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    result = stringWriter.GetStringBuilder().ToString();
                }

                //string data = doc.ToString();

                XmlElement root = doc.DocumentElement;
                XmlNodeList nodes = root.SelectNodes("food");
                List<String> csvLines = new List<string>();
                foreach (XmlNode node in nodes)
                {
                    //listBox1.Items.Add(result);
                    String parsedXml = "";
                    parsedXml += node["ProductID"].InnerText + ";";
                    parsedXml += node["name"].InnerText + ";";
                    parsedXml += node["price"].InnerText + ";";
                    parsedXml += node["description"].InnerText + ";";
                    parsedXml += node["calories"].InnerText + ";";
                    csvLines.Add(parsedXml);
                    listBox1.Items.Add(parsedXml);

                }


                String csvFile = filePath.Replace("xml", "csv");
                using (StreamWriter outputFile = new StreamWriter(csvFile))
                {
                    foreach (string line in csvLines)
                        outputFile.WriteLine(line);
                }

                MessageBox.Show("Utowrzono plik CSV w ścieżce źródłowej o nazwie " + csvFile);
            }
            catch (Exception ex2)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex2.GetBaseException());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
