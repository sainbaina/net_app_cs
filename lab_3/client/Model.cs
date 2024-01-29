namespace app
{
    public class Car
    {
        public int carId{get;set;}
        public int prod_year{get;set;}
        public int max_speed{get;set;}
        public float time0_100{get;set;}
        public string name{get;set;}
        public string company{get;set;}
        public bool was_in_accident{get;set;}
    }

    public class Plane
    {
        public int planeId{get;set;}
        public int wings_width{get;set;}
        public int speed{get;set;}
        public string name{get;set;}
        public string prod_country{get;set;}
        public bool needs_repair{get;set;}
        public int? RepairCompanyId{get;set;}
        public RepairCompany repairCompany{get;set;}
    }

    public class RepairCompany
    {
        public int RepairCompanyId{get;set;}
        public string Name{get;set;}
        public ICollection<Plane> Planes{get;set;}
        public RepairCompany()
        {
            Planes = new List<Plane>();
        }
    }
}