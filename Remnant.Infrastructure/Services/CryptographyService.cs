using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Remnant.Core.Services
{
  public static class CryptographyService
  {
    public static string SHA1(string source, string salt)
    {
      var sha1 = new SHA1Managed();
      var ae = new ASCIIEncoding();

      var sourceBytes = ae.GetBytes(salt + source);

      var hashBytes = sha1.ComputeHash(sourceBytes);

      return hashBytes.Aggregate("", (current, b) => current + string.Format("{0:x2}", b));
    }

    public static string TripleDESEncode(string plain, string key)
    {
      var des = new TripleDESCryptoServiceProvider
                {
                  IV = new byte[8]
                };

      var pdb = new PasswordDeriveBytes(key, new byte[-1 + 1]);

      des.Key = pdb.CryptDeriveKey("RC2", "MD5", 128, new byte[8]);

      var ms = new MemoryStream((plain.Length * 2) - 1);

      var encStream = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

      var plainBytes = Encoding.UTF8.GetBytes(plain);

      encStream.Write(plainBytes, 0, plainBytes.Length);

      encStream.FlushFinalBlock();

      var encryptedBytes = new byte[(int)ms.Length - 1 + 1];

      ms.Position = 0;

      ms.Read(encryptedBytes, 0, (int)ms.Length);

      encStream.Close();

      return Convert.ToBase64String(encryptedBytes);
    }

    public static string TripleDESDecode(string secure, string key)
    {
      var des = new TripleDESCryptoServiceProvider
                {
                  IV = new byte[8]
                };

      var pdb = new PasswordDeriveBytes(key, new byte[-1 + 1]);

      des.Key = pdb.CryptDeriveKey("RC2", "MD5", 128, new byte[8]);

      var encryptedBytes = Convert.FromBase64String(secure);

      var ms = new MemoryStream(secure.Length);

      var decStream = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

      decStream.Write(encryptedBytes, 0, encryptedBytes.Length);

      decStream.FlushFinalBlock();

      var plainBytes = new byte[(int)ms.Length - 1 + 1];

      ms.Position = 0;

      ms.Read(plainBytes, 0, (int)ms.Length);

      decStream.Close();

      return Encoding.UTF8.GetString(plainBytes);
    }


  }
}
