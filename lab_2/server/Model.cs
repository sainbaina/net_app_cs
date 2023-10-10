namespace app
{
    public class Car
    {
        public int CarId{get;set;}
        public int prod_year{get;set;}
        public int max_speed{get;set;}
        public string name{get;set;}
        public string company{get;set;}
        public bool was_in_accident{get;set;}
    }

    public class Plane
    {
        public int PlaneId{get;set;}
        public int wings_width{get;set;}
        public int speed{get;set;}
        public string name{get;set;}
        public string prod_country{get;set;}
        public bool needs_repair{get;set;}
    }
}