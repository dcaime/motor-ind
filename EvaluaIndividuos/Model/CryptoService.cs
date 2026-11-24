namespace EvaluaIndividuos.Model
{
    using System.Security.Cryptography;
    using System.Text;

    public static class CryptoService
    {
        private static readonly string Key = "YourSecretKeyMustBe32CharsLong!!";
        private static readonly string IV = "1234567890123456";

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;

            try
            {
                var buffer = Convert.FromBase64String(cipherText);
                using Aes aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = Encoding.UTF8.GetBytes(IV);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using MemoryStream ms = new MemoryStream(buffer);
                using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using StreamReader sr = new StreamReader(cs);

                return sr.ReadToEnd();
            }
            catch
            {
                return cipherText;
            }
        }
    }
}
