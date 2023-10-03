namespace StellarStreamAPI.Security
{
    internal static class BCryptHash
    {
        internal static string Hash(string value, int workload = 11)
        {
            string hashedValue = BCrypt.Net.BCrypt.HashPassword(value, workload);
            return hashedValue;
        }

        internal static bool Verify(string value, string hashedValue)
        {
            bool isValid = BCrypt.Net.BCrypt.Verify(value, hashedValue);
            return isValid;
        }

        internal static string Rehash(string value, string hashedValue, int workload)
        {
            bool needsRehash = BCrypt.Net.BCrypt.PasswordNeedsRehash(hashedValue, workload);

            if (!needsRehash)
            {
                return hashedValue;
            }

            bool isValid = Verify(value, hashedValue);

            if (isValid)
            {
                string newHash = Hash(value, workload);
                return newHash;
            }

            return hashedValue;
        }
    }
}
