using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LabTest.Models
{
    public class States
    {
        public List<State> StateList { get; set; }


        public List<State> GetStates()
        {
            List<State> statelist = new List<State>();
            using (SqlConnection conn = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=labtest;user id=LabTestUser;password=labtestuser1234;Trusted_Connection=True;"))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    SqlCommand query = new SqlCommand("uspStatesList", conn);
                    query.CommandType = CommandType.StoredProcedure;
                    SqlDataReader rst = query.ExecuteReader();
                    if (rst.HasRows)
                    {
                        while (rst.Read())
                        {
                            //var x = new SelectListItem { Value = rst["state_id"].ToString(), Text = rst["state_code"].ToString() };
                            statelist.Add(new State(int.Parse(rst["state_id"].ToString()),rst["state_code"].ToString()));
                        }
                    }
                }
               // statelist.Insert(0, new State(0, "Select State"));
            }
            return statelist;
        }
    }
}