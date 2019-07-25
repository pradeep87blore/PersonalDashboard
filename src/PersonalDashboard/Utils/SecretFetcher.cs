using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    // Open the secrets file and read the contents. Then return the requested values
    public class SecretFetcher
    {
        static string secretValuesJson = string.Empty;

        static Dictionary<string, string> secrets = new Dictionary<string, string>();
        public static string GetSecretValue(string secretName)
        {
            var file = SecretFileCreator.GetSecretsFilePath();

            if(secretValuesJson == string.Empty)
            {
                secretValuesJson = File.ReadAllText(file, Encoding.UTF8);
                ReadAllSecrets();
            }

            return SearchForSecretValue(secretName);
        }

        private static string SearchForSecretValue(string secretName)
        {
            string secretValue = "";
            if (secrets.TryGetValue(secretName, out secretValue))
                return secretValue;

            return string.Empty;
        }

        private static void ReadAllSecrets()
        {
            JsonTextReader reader = new JsonTextReader(new StringReader(secretValuesJson));
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string key = (string)reader.Value;

                        reader.Read();

                        if (reader.Value != null && reader.TokenType == JsonToken.String)
                        {
                            string value = (string)reader.Value;
                            secrets.Add(key, value);
                        }
                    }
                }
            }
        }
    }
}
