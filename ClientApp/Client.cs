using System;
using System.Net.Sockets;
using System.Text;

class Client{
    const string localHost = "127.0.0.1";
    const int PORT = 8085;
    static void Main(String[] args){
        TcpClient client = new TcpClient(localHost,PORT);
        Console.WriteLine("connected to Server");
        Console.Write("Enter string like SetA-One: ");
        
        string message = Console.ReadLine();
        NetworkStream stream = client.GetStream();
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data,0,data.Length);

        byte[] buffer = new byte[1024];
        stream.Read(buffer,0,buffer.Length);
        string response = Encoding.ASCII.GetString(buffer,0,buffer.Length);
        Console.WriteLine(response);
        client.Close();
        
    }
    
}