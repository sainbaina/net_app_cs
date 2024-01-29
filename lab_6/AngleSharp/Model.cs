namespace App
{
    class Smartphone
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public Dictionary<string,string> dict{get;set;} 

        public Smartphone()
        {
            dict = new Dictionary<string, string>();
        }

        public string GetInfo()
        {
            string res = $"Name: {Name}, Cost: {Price}\nParameters:\n";
            foreach (var item in dict)
            {
                res += item.Key;
                res += "\t";
                res += item.Value;
            }
            return res;
        }
    }
}