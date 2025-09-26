using System;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;

class Client
{
    static readonly byte[] key = Encoding.UTF8.GetBytes("1234567890123456"); 
    static readonly byte[] iv  = Encoding.UTF8.GetBytes("1234567890123456"); 

    static void Main(string[] args)
    {
        const int PORT = 8085;
        const string SERVER = "127.0.0.1";

        try
        {
            using (TcpClient client = new TcpClient(SERVER, PORT))
            using (NetworkStream stream = client.GetStream())
            {
                Console.Write("Enter request (e.g., SetA-One ): ");
                string message = Console.ReadLine();

                string encryptedMessage = Encrypt(message);
                byte[] requestData = Encoding.UTF8.GetBytes(encryptedMessage);

                stream.Write(requestData, 0, requestData.Length);

                byte[] buffer = new byte[4096];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    string encryptedResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    string decryptedResponse = Decrypt(encryptedResponse);

                    Console.WriteLine("\n--- Server Response ---");
                    Console.WriteLine(decryptedResponse);
                }
                else
                {
                    Console.WriteLine("Empty response from server.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Client error: " + ex.Message);
        }
    }

    static string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            return Convert.ToBase64String(encrypted);
        }
    }

    static string Decrypt(string cipherText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] decrypted = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
