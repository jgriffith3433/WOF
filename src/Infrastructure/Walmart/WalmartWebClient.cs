using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace WOF.Application.Common.Security;

public class WalmartWebClient : WebClient
{
    public readonly string Key = @"-----BEGIN PRIVATE KEY-----
MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDPAzvMRjxrGEf1
vFP8c7IQWjHiRZ5G+DLLBntUPv01Ga+UGwEyyOcwhCEQ45ZhCukEsnFdxdWQKr7G
3IOQeeVq7/47BgHGLIOCsPvhTQGR30/QWCUtTOHDzdXjAqfw+/DvIGuG2Jp49JUJ
ndCg5vuWpmQiHC8qQHjF0uOWB/vnndCLADGqjv+ZUVLFQpPF6ANB64D4QHDrKCHJ
2YPioN4kVcCpusNM21JBN2JzdTTH0S9Eehk/dqzcTw9iz43uXmtBSx1H+rvU02RB
ksHL0Fge4KQNnB0+1wOF6eQsaMW6/hQMPDPmFA/nXE+AZMfqwPEaHTqFLwRzt89g
pQLnx7sZAgMBAAECggEAMcJcOFDVqwRuH6JCKdeBgK69eN81GWgFhYmsUWbGpGHl
M1t+YT+rWp0X+aBLAE6IDkn6OAkauYmcy94rfanfPGItoPBjssf2YI68LL6+1tpv
q+vUG9x0FAXBGM0Fo0Zb9o67afFcyCA6RtJ5WIPNC7w5K33M1IIkxaBYdjK7GUQG
BV5axBSAtLHDUPT+voc0ARga6JnCtxElvW9dYT2IvTZPLCWIiAgcbmEvta3OgZqH
j2eNPpo3tAG45n+WaN86ZBjFcrfN2F6Qb1A1a8vbhfWLTENYfrFBcgQ8yZQ/oivD
dGLjIvImEmoGBozVqyWOrQRiA2oTkKbAvzE04szZ1QKBgQD0GuoeJ/+yv4ABc75Z
JEcRP4oUNNR9c+6dUI3wzH3g9A62HcBuEnAIW5OF2YWLv7WtzI5mgW4PSbhR5jzT
zs+7KOaBzK0gN4lX6uToT2pQCWQjM4shSwYDE9wlaZuQGFNeVuDboFfIKBXq1t/K
oyTpkTazACR/2eeawc9W5CyTfwKBgQDZGZv3CApB/ffdUwGso5RkGhKTWQQvIB9K
rR4XZCiUZA1P1UYqK6L5DqWyVcFf8eArz6hHb2/aM3MFs7As1Km5nO629DSfAg3k
AYOaAouEY8xW9c/358MnXKgM5etNfCErIoIktgpl3fzRgE8+gtLxqwKYGVAF9lMI
7DjZiF0dZwKBgQCksdgNmp0ZQ70gva/KwwAz8fO+aFqJfgObHjN2KPIxKVkXpIEl
gskVynuBDl7dB+6TIXVeUaspI2r5zuZxXZKoSxMiti6EkxPWPoRM/O/UqlFmsqsH
PnAC+Y5Jq7Qqh08QUnuJkuhHAkyvUmRRers33yLRqKH3pNRvhJ3YmUA+DwKBgAqA
RJ0EVIqwoCaidzhEiU72O+PIsH0fDqRsD0KOY7AZztMHu+caST3GyZkAsOPVLUSx
PLCH4V8qkeu6r1Db0IHb52gOE+WFEervb9ApQ1paAW5LYY3aNgRXZQGKtYD/+hK+
DpF8BLC8thfkHvElHtL1tKBqyQYgzg9mbbGH+QkNAoGATHfYb2bbZu1TkSJyc3AA
84GBiQN5PQvkc5w4Z7WuTBAHvJSeysRnAJabfK8t1QHuAI4fLExNoZbY9msTnSmz
gJ6eq0XTcvSoRrOVhVxIIcS1EaR5JRcASsfqnMtBpgTMUCP8TvpurpQs1sNhG5di
Dj1H1rvYeKTt6XvYkjt9qCA=
-----END PRIVATE KEY-----";

    protected override WebRequest GetWebRequest(Uri address)
    {
        var request = base.GetWebRequest(address) as HttpWebRequest;
        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
        return request;
    }

    /// <summary>
    /// Begining of time according to Java world.
    /// </summary>
    private readonly DateTime JanuaryFirst1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// The calculated signature
    /// </summary>
    /// <returns>
    /// The calculated signature
    /// </returns>
    public string GetWalmartSignature(string privateKey, string consumerId, string timeStamp, string version)
    {
        // Append values into string for signing
        var message = consumerId + "\n" + timeStamp + "\n" + version + "\n";

        RsaKeyParameters rsaKeyParameter;
        try
        {
            StringReader stringReader = new StringReader(privateKey);
            PemReader pemReader = new PemReader(stringReader, new PasswordFinder("Grif#Over9000"));
            RsaPrivateCrtKeyParameters keyParams = (RsaPrivateCrtKeyParameters)pemReader.ReadObject();

            rsaKeyParameter = keyParams;
        }
        catch (Exception)
        {
            throw new Exception("Unable to load private key");
        }

        var signer = SignerUtilities.GetSigner("SHA256withRSA");
        signer.Init(true, rsaKeyParameter);
        var messageBytes = Encoding.UTF8.GetBytes(message);
        signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
        var signed = signer.GenerateSignature();
        var hashed = Convert.ToBase64String(signed);
        return hashed;
    }

    /// <summary>
    /// Get the TimeStamp as a string equivalent to Java System.currentTimeMillis
    /// </summary>
    /// <returns>
    /// Generated sign string
    /// </returns>
    public string GetTimestampInJavaMillis()
    {
        var millis = (DateTime.UtcNow - JanuaryFirst1970).TotalMilliseconds;
        return Convert.ToString(Math.Round(millis), CultureInfo.InvariantCulture);
    }

    private class PasswordFinder : IPasswordFinder
    {
        private string password;
        public PasswordFinder(string pwd) => password = pwd;
        public char[] GetPassword() => password.ToCharArray();
    }
}
