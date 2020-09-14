using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Thoth.Static_Resources;

namespace Thoth.Models
{
    public class User
    {
        /*
        * Max username length should be 12
        * Max password length should be 20
        */

        //No recovery options will be implemented
        public string username;
        private string usernameHash; //hash of username
        private string passwordHash; // hash of username

        private Aes aes = null;
        public Aes GetAes() => aes;

        public User(string username, string password)
        {
            SetUsername(username);
            SetPassword(password);
            GenerateAes();
        }

        public User(Aes aes)
        {
            this.aes = aes;
        }

        public Aes GenerateAes()
        {
            if (aes == null)
            {
                aes = Aes.Create();
                aes.KeySize = 256;
                aes.Key = EncryptionCore.ComputeSha256Hash(usernameHash + passwordHash);
                aes.GenerateIV();

                byte[] key = new byte[aes.Key.Length + aes.IV.Length + 2];
                key[0] = Convert.ToByte(aes.Key.Length);
                key[1] = Convert.ToByte(aes.IV.Length);

                int i;
                for (i = 2; i < aes.Key.Length + 2; i++)
                    key[i] = aes.Key[i - 2];

                for (; i < key.Length; i++)
                    key[i] = aes.IV[i - (aes.Key.Length + 2)];

                File.WriteAllBytes(GetAvailableFileName(), key);
                return aes;
            }

            return aes;
        }

        private string GetAvailableFileName()
        {
            if (!Directory.Exists("users")) Directory.CreateDirectory("users");

            for (int i = 0; true; i++)
            {
                string file = $@"users\{i}";
                if (!File.Exists(file))
                    return file;
            }
        }

        public void SetUsername(string username)
        {
            this.username = username;
            usernameHash = Encoding.UTF8.GetString(EncryptionCore.ComputeSha256Hash(username));
        }

        public void SetPassword(string password)
        {
            passwordHash = Encoding.UTF8.GetString(EncryptionCore.ComputeSha256Hash(password));
        }

        public bool AuthenticateLoginDetails(string username, string password)
        {
            if (!Directory.Exists("users")) Directory.CreateDirectory("users");

            string usernameHash = Encoding.UTF8.GetString(EncryptionCore.ComputeSha256Hash(username));
            string passwordHash = Encoding.UTF8.GetString(EncryptionCore.ComputeSha256Hash(password));

            byte[] key = EncryptionCore.ComputeSha256Hash(usernameHash + passwordHash);
            if(key.SequenceEqual(aes.Key))
            {
                this.username = username;
                this.usernameHash = Encoding.UTF8.GetString(EncryptionCore.ComputeSha256Hash(username));
                this.passwordHash = Encoding.UTF8.GetString(EncryptionCore.ComputeSha256Hash(password));
                return true;
            }

            return false;
        }
    }
}
