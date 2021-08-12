using System;
using System.Collections.Generic;
using System.Text;

namespace Dashboard1.Library
{
    class Employee
    {
        public Employee(int id, string name, string dateOfjoining, string gender, double monthlySalary)
        {
            set(id, name, dateOfjoining, gender, monthlySalary);
        }
        public int Id { set; get; }
        public string Name { set; get; }
        public string DateOfJoining { set; get; }
        public string Gender { set; get; }
        public double MonthlySalary { set; get; }
        public void set(int id, string name, string dateOfjoining, string gender, double monthlySalary)
        {
            Id = id;
            Name = name;
            DateOfJoining = dateOfjoining;
            Gender = gender;
            MonthlySalary = monthlySalary;
        }
    }
}
