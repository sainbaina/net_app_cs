using System.ComponentModel.DataAnnotations.Schema;

namespace App
{
    public class Smartphone
    {
        public int smartphoneId { get; set; }
        public string Name { get; set; }
        public string Processor { get; set; }

        [NotMapped]
        public Dictionary<string,string> dict{get;set;} 

        public Smartphone()
        {
            dict = new Dictionary<string, string>();
        }

        public string GetInfo()
        {
            string res = $"Name: {Name}, Cost: {Processor}\nParameters:\n";
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