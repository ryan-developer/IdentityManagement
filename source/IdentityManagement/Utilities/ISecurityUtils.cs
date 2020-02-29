using System;
using System.Security.Cryptography;

namespace IdentityManagement.Utilities
{
    public interface ISecurityUtils
    {
        string CreateRandomKey(int length = 32);
    }

    public class SecurityUtils : ISecurityUtils
    {
        public string CreateRandomKey(int length = 32)
        {
            var rando = RandomNumberGenerator.Create();
            byte[] rawKey = new byte[length];
            rando.GetNonZeroBytes(rawKey);

            return Convert.ToBase64String(rawKey);
        }
    }
}
