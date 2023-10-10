using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using app;
using Newtonsoft.Json;
using NLog;


enum Types
{
    Plane,
    Car
}

namespace server
{
    class Server
    {
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        public static UdpClient udpClient_S;
        private int port;
        private static Controller controller = new Controller();

        Dictionary<string, Delegate> functions = new Dictionary<string, Delegate>();

        Logger log = LogManager.GetCurrentClassLogger();

        public Server(int _port)
        {
            udpClient_S = new UdpClient(_port);
            this.port = _port;
            Console.WriteLine("Сервер работает"); 

            functions["FetchFileData"] = controller.FetchFileData;
            functions["GetFileRow"] = new Func<string, Status>(controller.GetFileRow);
            functions["DeleteLines"] = new Func<string[], Status>(controller.DeleteLines);
            functions["WriteLines"] = new Func<object[], Status>(controller.WriteLines);
        }

        public void StartListenAsync()
        {
            while(true)
            {
                allDone.Reset();
                udpClient_S.BeginReceive(RequestCallback, udpClient_S);
                allDone.WaitOne();
            }
        }

        private void RequestCallback(IAsyncResult ar)
        {
            allDone.Set();
            var listener = (UdpClient)ar.AsyncState;
            var ep = (IPEndPoint)udpClient_S.Client.LocalEndPoint;
            var res = listener.EndReceive(ar, ref ep);
            string data = Encoding.Unicode.GetString(res);
            string[] data_splited = data.Split(' ');

            Console.WriteLine(data);
            log.Info(data);

            string func_str = data_splited[0];
            
            if(functions.ContainsKey(func_str))
            {
                Status outcome;
                object arg;
                if (data_splited.Length == 1)
                {
                    outcome = (Status)functions[func_str].DynamicInvoke();
                }
                else
                {
                    Type type = Type.GetType(data_splited[1]);
                    var arguments = JsonConvert.DeserializeObject(data_splited[2], type);
                    outcome = (Status)functions[func_str].DynamicInvoke(arguments);
                }
                string jsonArr = System.Text.Json.JsonSerializer.Serialize(outcome);
                Console.WriteLine(jsonArr);
                log.Info(jsonArr);
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonArr);
                udpClient_S.SendAsync(byteArray, byteArray.Length, ep);
            }
        }
    }
    
    class Program
    {
        public static UdpClient udpServer;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("entered");

            Server server = new Server(8001);
            server.StartListenAsync();
        }
    }
}