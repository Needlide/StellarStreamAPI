using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System.Security.Cryptography;

namespace StellarStreamAPI.Security.JWT
{
    public static class JWTKeyReader
    {
        public static RSA ReadPrivateKey(string privateKeyPath)
        {
            using var reader = File.OpenText(privateKeyPath);
            var pemReader = new PemReader(reader);
            var keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            var privateParams = (RsaPrivateCrtKeyParameters)keyPair.Private;
            var rsa = RSA.Create();
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = privateParams.Modulus.ToByteArrayUnsigned(),
                Exponent = privateParams.PublicExponent.ToByteArrayUnsigned(),
                P = privateParams.P.ToByteArrayUnsigned(),
                Q = privateParams.Q.ToByteArrayUnsigned(),
                DP = privateParams.DP.ToByteArrayUnsigned(),
                DQ = privateParams.DQ.ToByteArrayUnsigned(),
                InverseQ = privateParams.QInv.ToByteArrayUnsigned(),
                D = privateParams.Exponent.ToByteArrayUnsigned()
            });
            return rsa;
        }

        public static RSA ReadPublicKey(string publicKeyPath)
        {
            using var reader = File.OpenText(publicKeyPath);
            var pemReader = new PemReader(reader);
            var obj = pemReader.ReadObject();
            var publicParams = (RsaKeyParameters)obj;
            var rsa = RSA.Create();
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = publicParams.Modulus.ToByteArrayUnsigned(),
                Exponent = publicParams.Exponent.ToByteArrayUnsigned(),
            });
            return rsa;
        }
    }
}
