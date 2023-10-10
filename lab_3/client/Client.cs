using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using app;
using NLog.Config;
using NLog;


namespace client
{
    class Program
    {
        public static UdpClient udpClient;
        private static ManualResetEvent allDone = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            udpClient = new UdpClient(8002);
            Console.WriteLine("Клиент работает");

            Controller controller = new Controller();
            Status status = new Status();
            Console.Clear();
            
            ConsoleKey input;
            string function;
            string arguments;

            Logger log = LogManager.GetCurrentClassLogger();

            while (true)
            {
                string arg1;
                string arg2;
                string arg3;
                string msg;
                int j = 0;
                string type_ind_s;
                int type_ind;

                Console.WriteLine("\n\nВведите команду: \n1)вывод всех записей на экран \n2)Вывод записи по ID\n3)Удаление записи (записей) из файла\n4)Добавление записей в файл");
                input = Console.ReadKey().Key;

                try
                {
                    switch(input)
                    {
                        case ConsoleKey.D1:
                            SendMessage("FetchFileData");
                            status = ReceiveMessage();
                            break;

                        case ConsoleKey.D2:
                            Console.WriteLine("\nИндекс: ");
                            string index = Console.ReadLine();
                            msg = "GetFileRow System.String " + JsonSerializer.Serialize(index);
                            SendMessage(msg);
                            status = ReceiveMessage();
                            break;

                        case ConsoleKey.D3:
                            Console.WriteLine("\nКоличество удаляемых записей:");
                            string num_s = Console.ReadLine();
                            int num = int.Parse(num_s);
                            string[] arr = new string[num];
                            j = 0;
                            foreach (var item in (Types[]) Enum.GetValues(typeof(Types)))
                            {
                                Console.WriteLine(j.ToString() + ") " + item.ToString());
                                j++;
                            }
                            Console.WriteLine("Индекс:");
                            type_ind_s = Console.ReadLine();
                            type_ind = int.Parse(type_ind_s);
                            for (int i = 0; i < num; i++)
                            {
                                Console.WriteLine("Индекс:");
                                arr[i] += Console.ReadLine();
                            }
                            msg = "DeleteLines System.String[] " + JsonSerializer.Serialize(arr);
                            SendMessage(msg);
                            status = ReceiveMessage();
                            break;

                        case ConsoleKey.D4:
                            Console.WriteLine("\nКоличество добавляемых записей:");
                            string num1_s = Console.ReadLine();
                            int num1 = int.Parse(num1_s);

                            Console.WriteLine("Тип данных:");
                            j = 0;
                            foreach (var item in (Types[]) Enum.GetValues(typeof(Types)))
                            {
                                Console.WriteLine(j.ToString() + ") " + item.ToString());
                                j++;
                            }

                            object[] result = new object[num1];
                            object _obj = new object();

                            Console.WriteLine("Индекс:");
                            type_ind_s = Console.ReadLine();
                            type_ind = int.Parse(type_ind_s);

                            for (int i = 0; i < num1; i++)
                            {
                                switch (type_ind)
                                {
                                    case 0:
                                        _obj = new Plane();
                                        break;
                                    case 1:
                                        _obj = new Car();
                                        break;
                                }

                                foreach (var prop in _obj.GetType().GetProperties())
                                {
                                    Console.Write(prop + ": ");
                                    string userInput = Console.ReadLine();

                                    object convertedValue = Convert.ChangeType(userInput, prop.PropertyType);

                                    prop.SetValue(_obj, convertedValue);
                                }
                                result[i] = _obj;
                            }
                            string jsonArr = JsonSerializer.Serialize(result);
                            msg = "WriteLines" + ' ' + _obj.GetType().MakeArrayType().ToString() + ' ' + jsonArr;
                            SendMessage(msg);
                            status = ReceiveMessage();
                            break;

                        case ConsoleKey.Escape:
                            return;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    log.Error(e.Message);
                }

                Console.WriteLine("\n---\nstatus: {0}", status.code);
                Console.WriteLine("message: {0}", status.message);
                Console.WriteLine("data: ", status.data);

                if (status.code < 0) log.Warn(string.Format("--- status: {0} message: {1} data: {2}", status.code, status.message, status.data));
                else log.Info(string.Format("--- status: {0} message: {1} data: {2}", status.code, status.message, status.data));
                
                int count = 0;
                if (status.data is not null)
                {
                    foreach (var item in status.data)
                    {
                        Console.WriteLine("\t{0}) {1}", count, item);
                        count++;
                    }
                }
                
                Console.Write("\npress any key...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private static void SendMessage(string msg)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(msg);
                udpClient.Send(data, data.Length, "127.0.0.1", 8001);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static Status ReceiveMessage()
        {
            IPEndPoint remoteIp = (IPEndPoint)udpClient.Client.LocalEndPoint;
            Status status = new Status();
            // allDone.Set();
            try
            {
                byte[] data = udpClient.Receive(ref remoteIp);
                string decoded = Encoding.UTF8.GetString(data);
                status = JsonSerializer.Deserialize<Status>(decoded);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return status; 
        }
    }
}
