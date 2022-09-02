using System;
using NUnit.Framework;
using ShopTools.Data.Common;

namespace DataTests;

public class LockedStringTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestLockedString()
    {
        LockedString myTestEncoder = 
            new LockedString("password", "plaintext");

        LockedString myTestDecoder = 
            new LockedString(myTestEncoder.Ciphertext, myTestEncoder.SaltString, myTestEncoder.IntVectorString);
        myTestDecoder.Passphrase = "password";
        
        Console.WriteLine($"Encoder: {myTestEncoder.IntVectorString}, {myTestEncoder.SaltString}");
        Console.WriteLine($"Decoder: {myTestDecoder.IntVectorString}, {myTestDecoder.SaltString}");
        
        Assert.IsTrue(myTestDecoder.Plaintext.Equals("plaintext"));
    }
}