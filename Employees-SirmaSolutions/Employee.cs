using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees_SirmaSolutions
{
    class Employee
    {
        public int id;
        public int projectId;
        public DateTime dateFrom = new DateTime();
        public DateTime dateTo = new DateTime();

        public Employee(int id, int projectId, DateTime dateFrom, DateTime dateTo)
        {
            this.id = id;
            this.projectId = projectId;
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
        }
    }
}
