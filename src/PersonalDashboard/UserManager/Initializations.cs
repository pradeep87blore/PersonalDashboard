using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace UserManager
{
    public class Initializations
    {
        public static string InitializeUserManager()
        {
            if(!SecretFileCreator.DoesSecretsFileExist())
            {
                SecretFileCreator.CreateSecretFile();
                return "Secrets file created but doesn't have valid values. Update and restart the application";
            }

            return string.Empty; // Secrets file already exists. Nothing to be done.
        }
    }
}
