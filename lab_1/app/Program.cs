using System;
using System.IO;
using System.Text;


enum Types
{
    Plane,
    Car
}

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string path = "C:\\Users\\zharn\\Desktop\\subjects\\3 year\\IS Architecture\\lab_1\\text.csv";
            InputHandler handler = new InputHandler();

            do
            {
                Console.WriteLine("\nВведите команду: \n1)вывод всех записей на экран \n2)Вывод записи по номеру\n3)Удаление записи (записей) из файла\n4)Добавление записей в файл\n");
                int number = 0;
                bool res = int.TryParse(Console.ReadLine(), out number);
                if (!res) Console.WriteLine("not a number");
                else if (number<=5 && number>=1)
                {
                    int num;
                    int[] arr;
                    try
                    {
                        switch(number)
                        {
                            case 1:
                                int count = 0;
                                foreach (var item in handler.FetchFileData(path))
                                {
                                    Console.WriteLine("{0} ) {1}", count, item);
                                    count++;
                                }
                                break;

                            case 2:
                                Console.WriteLine("Индекс:");
                                int index = int.Parse(Console.ReadLine());
                                Console.WriteLine(handler.GetFileRow(index, path));
                                break;

                            case 3:
                                Console.WriteLine("Количество удаляемых записей:");
                                num = int.Parse(Console.ReadLine());
                                arr = new int[num];
                                for (int i = 0; i < num; i++)
                                {
                                    Console.WriteLine("Индекс:");
                                    arr[i] = int.Parse(Console.ReadLine());
                                }
                                handler.DeleteLines(arr, path);
                                break;

                            case 4:
                                Console.WriteLine("Количество добавляемых записей:");
                                num = int.Parse(Console.ReadLine());

                                Console.WriteLine("Тип данных:");
                                int j = 0;
                                foreach (var item in (Types[]) Enum.GetValues(typeof(Types)))
                                {
                                    Console.WriteLine(j.ToString() + ") " + item.ToString());
                                    j++;
                                }

                                object[] result = new object[num];
                                object _obj = new object();
                                for (int i = 0; i < num; i++)
                                {
                                    Console.WriteLine("Индекс:");
                                    int type_ind = int.Parse(Console.ReadLine());

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

                                handler.WriteLines(result, path);
                                break;
                        }
                    }
                    catch (IOException e)
                    {
                        TextWriter errorWriter = Console.Error;
                        errorWriter.WriteLine(e.Message);
                    }
                }
                else Console.WriteLine("wrong number");
            } 
            while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
    }
}
