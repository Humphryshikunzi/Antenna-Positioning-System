using System;
using System.Net;  
using System.Net.Sockets;
using System.Text;

namespace Chat_App
{
    class Program
    {
        static string serverIP = "localhost"; //"defrilab.com";  
        static int port = 5070; 
        
        static void Main(string[] args)
        {
            Console.WriteLine("C# Client for TCP");
            TcpClient tcpClient = new TcpClient();    
            tcpClient.Connect("127.0.0.1", 5070);                     
            NetworkStream stream = tcpClient.GetStream();
            

            while(true)
            {
                Console.WriteLine("Type what to say");
                var message = Console.ReadLine();   
                byte[] data =  Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length); 
                Console.WriteLine("Sent '{0}' to server", message);
            }                             
            stream.Close();  
            tcpClient.Close();  
        }
    }
}
