using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace TaskManagementSystem.Helpers
{
    public static class Hasher
    {
        public static string Encrypt(string value) =>
            Convert.ToBase64String(MachineKey.Protect(Encoding.UTF8.GetBytes(value)));

        public static string Decrypt(string value) =>
            Encoding.UTF8.GetString(MachineKey.Unprotect(Convert.FromBase64String(value)));

        public static string DecryptEmail(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch
            {
                decrypted = "";
            }
            return decrypted;
        }

        public static string EncryptEmail(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
    }
}