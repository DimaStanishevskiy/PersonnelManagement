using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonnelManagement.Models;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Threading;
using PersonnelManagement.Classes;


namespace PersonnelManagement.Controllers
{
    public class HomeController : Controller
    {
        private WorkerContext context = new WorkerContext();
        private const string errorView = "/Home/Error";
        private const string indexView = "/Home/index";

        [HttpGet]
        public ActionResult Index()
        {
            //eager loading of "chief"
            IEnumerable<Worker> workers = context.Workers.Include(c => c.Chief).ToList();
            ViewBag.Workers = workers;
            return View();
        }

        public ActionResult Error()
        {
            return View("../Shared/Error");
        }

        [HttpGet]
        public ActionResult CreateWorker(string firstName = "", string lastName = "", int chiefId = 0)
        {
            //Used, yoda's abstract was.
            string emptyLine = "";
            if (emptyLine.Equals(firstName) && emptyLine.Equals(lastName))
                //return "";
                return Redirect(indexView);

            Worker chief = null;
            if (chiefId != 0 & (chief = this.GetWorkerById(chiefId)) == null)
                Redirect(errorView);

            Worker worker = new Worker
            {
                FirstName = firstName,
                LastName = lastName,
                Chief = chief
            };
            context.Workers.Add(worker);
            context.SaveChanges();
            //return JsonConvert.SerializeObject(worker);
            return Redirect(indexView);
        }
        [HttpGet]
        public ActionResult DeleteWorker(int id)
        {
            Worker worker;
            if ((worker = this.GetWorkerById(id)) != null)
            {
                context.Workers.Remove(worker);
                context.SaveChanges();
            }
            return Redirect(indexView);
        }
        [HttpGet]
        public ActionResult UpdateWorker(int id, string firstName = null, string lastName = null, int chiefId = 0)
        {
            Worker worker;
            if ((worker = this.GetWorkerById(id)) == null)
                Redirect(errorView);

            if (firstName != null)
                worker.FirstName = firstName;
            if (lastName != null)
                worker.LastName = lastName;
            if (chiefId == 0)
                worker.Chief = null;
            else if ((worker.Chief = this.GetWorkerById(chiefId)) == null)
                Redirect(errorView);

            context.SaveChanges();
            //return JsonConvert.SerializeObject(worker);
            return Redirect(indexView);
        }

        [HttpPost]
        public String GetChiefs(int id)
        {
            List<Worker> chiefs = (new ChiefSearcher()).GetPossibleChiefs(id);
            foreach (Worker item in chiefs)
                item.AdaptForJson();

            return JsonConvert.SerializeObject(chiefs);
        }
        private Worker GetWorkerById(int id)
        {
            try
            {
                return context.Workers.First(t => t.Id == id);
            }
            catch (InvalidOperationException)
            {
                //No worker with this id
                return null;
            }
        }

    }
}