using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows.Controls;

namespace TASKMANAGER
{
    public static class FilterMethods
    {
        public static void ApplyFilters(DataGrid dataGrid, string priority, string status, string timeFilter)
        {
            string query = "SELECT Id, TaskName, Priority, DueDate, IsCompleted FROM Tasks WHERE 1=1";
            List<object> parameters = new List<object>();

            if (priority != "All priorities")
            {
                query += " AND Priority = ?";
                parameters.Add(priority);
            }

            if (status == "Completed")
            {
                query += " AND IsCompleted = true";
            }
            else if (status == "Not Completed")
            {
                query += " AND IsCompleted = false";
            }

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            if (timeFilter == "Today")
            {
                startDate = DateTime.Today;
                endDate = startDate.AddDays(1).AddTicks(-1);
            }
            else if (timeFilter == "Week")
            {
                startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                endDate = startDate.AddDays(7).AddTicks(-1);
            }
            else if (timeFilter == "Month")
            {
                startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                endDate = startDate.AddMonths(1).AddTicks(-1);
            }

            if (timeFilter != "All Time")
            {
                query += " AND DueDate BETWEEN ? AND ?";
                parameters.Add(startDate.ToString("yyyy-MM-dd"));
                parameters.Add(endDate.ToString("yyyy-MM-dd"));
            }
            TaskOperations.LoadTasks(dataGrid, query, parameters.ToArray());
        }

    }
}
