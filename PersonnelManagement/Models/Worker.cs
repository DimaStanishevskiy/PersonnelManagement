using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonnelManagement.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Worker Chief { get; set; }
        public List<Worker> Workers { get; set; }

        public Worker()
        {
            Workers = new List<Worker>();
        }

        public void AdaptForJson()
        {
            Workers = null;
            if (Chief != null)
                Chief.Chief = null;
        }
    }
}