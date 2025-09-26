using System;
using System.Net.Sockets;
using System.Text;

class Client
{
    const string localHost = "127.0.0.1";
    const int PORT = 8085;

    static void Main(string[] args)
    {
        try
        {
            using (TcpClient client = new TcpClient(localHost, PORT))
            {
                Console.WriteLine("Connected to Server");

                Console.Write("Enter string like SetA-One: ");
                string message = Console.ReadLine();

                // Send request
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    // Read response
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("Server response:");
                        Console.WriteLine(response);
                    }
                    else
                    {
                        Console.WriteLine("No response from server.");
                    }
                }
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine("Socket error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error: " + ex.Message);
        }
        finally
        {
            Console.WriteLine("Client closed.");
        }
    }
}
