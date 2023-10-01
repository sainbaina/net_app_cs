namespace app
{
    class InputHandler
    {
        public List<string> FetchFileData(string path)
        {
            List<string> res = new List<string>();
            using (var sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    res.Add(line);
                }
                return res;
            } 
        }

        public string GetFileRow(int index, string path)
        {
            List<string> lines = new List<string>();

            using (var sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            } 

            if (index >= lines.Count) return "";
            return lines[index];
        }

        public void DeleteLines(int[] indexes, string path)
        {
            List<string> lines = File.ReadAllLines(path).ToList();
            int len = lines.Count;
            int delta = 0;

            foreach (var index in indexes)
            {
                int i = index;
                i -= delta;
                if (i >= len) return;
                lines.RemoveAt(i);
                len--;
                delta++;
            }
            
            File.WriteAllLines(path, lines);
        }

        public void WriteLines(object[] objects, string path)
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
        }
    }
}