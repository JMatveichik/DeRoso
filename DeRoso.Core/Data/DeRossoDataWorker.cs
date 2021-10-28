using DeRoso.Core.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Data
{
    public static class DeRossoDataWorker
    {
        public static List<Patient> GetAllPatients()
        {
            using (DeRosoContext db = new DeRosoContext())
            {
                var results = db.Patients.ToList();
                return results;
            }
        }
        
    }
}
