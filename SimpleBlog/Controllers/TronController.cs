using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models;
using Tron.Net.Client;
using Tron.Net.Common;
using Tron.Net.Crypto;


namespace SimpleBlog.Controllers;

public class TronController : ApiController
{
    [HttpGet("address/generate")]
    public IActionResult GenerateTronAddress([FromQuery(Name = "PrivateKey")] string prvKey)
    {
        var keyPair = ECKey.FromPrivateHexString(prvKey);
        var address = WalletAddress.MainNetWalletAddress(keyPair);

        return Ok(new
        {
            hex = address.Value.ToHexString(), // public key
            base58 = address.ToString() // address base58
        });
    }

    [HttpGet("address/from_public")]
    public IActionResult GenerateTronAddressFromPublic([FromQuery(Name = "PublicKey")] string pubKey)
    {
        return Ok(TronAddress.GenerateTronAddressFromPublicKey(pubKey));
    }
}