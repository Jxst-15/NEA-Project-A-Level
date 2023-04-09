using System.Text;
using System.Security.Cryptography;

// This class is used for encrypting the password to add to the database for added security
public static class PasswordEncrypter
{
   public static string SHA256Encryption(string pass)
   {
        var hashedString = new StringBuilder();

        using (SHA256 hash = SHA256.Create())
        {
            byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(pass)); // Creates a byte array and computes the hash

            // Goes through each element in the array and converts the byte value into its hex value (x2)
            foreach (byte theByte in bytes)
            {
                hashedString.Append(theByte.ToString("x2"));
            }
        }
        return hashedString.ToString();
    }
}
