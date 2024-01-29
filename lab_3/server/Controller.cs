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
                
                Status status;
                using (var context = new lab_3_database_Context())
                {
                    string res_str = "";
                    switch(type)
                    {
                        case "Car":
                            var car = context.Cars.Find(id);
                            string[] res = new string[2];
                            res[0] = "Car";
                            res[1] = System.Text.Json.JsonSerializer.Serialize(car);
                            status =  new Status(res);
                            return status; 
                        
                        case "Plane":
                            var plane = context.Planes.Find(id);
                            string[] res1 = new string[2];
                            res1[0] = "Plane";
                            res1[1] = System.Text.Json.JsonSerializer.Serialize(plane);
                            status =  new Status(res1);
                            return status;
                        
                        case "RepairCompany":
                            var repairCompany = context.RepairCompanies.Find(id);
                            string[] res2 = new string[2];
                            res2[0] = "RepairCompany";
                            res2[1] = System.Text.Json.JsonSerializer.Serialize(repairCompany);
                            status =  new Status(res2);
                            return status;
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

                    Status status;
                    switch(type)
                    {
                        case "Car":
                            var car = context.Cars.Find(id);
                            string[] res = new string[2];
                            res[0] = "Car";
                            res[1] = System.Text.Json.JsonSerializer.Serialize(car);
                            status =  new Status(res);
                            context.Cars.Remove(car);
                            context.SaveChanges();
                            return status; 
                        
                        case "Plane":
                            var plane = context.Planes.Find(id);
                            string[] res1 = new string[2];
                            res1[0] = "Plane";
                            res1[1] = System.Text.Json.JsonSerializer.Serialize(plane);
                            status =  new Status(res1);
                            context.Planes.Remove(plane);
                            context.SaveChanges();
                            return status;

                        case "RepairCompany":
                            var repairCompany = context.RepairCompanies.Find(id);
                            string[] res2 = new string[2];
                            res2[0] = "RepairCompany";
                            res2[1] = System.Text.Json.JsonSerializer.Serialize(repairCompany);
                            status =  new Status(res2);
                            context.RepairCompanies.Remove(repairCompany);
                            context.SaveChanges();
                            return status;
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