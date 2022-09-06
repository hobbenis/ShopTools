//I am not a cryptographer. This was thrown together to provide some basic assurance
//on api keys stored on an otherwise trustworthy machine. You need to review this yourself.

using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace ShopTools.Common;

public class LockedString
{
    public LockedString(string thisPassphrase, string thisPlaintext)
    {
        Passphrase = thisPassphrase;
        Plaintext = thisPlaintext;
    }
    
    [JsonConstructor]
    public LockedString(string cipher_text, string salt_string, string vector_string)
    {
        Ciphertext = cipher_text;
        SaltString = salt_string;
        IntVectorString = vector_string;
    }
    
    private string ciphertext;
    
    [JsonProperty("cipher_text")]
    public string Ciphertext
    {
        get
        {
            if (ciphertext is null)
            {
                return String.Empty;
            }
            
            return ciphertext;
        }
        set
        {
            if (ciphertext != null && ciphertext.Length > 0)
            {
                throw new Exception("Cannot edit ciphertext after object creation.");
            }
            
            ciphertext = value;
        }
    }
    
    private byte[] _salt;
    private byte[] Salt
    {
        get
        {
            if (_salt == null)
            {
                _salt = new byte[8];

                using (RNGCryptoServiceProvider myRngCsp = new RNGCryptoServiceProvider())
                {
                    myRngCsp.GetBytes(_salt);
                }
            }

            return _salt;
        }
    }
    
    [JsonProperty("salt_string")]
    public string SaltString
    {
        get
        {
            return Convert.ToBase64String(Salt);
        }
        set
        {
            _salt = Convert.FromBase64String(value);
        }
    }
    
    private byte[] intvector;
    
    [JsonProperty("vector_string")]
    public string IntVectorString
    {
        get
        {
            if (intvector is null)
            {
                return string.Empty;
            }
            
            return Convert.ToBase64String(intvector);
        }
        set
        {
            intvector = Convert.FromBase64String(value);
        }
    }
    
    private byte[] key;
    
    [JsonIgnore]
    public string Passphrase
    {
        set
        {
            using (Rfc2898DeriveBytes myKeyGen = new Rfc2898DeriveBytes(value, Salt))
            {
                key = myKeyGen.GetBytes(16);    
            }
        }
    }

    private string plaintext;
    
    [JsonIgnore]
    public string Plaintext
    {
        set
        {
            plaintext = value;

            ciphertext = Encrypt(plaintext);
        }
        get
        {
            if (Ciphertext == null || Ciphertext.Length < 1)
            {
                return string.Empty;
            }
        
            plaintext = Decrypt(Ciphertext);
            
            return plaintext;
        }
    }

    private string Encrypt(string thisPlainText)
    {
        if (key == null)
        {
            throw new Exception("No key to encrypt with.");
        }
        
        if (thisPlainText == null)
        {
            return string.Empty;
        }
        
        Aes myEncoder = Aes.Create();
        myEncoder.Key = key;
        myEncoder.GenerateIV();
        intvector = myEncoder.IV;
        
        MemoryStream myStream = new MemoryStream();
        CryptoStream myCrypto = new CryptoStream(myStream, myEncoder.CreateEncryptor(), CryptoStreamMode.Write);
        byte[] encodedText = new System.Text.UTF8Encoding(false).GetBytes(thisPlainText);
        
        myCrypto.Write(encodedText, 0, encodedText.Length);
        myCrypto.FlushFinalBlock();
        myCrypto.Close();

        return Convert.ToBase64String(myStream.ToArray());
    }
    
    private string Decrypt(string thisCipherText)
    {
        if (key == null)
        {
            throw new Exception("No key to decrypt with.");
        }

        if (thisCipherText == null)
        {
            return string.Empty;
        }
        
        byte[] encData = Convert.FromBase64String(thisCipherText);
        
        Aes myDecoder = Aes.Create();
        
        myDecoder.Key = key;
        myDecoder.IV = intvector;
        
        MemoryStream myStream = new MemoryStream();
        CryptoStream myCrypto = new CryptoStream(myStream, myDecoder.CreateDecryptor(), CryptoStreamMode.Write);
        
        myCrypto.Write(encData, 0, encData.Length);
        myCrypto.Flush();
        myCrypto.Close();
        
        string plainText = new UTF8Encoding(false).GetString(myStream.ToArray());
        
        myStream.Close();
        
        return plainText;
    }
}