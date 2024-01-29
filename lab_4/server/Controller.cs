using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using app;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.Data.Sqlite;

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
            try
            {
                string type_str = args[0];
                string id_str = args[1];
                string type = type_str.Split('.').Last();
                int id = int.Parse(id_str);
                
                using (var context = new lab_3_database_Context())
                {
                    string res_str = "";
                    switch(type)
                    {
                        case "Car":
                            var car = context.Cars.Find(id);

                            foreach (PropertyInfo property in car.GetType().GetProperties())
                            {
                                object value = property.GetValue(car);
                                res_str += $"{property.Name}: {value}    ";
                            }
                            return new Status(res_str); 
                        
                        case "Plane":
                            var plane = context.Cars.Find(id);

                            foreach (PropertyInfo property in plane.GetType().GetProperties())
                            {
                                object value = property.GetValue(plane);
                                res_str += $"{property.Name}: {value}\t";
                            }
                            return new Status(res_str);
                    }

                    return new Status("wrong data. Could not get any value", -1);
                } 
            }
            catch (Exception e)
            {
                return new Status(e.Message, e.HResult);
            }
        }

        public Status DeleteLine(string[] args)
        {
            string type_str = args[0];
            string id_str = args[1];
            string type = type_str.Split('.').Last();

            try
            {
                using (var context = new lab_3_database_Context())
                {
                    string ent = type + 's';
                    int id = int.Parse(id_str);

                    switch(type)
                    {
                        case "Car":
                            var car = context.Cars.Find(id);
                            
                            if(car!=null)
                            {
                                context.Cars.Remove(car);
                            }
                            context.SaveChanges();
                            return new Status("success"); 
                        
                        case "Plane":
                            var plane = context.Planes.Find(id);

                            if(plane!=null)
                            {
                                context.Planes.Remove(plane);
                            }
                            context.SaveChanges();
                            return new Status("success");
                    }
                    return new Status("deleted");
                } 
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