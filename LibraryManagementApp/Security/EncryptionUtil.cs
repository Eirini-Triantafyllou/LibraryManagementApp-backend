namespace LibraryManagementApp.Security
{
    public static class EncryptionUtil
    {

        public static string Encrypt(string plaintext)
        {
            var encryptedPassword = BCrypt.Net.BCrypt.HashPassword(plaintext);
            return encryptedPassword;
        }

        public static bool IsValidPassword(string plaintext, string cipherText)
        {
            return BCrypt.Net.BCrypt.Verify(plaintext, cipherText);
        }
    }
}
