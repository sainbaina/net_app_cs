using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using app;
using System.Text.Json.Nodes;

namespace app
{
    class Status
    {
        public int code {get;set;}
        public string message {get;set;}
        public string[] data {get;set;}

        public Status() {}

        public Status(List<string> result)
        {
            code = 1;
            message = "success";
            data = result.ToArray();
        }

        public Status(string[] result)
        {
            code = 1;
            message = "success";
            data = result;
        }

        public Status(string result)
        {
            code = 1;
            message = "success";
            data = new string[1];
            data[0] = result;
        }

        public Status(string error, int err_code)
        {
            code = err_code;
            message = error;
        }
    }

    class Controller
    {
        string path = Directory.GetCurrentDirectory() + "\\text.csv";

        private readonly lab_3_database_Context _context;

        public Status FetchFileData()
        {
            try
            {
                using (var context = new lab_3_database_Context())
                {
                    var allRecords = new List<object>();

                    allRecords.AddRange(context.Cars.ToList());
                    allRecords.AddRange(context.Planes.ToList());

                    List<string> res = new List<string>();
                    foreach (var item in allRecords)
                    {
                        res.Add(System.Text.Json.JsonSerializer.Serialize(item));
                    }

                    return new Status(res);
                } 
            }
            catch (Exception e)
            {
                return new Status(e.Message, e.HResult);
            }
        }

        public Status GetFileRow(string[] args)
        {
            string type_str = args[0];
            string id_str = args[1];
            try
            {
                using (var context = new lab_3_database_Context())
                {
                    int id = int.Parse(id_str);
                    context.

                    allRecords.AddRange(context.Cars.ToList());
                    allRecords.AddRange(context.Planes.ToList());

                    return new Status(System.Text.Json.JsonSerializer.Serialize(allRecords));
                } 

                // int id = int.Parse(id_str);
                List<string> lines = new List<string>();

                using (var sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                } 
                
                return new Status(lines[index]);
            }
            catch (Exception e)
            {
                return new Status(e.Message, e.HResult);
            }
        }

        public Status DeleteLines(string[] indexes_str)
        {
            try
            {
                int[] indexes = new int[indexes_str.Length];
                for (int i = 0; i < indexes_str.Length; i++)
                {
                    indexes[i] = int.Parse(indexes_str[i]);
                }

                List<string> lines = File.ReadAllLines(path).ToList();
                int len = lines.Count;
                int delta = 0;

                foreach (var index in indexes)
                {
                    int i = index;
                    i -= delta;
                    if (i >= len) return new Status("CUSTOM_ERR: wrong indexes", -1);
                    lines.RemoveAt(i);
                    len--;
                    delta++;
                }
                
                File.WriteAllLines(path, lines);
                return new Status("success");
            }
            catch (Exception e)
            {
                return new Status(e.Message, e.HResult);
            }
        }

        public Status WriteLines(object[] objects)
        {
            try
            {
                using (var context = new lab_3_database_Context())
                {
                    foreach (var item in objects)
                    {
                        context.Add(item);
                    }
                    context.SaveChanges();
                }

                return new Status("success");
            }
            catch (Exception e)
            {
                return new Status(e.Message, e.HResult);
            }
        }
    }
}