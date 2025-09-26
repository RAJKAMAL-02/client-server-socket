using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading;

class Server {
    static Dictionary<string,Dictionary<string,int>> sets = new Dictionary<string,Dictionary<string,int>>();

    static void Main(String[] args){
        const int PORT = 8085;

        try
        {
            sets.Add("SetA",new Dictionary<string,int> {{"One",1},{"Two",2}});
            sets.Add("SetB",new Dictionary<string,int> {{"Three",3},{"Four",4}});
            sets.Add("SetC",new Dictionary<string,int> {{"Five",5},{"Six",6}});
            sets.Add("SetD",new Dictionary<string,int> {{"Seven",7},{"Eight",8}});
            sets.Add("SetE",new Dictionary<string,int> {{"Nine",9},{"Ten",10}});

            TcpListener server = new TcpListener(IPAddress.Any,PORT);
            server.Start();
            Console.WriteLine("Server started at PORT: " + PORT);

            while(true){
                try
                {
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Client Connected to Server");

                    // Thread per client
                    Thread t = new Thread(() => HandleClientRequest(client));
                    t.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error accepting client: " + ex.Message);
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
    }

    static void HandleClientRequest(TcpClient client){
        try
        {
            using (client) 
            using (NetworkStream stream = client.GetStream()) 
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer,0,buffer.Length);

                if (bytesRead > 0)
                {
                    string clientRequest = Encoding.ASCII.GetString(buffer,0,bytesRead);
                    string response  = ProcessRequest(clientRequest);

                    byte[] responseData = Encoding.ASCII.GetBytes(response);
                    stream.Write(responseData,0,responseData.Length);
                }
                else
                {
                    Console.WriteLine("Received empty request from client.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error handling client request: " + ex.Message);
        }
    }

    static string ProcessRequest(string message){
        try
        {
            string[] subString = message.Split('-');
        
            if(subString.Length !=2) return "Empty";
            string setName = subString[0].Trim();
            string key = subString[1].Trim();

            if(sets.ContainsKey(setName) && sets[setName].ContainsKey(key)){
                int n = sets[setName][key];
                StringBuilder sb = new StringBuilder();
                for(int i=0;i<n;i++){
                    sb.AppendLine(DateTime.Now.ToString());
                    Thread.Sleep(1000);
                }
                return sb.ToString();
            }
            else{
                return "Emtpy";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in ProcessRequest: " + ex.Message);
            return "Error";
        }
    }
}
