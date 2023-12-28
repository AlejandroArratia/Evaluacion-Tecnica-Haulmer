// KeyManager.cs
using System;
using System.Security.Cryptography;
using System.Text;

public static class KeyManager
{
    public static byte[] ObtenerClave()
    {
        return Encoding.UTF8.GetBytes("YourSecretKey123");
    }

public static byte[] ObtenerIV()
{
    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
    {
        var iv = new byte[16];
        rng.GetBytes(iv);
        return iv;
    }
}


}
