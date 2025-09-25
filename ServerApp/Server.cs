using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
class Server {
    static Dictionary<string,Dictionary<string,int>> sets = new Dictionary<string,Dictionary<string,int>>();

    static void Main(String[] args){
        const int PORT = 8085;
        

        sets.Add("SetA",new Dictionary<string,int> {{"One",1},{"Two",2}});
        sets.Add("SetB",new Dictionary<string,int> {{"Three",3},{"Four",4}});
        sets.Add("SetC",new Dictionary<string,int> {{"Five",5},{"Six",6}});
        sets.Add("SetD",new Dictionary<string,int> {{"Seven",7},{"Eight",8}});
        sets.Add("SetE",new Dictionary<string,int> {{"Nine",9},{"Ten",10}});

        TcpListener server = new TcpListener(IPAddress.Any,PORT);
        server.Start();
        Console.WriteLine("Server started at PORT: " + PORT);
        while(true){
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Client Connected to Server");
             Thread t = new Thread(()=>HandleClientRequest(client));
             t.Start();
        }
      
        server.Stop();

    }

    static void HandleClientRequest(TcpClient client){
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer,0,buffer.Length);
        string clientRequest = Encoding.ASCII.GetString(buffer,0,bytesRead);
        string response  = ProcessRequest(clientRequest);

        // Console.WriteLine("Client Request: "+ clientRequest);
        // Console.WriteLine("Client Response: "+ response);
        byte[] responseData = Encoding.ASCII.GetBytes(response);
        stream.Write(responseData,0,responseData.Length);
        client.Close();
    }
    static string ProcessRequest(string message){
        string[] subString = message.Split('-');
    
        if(subString.Length !=2) return "Empty";
        string setName = subString[0].Trim();
        string key = subString[1].Trim();
        // Console.WriteLine("SetName: "+setName +" key: "+key);
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
    
}