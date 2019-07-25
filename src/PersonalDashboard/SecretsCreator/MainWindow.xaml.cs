using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SecretsCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string secretsFile = string.Empty;
        string secretsFileName = string.Empty;
        FileStream secretsFileStream = null;
        StreamReader sr = null;
        string fileContents = null;

        // TODO: Handle negative scenarios
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_LoadSecretsFile(object sender, RoutedEventArgs e)
        {
            OpenSecretsFile();

            LoadUIWithSecrets();
        }

        private void LoadUIWithSecrets()
        {            
            JsonTextReader reader = new JsonTextReader(new StringReader(fileContents));

            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    listBox_secretsList.Items.Add(string.Format("{0} : {1}", reader.TokenType, reader.Value));
                }
            }
        }

        private void OpenSecretsFile()
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "*.json;";
            if (true == fileDlg.ShowDialog())
            {
                secretsFileStream = File.Open(fileDlg.FileName, FileMode.Open, FileAccess.ReadWrite);

                sr = File.OpenText(fileDlg.FileName);

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    fileContents += line;
                }
            }
        }

        private void Button_AddNewSecret(object sender, RoutedEventArgs e)
        {
            if(secretsFile == string.Empty)
            {
                SaveFileDialog createFileDlg = new SaveFileDialog();
                createFileDlg.Title = "Choose the path to save the secrets file";
                createFileDlg.Filter = "JSON Files | *.json;"; // Image files (*.bmp, *.jpg)|*.bmp;*.jpg|All files (*.*)|*.*"'

                if (createFileDlg.ShowDialog() == true)
                {
                    secretsFileStream = CreateSecretsFile(createFileDlg.FileName);

                    string json = CreateSecretJSon();

                    Byte[] txt = new UTF8Encoding(true).GetBytes(json);
                    secretsFileStream.Write(txt, 0, txt.Length);
                }
            }
            else
            {
                string json = CreateSecretJSon();                
                Byte[] txt = new UTF8Encoding(true).GetBytes(json);
                secretsFileStream.Write(txt, (int) secretsFileStream.Length + 1, txt.Length);
            }
        }

        private string CreateSecretJSon()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName(textBox_secretName.Text);
                writer.WriteValue(textBox_secretValue.Text);
                writer.WriteEnd();
                writer.WriteEndObject();
            }

            return sb.ToString();
        }

        private FileStream CreateSecretsFile(string fileName)
        {
            return File.Create(fileName);

        }
    }
}
