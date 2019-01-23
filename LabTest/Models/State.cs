using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabTest.Models
{
    public class State
    {
        public int State_ID { get; set; }
        public string State_Code { get; set; }

        public State(int id, string code)
        {
            State_ID = id;
            State_Code = code;
        }
    }
}