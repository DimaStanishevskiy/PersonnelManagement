using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;
using PersonnelManagement.Models;
using System.Threading.Tasks;

namespace PersonnelManagement.Classes
{
    public class ChiefSearcher
    {
        private WorkerContext context = new WorkerContext();
        private List<Worker> result;
        public List<Worker> GetPossibleChiefs(int id)
        {
            Worker worker;
            try
            {
                worker = context.Workers.First(t => t.Id == id);
            }
            catch (InvalidOperationException)
            {
                //No worker with this id
                return null;
            }

            result = context.Workers.ToList();
            Remove(worker);
            //foreach(Worker item in context.Workers.ToList().FindAll())
            DropSubordinates(worker.Workers);
            return result;

        }
        private void DropSubordinates(List<Worker> workers)
        {
            foreach (Worker item in workers)
            {
                if (item.Workers.Count > 0)
                {
                    DropSubordinates(item.Workers);
                }
                    Remove(item);
            }
        }

        private void Remove(Worker worker)
        {
            result.Remove(result.Find(n => n.Id == worker.Id));
        }
    }
}