using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class SecretFileCreator
    {
        public const string SecretsFileName = "Secrets.keys";

        public static bool DoesSecretsFileExist()
        {
            if (File.Exists(GetSecretsFilePath()))
                return true;

            return false;
        }

        public static string GetSecretsFilePath ()
        {
            string currentAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);          

            return currentAppPath + "\\" + SecretsFileName;
        }

        public static void CreateSecretFile()
        {
            string currentAppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string secretFilePath = currentAppPath + "\\" + SecretsFileName;
            if (File.Exists(secretFilePath))
            {
                return;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);

                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.AutoCompleteOnClose = true;

                    writer.WriteStartObject();
                    writer.WritePropertyName(SecretNames.USER_POOL_ID);
                    writer.WriteValue("Temp");
                    writer.WritePropertyName(SecretNames.CLIENT_ID);
                    writer.WriteValue("Temp");
                    writer.WritePropertyName(SecretNames.CLIENT_SECRET);
                    writer.WriteValue("Temp");
                    writer.WritePropertyName(SecretNames.AWS_REGION);
                    writer.WriteValue("Temp");
                    writer.WriteEndObject();
                }

                File.WriteAllText(secretFilePath, sb.ToString());

                return;
            }

        }
    }
}
