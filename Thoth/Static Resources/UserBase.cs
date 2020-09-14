using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Thoth.Models;

namespace Thoth.Static_Resources
{
    static class UserBase
    {
        private static readonly List<User> Users = new List<User>();

        public static void AddUser(User user)
        {
            Users.Add(user);
        }

        public static void LoadUsers()
        {
            var users = Directory.GetFiles("users");
            foreach (var userFile in users)
            {
                RetrieveUserEncryptionData(File.ReadAllBytes(userFile), out byte[] key, out byte[] iv);
                Aes aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;

                Users.Add(new User(aes));
            }
        }

        static void RetrieveUserEncryptionData(byte[] rawdata, out byte[] key, out byte[] iv)
        {
            //Get encryption key details
            int ivLength = Convert.ToInt32(rawdata[1]);
            int keyLength = Convert.ToInt32(rawdata[0]);

            //Set encryption key objects
            iv = new byte[ivLength];
            key = new byte[keyLength];
            Array.Copy(rawdata, 2, key, 0, keyLength);
            Array.Copy(rawdata, 2 + keyLength, iv, 0, ivLength);
        }

        public static User FindUser(string username, string password)
        {
            foreach (var user in Users)
            {
                if (user.AuthenticateLoginDetails(username, password)) return user;
            }

            return null;
        }
    }
}
