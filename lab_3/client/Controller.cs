enum Types
{
    Plane,
    Car
}

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


        public Status FetchFileData()
        {
            try
            {
                List<string> res = new List<string>();
                using (var sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        res.Add(line);
                    }
                    return new Status(res);
                } 
            }
            catch (Exception e)
            {
                return new Status(e.Message, e.HResult);
            }
        }

        public Status GetFileRow(string index_str)
        {
            try
            {
                int index = int.Parse(index_str);
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
                string[] lines = File.ReadAllLines(path);
                using (var sw = new StreamWriter(path))
                {
                    foreach (var obj in objects)
                    {
                        sw.Write(obj.GetType());
                        sw.Write(',');
                        foreach (var prop in obj.GetType().GetProperties())
                        {
                            sw.Write(prop.GetValue(obj, null));
                            sw.Write(',');
                        }  
                        sw.Write('\n');
                    }

                    foreach (var line in lines)
                    {
                        sw.WriteLine(line);
                    }
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