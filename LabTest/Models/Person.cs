using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LabTest.Models
{
    public class Person
    {
        public int PersonID { get; set; }
        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is Required.")]
        public string First_Name { get; set; }
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name is Required.")]
        public string Last_Name { get; set; }
        [DisplayName("State")]
        [Required(ErrorMessage = "State is Required.")]
        public string SelectedState { get; set; }
        [DisplayName("Gender")]
        [Required(ErrorMessage = "Gender is Required.")]
        public char Gender { get; set; }
        [DisplayName("Date Of Birth")]
        [Required(ErrorMessage = "Date of Birth is Required.")]
        public DateTime DOB { get; set; }


        public Person(int pID, string fname, string lname, string st, char g, DateTime dateofbirth)
        {
            PersonID = pID;
            First_Name = fname;
            Last_Name = lname;
            SelectedState = st;
            Gender = g;
            DOB = dateofbirth;
        }
    }
}