using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTry.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().
                HasData(
                    new Employee
                    {
                        Id = 1,
                        Name = "Mark",
                        Department = Dept.IT,
                        Email = "mark@gmail.com"
                    }
                );
        }
    }


}
