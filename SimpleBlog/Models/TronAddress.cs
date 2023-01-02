using NBitcoin;
using Tron.Net.Common;
using Tron.Net.Crypto;
using Tron.Net.Crypto.SHA3;

namespace SimpleBlog.Models;

public class TronAddress
{
    /*
        Use the public key P as the input, by SHA3 get the result H.
        The length of the public key is 64 bytes, SHA3 uses Keccak256. 
        Use the last 20 bytes of H, and add a byte of 0x41 in front of it, 
        then the address come out. Do basecheck to address, here is the final address. 
        All addresses start with 'T'.
        basecheck process: 
        first do sha256 calculation to address to get h1, 
        then do sha256 to h1 to get h2, 
        use the first 4 bytes as check to add it to the end of the address to get address||check, 
        do base58 encode to address||check to get the final result. 
    */
    public static string GenerateTronAddressFromPublicKey(string publicKey)
    {
        var decompressPublicKey = publicKey;
        Console.WriteLine(publicKey.StartsWith("04"));
        if (!publicKey.StartsWith("04"))
        {
            decompressPublicKey = DecompressPublicKey(publicKey);
        }

        decompressPublicKey = decompressPublicKey.Substring(2);
        var hash = Sha3.Sha3256().ComputeHash(decompressPublicKey.FromHexToByteArray());
        var numArray = new byte[20];
        Array.Copy((Array)hash, hash.Length - 20, (Array)numArray, 0, 20);
        var input = "41" + numArray.ToHexString();
        var byteArray1 = input.FromHexToByteArray();
        var sourceArray = Sha256.HashTwice(byteArray1);
        var bytes = new byte[4];
        Array.Copy((Array)sourceArray, (Array)bytes, 4);
        var byteArray2 = (input + bytes.ToHexString()).FromHexToByteArray();
        Console.WriteLine(byteArray2.ToHexString());
        Array.Copy((Array)byteArray1, (Array)byteArray2, byteArray1.Length);
        Console.WriteLine(byteArray2.ToHexString());
        return Base58.Encode(byteArray2);
    }

    private static string DecompressPublicKey(string publicKey)
    {
        var pubKey = new PubKey(publicKey);
        return pubKey.Decompress().ToHex();
    }
}