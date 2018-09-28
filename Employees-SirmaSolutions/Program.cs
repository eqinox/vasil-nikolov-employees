using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Employees_SirmaSolutions
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader fileReader = new StreamReader("employees.txt");
            string line = fileReader.ReadLine();
            List<Employee> employees = new List<Employee>();

            while (line != null)
            {
                string[] splittedLine = line.Split(',');
                int currentId = int.Parse(splittedLine[0].Trim());
                int currentProjectId = int.Parse(splittedLine[1].Trim());
                DateTime currentDateFrom = DateTime.Parse(splittedLine[2].Trim());
                DateTime currentDateTo = splittedLine[3].Trim() == "NULL" ? DateTime.Now : DateTime.Parse(splittedLine[3].Trim());

                Employee currentEmployee = new Employee(currentId, currentProjectId, currentDateFrom, currentDateTo);
                employees.Add(currentEmployee);

                line = fileReader.ReadLine();
            }
            fileReader.Close();

            List<KeyValuePair<int, int>> projectsIndexes = new List<KeyValuePair<int, int>>();
            List<PairEmployees> pairEmployees = new List<PairEmployees>();

            for (int i = 0; i < employees.Count; i++)
            {
                int projectId = employees[i].projectId;
                int employeeId = employees[i].id;

                if (projectsIndexes.Exists(x => x.Key == projectId))
                {
                    List<KeyValuePair<int, int>> allWithSameProjectId = projectsIndexes.FindAll(x => x.Key == projectId);

                    foreach (var item in allWithSameProjectId)
                    {
                        int currentProjectId = item.Key;
                        int index = item.Value;
                        PairEmployees newPair = new PairEmployees();
                        newPair.firstEmployee = employees[i].id;
                        newPair.secondEmployee = employees[index].id;
                        DateTime firstMatchDate = GetFirstMatchDate(employees[i].dateFrom, employees[index].dateFrom);
                        DateTime lastMatchDate = GetLastMatchDate(employees[i].dateTo, employees[index].dateTo);
                        if (lastMatchDate > firstMatchDate)
                        {
                            long totalTimeWorkTogether = (lastMatchDate - firstMatchDate).Ticks;
                            PairEmployees existing = pairEmployees.FirstOrDefault(x => x.firstEmployee == employees[i].id && x.secondEmployee == employees[index].id);
                            if (existing != null)
                            {
                                existing.totalTimeWorkTogether += totalTimeWorkTogether;
                            }
                            else
                            {
                                newPair.totalTimeWorkTogether += totalTimeWorkTogether;
                                pairEmployees.Add(newPair);
                            }
                        }
                    }
                }

                projectsIndexes.Add(new KeyValuePair<int, int>(projectId, i));
            }

            long maxTime = pairEmployees[0].totalTimeWorkTogether;
            int firstEmployee = 0;
            int secondEmployee = 0;

            foreach (var item in pairEmployees)
            {
                if (item.totalTimeWorkTogether >= maxTime)
                {
                    maxTime = item.totalTimeWorkTogether;
                    firstEmployee = item.firstEmployee;
                    secondEmployee = item.secondEmployee;
                }
            }

            TimeSpan time = new TimeSpan(maxTime);

            Console.WriteLine("the pair of employees who have worked longest time on common projects are {0} and {1} with {2} days", firstEmployee, secondEmployee, time.Days);
        }

        private static DateTime GetLastMatchDate(DateTime dateTo1, DateTime dateTo2)
        {
            DateTime result = new DateTime();
            if (dateTo1 > dateTo2)
            {
                while ((dateTo1.Year != dateTo2.Year) || (dateTo1.Month != dateTo2.Month) || (dateTo1.Day != dateTo2.Day))
                {
                    dateTo1 = dateTo1.AddDays(-1);
                }
                result = dateTo1;
            }
            else
            {
                while ((dateTo1.Year != dateTo2.Year) || (dateTo1.Month != dateTo2.Month) || (dateTo1.Day != dateTo2.Day))
                {
                    dateTo2 = dateTo2.AddDays(-1);
                }
                result = dateTo2;
            }

            return result;
        }

        private static DateTime GetFirstMatchDate(DateTime dateFrom1, DateTime dateFrom2)
        {
            DateTime result = new DateTime();
            if (dateFrom1 > dateFrom2)
            {
                while ((dateFrom1.Year != dateFrom2.Year) || (dateFrom1.Month != dateFrom2.Month) || (dateFrom1.Day != dateFrom2.Day))
                {
                    dateFrom2 = dateFrom2.AddDays(1);
                }
                result = dateFrom2;
            }
            else
            {
                while ((dateFrom1.Year != dateFrom2.Year) || (dateFrom1.Month != dateFrom2.Month) || (dateFrom1.Day != dateFrom2.Day))
                {
                    dateFrom1 = dateFrom1.AddDays(1);
                }
                result = dateFrom1;
            }

            return result;
        }
    }

    class PairEmployees
    {
        public int firstEmployee = 0;
        public int secondEmployee = 0;
        public long totalTimeWorkTogether = 0;
    }
}
