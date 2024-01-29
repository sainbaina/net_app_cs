using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace app
{
    public class lab_3_database_Context : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Plane> Planes { get; set; }
        public DbSet<RepairCompany> RepairCompanies { get; set; }

        public string DbPath{get;}

        public lab_3_database_Context()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "lab_3_database.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    }
}