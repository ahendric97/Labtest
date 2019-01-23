using LabTest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LabTest.Controllers
{
    public class PersonController : Controller
    {
        // GET: Person
        public ActionResult Index(string Search = "")
        {
            List<Person> persons = new List<Person>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connstring"]))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    SqlCommand query = new SqlCommand("uspPersonSearch", conn);
                    query.CommandType = CommandType.StoredProcedure;
                    query.Parameters.Add(new SqlParameter("@search", Search));
                    SqlDataReader rst = query.ExecuteReader();
                    if (rst.HasRows)
                    {
                        while (rst.Read())
                        {
                            persons.Add(new Person(int.Parse(rst["person_id"].ToString()),
                                rst["first_name"].ToString(),
                                rst["last_name"].ToString(),
                                rst["state_code"].ToString(),
                                char.Parse(rst["gender"].ToString()),
                                DateTime.Parse(rst["dob"].ToString())));
                        }
                    }
                }
            }
            return View(persons);
        }



        // GET: Person/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            States st = new States();
            ViewBag.States = st.GetStates();
            return View();
        }

        // POST: Person/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connstring"]))
                    {
                        if (conn.State != ConnectionState.Open)
                        {
                            //Error Handling
                            #region Error Handling/Data Correction
                            int tstate;
                            string state = "0";
                            if (int.TryParse(collection["ddlstates"], out tstate))
                            {
                                state = ((tstate < 1) || collection["ddlstates"] == "") ? "0" : collection["ddlstates"];
                            }
                            DateTime dob;
                            if (!DateTime.TryParse(collection["DOB"].ToString(), out dob))
                                dob = DateTime.MaxValue;
                            Char Gen;
                            if (collection["Gender"] == "M" || collection["Gender"] == "F" || collection["Gender"] == "U")

                                Gen = Char.Parse(collection["Gender"]);
                            else
                                Gen = 'U';
                            #endregion

                            conn.Open();
                            SqlCommand query = new SqlCommand("uspPersonUpsert", conn);
                            query.CommandType = CommandType.StoredProcedure;
                            query.Parameters.Add(new SqlParameter("@personid", null));
                            query.Parameters.Add(new SqlParameter("@fname", collection["First_Name"]));
                            query.Parameters.Add(new SqlParameter("@lname", collection["Last_Name"]));
                            query.Parameters.Add(new SqlParameter("@state", state));
                            query.Parameters.Add(new SqlParameter("@gender", Gen));
                            query.Parameters.Add(new SqlParameter("@dob", dob));
                            query.ExecuteNonQuery();

                        }
                    }

                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return View();
            }
        }



        // GET: Person/Edit/5
        public ActionResult Edit(int id)
        {

            Person p = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connstring"]))

            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    SqlCommand query = new SqlCommand("uspLoadPerson", conn);
                    query.CommandType = CommandType.StoredProcedure;
                    query.Parameters.Add(new SqlParameter("@personid", id));
                    SqlDataReader rst = query.ExecuteReader();
                    if (rst.HasRows)
                    {
                        while (rst.Read())
                        {
                            p = new Person(int.Parse(rst["person_id"].ToString()),
                               rst["first_name"].ToString(),
                               rst["last_name"].ToString(),
                               rst["state_id"].ToString(),
                               char.Parse(rst["gender"].ToString()),
                               DateTime.Parse(rst["dob"].ToString()));
                        }
                    }
                }
            }
            States state = new States();
            ViewBag.States = new SelectList(state.GetStates(), "state_id", "state_code", p.SelectedState);




            return View(p);
        }

        // POST: Person/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            States statebag = new States();
            ViewBag.States = new SelectList(statebag.GetStates(), "state_id", "state_code");
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connstring"]))
                    {
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                            //Error Handling
                            #region Error Handling/Data Correction
                            int tstate;
                            string state = "0";
                            if (int.TryParse(collection["ddlstates"], out tstate))
                            {
                                state = ((tstate < 1) || collection["ddlstates"] == "") ? "0" : collection["ddlstates"];
                            }

                            DateTime dob;
                            if (!DateTime.TryParse(collection["DOB"].ToString(), out dob))
                                dob = DateTime.MaxValue;
                            Char Gen;
                            if (collection["Gender"] == "M" || collection["Gender"] == "F" || collection["Gender"] == "U")
                                Gen = Char.Parse(collection["Gender"]);
                            else

                                Gen = 'U';
                            #endregion


                            SqlCommand query = new SqlCommand("uspPersonUpsert", conn);
                            query.CommandType = CommandType.StoredProcedure;
                            query.Parameters.Add(new SqlParameter("@personid", id));
                            query.Parameters.Add(new SqlParameter("@fname", collection["First_Name"]));
                            query.Parameters.Add(new SqlParameter("@lname", collection["Last_Name"]));
                            query.Parameters.Add(new SqlParameter("@state", state));
                            query.Parameters.Add(new SqlParameter("@gender", Gen));
                            query.Parameters.Add(new SqlParameter("@dob", dob));
                            query.ExecuteNonQuery();

                        }
                    }

                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Person/Delete/5
        public ActionResult Delete(int id)
        {

            Person p = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connstring"]))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                    SqlCommand query = new SqlCommand("uspLoadPerson", conn);
                    query.CommandType = CommandType.StoredProcedure;
                    query.Parameters.Add(new SqlParameter("@personid", id));
                    SqlDataReader rst = query.ExecuteReader();
                    if (rst.HasRows)
                    {
                        while (rst.Read())
                        {
                            p = new Person(int.Parse(rst["person_id"].ToString()),
                               rst["first_name"].ToString(),
                               rst["last_name"].ToString(),
                               rst["state_code"].ToString(),
                               char.Parse(rst["gender"].ToString()),
                               DateTime.Parse(rst["dob"].ToString()));
                        }
                    }
                }
            }
            return View(p);
        }

        // POST: Person/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connstring"]))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                        SqlCommand query = new SqlCommand("uspDeletePerson", conn);
                        query.CommandType = CommandType.StoredProcedure;
                        query.Parameters.Add(new SqlParameter("@personid", id));
                        query.ExecuteNonQuery();

                    }
                }


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
