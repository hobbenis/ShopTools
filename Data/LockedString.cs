//I am not a cryptographer. This was thrown together to provide some basic assurance
//on api keys stored on an otherwise trustworthy machine.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Shared
{
    public class LockedString
    {
        private byte[] b_salt;
        public string salt
        {
            get
            {
                if (b_salt == null) { b_salt = GetSalt(); }
                return Convert.ToBase64String(b_salt);
            }
            set { b_salt = Convert.FromBase64String(value); }
        }
        
        private string myPass;
        public void SetPass(string thisPass)
        {
            myPass = thisPass;
        }

        private byte[] stored_key;
        private byte[] key
        {
            get
            {
                if (stored_key == null)
                {
                    if (b_salt == null)
                    {
                        b_salt = GetSalt();
                    }
                    
                    using (Rfc2898DeriveBytes myKeyGen = new Rfc2898DeriveBytes(myPass, b_salt))
                    {
                        stored_key = myKeyGen.GetBytes(16);    
                    }
                }

                return stored_key;
            }
        }

        private string p_plaintext;
        public string GetPlainText()
        {
            if (p_ciphertext == null || p_ciphertext.Length < 1) { return p_plaintext; }
            
            p_plaintext = Decrypt(p_ciphertext);
            return p_plaintext;
        }
        public void SetPlainText(string thisText)
        {
            p_plaintext = thisText;
        }

        private string p_ciphertext;
        public string ciphertext
        {
            get
            {
                if (p_plaintext == null || p_plaintext.Length < 1) { return p_ciphertext; }
                
                p_ciphertext = Encrypt(p_plaintext);
                return p_ciphertext;
            }
            set
            {
                p_ciphertext = value;
            }
        }
        
        private string Encrypt(string data)
        {
            if (key == null)
            {
                throw new Exception("No key to encrypt with.");
            }
            
            if (data == null)
            {
                return string.Empty;
            }
            
            Aes myEncoder = Aes.Create();
            myEncoder.Key = key;
            myEncoder.GenerateIV();
            b_iv = myEncoder.IV;
            
            MemoryStream myStream = new MemoryStream();
            CryptoStream myCrypto = new CryptoStream(myStream, myEncoder.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] encodedText = new System.Text.UTF8Encoding(false).GetBytes(data);
            
            myCrypto.Write(encodedText, 0, encodedText.Length);
            myCrypto.FlushFinalBlock();
            myCrypto.Close();

            return Convert.ToBase64String(myStream.ToArray());
        }
        
        private string Decrypt(string encString)
        {
            if (key == null)
            {
                throw new Exception("No key to decrypt with.");
            }

            if (encString == null)
            {
                return string.Empty;
            }
            
            byte[] encData = Convert.FromBase64String(encString);
            
            Aes myDecoder = Aes.Create();
            
            myDecoder.Key = key;
            myDecoder.IV = b_iv;
            
            MemoryStream myStream = new MemoryStream();
            CryptoStream myCrypto = new CryptoStream(myStream, myDecoder.CreateDecryptor(), CryptoStreamMode.Write);
            
            myCrypto.Write(encData, 0, encData.Length);
            myCrypto.Flush();
            myCrypto.Close();
            
            string plainText = new UTF8Encoding(false).GetString(myStream.ToArray());
            
            myStream.Close();
            
            return plainText;
        }
        
        private byte[] GetSalt()
        {
            byte[] thisSalt = new byte[8];
            
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(thisSalt);
            }
            
            return thisSalt;
        }

        private byte[] b_iv;
        public string iv
        {
            get
            {
                if (b_iv == null) { return string.Empty; }
                return Convert.ToBase64String(b_iv);
            }
            set{ b_iv = Convert.FromBase64String(value); }
        }
    }
}
