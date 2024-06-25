using ADOX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;

namespace TASKMANAGER
{
    internal class TaskOperations //crud concept (Create, Read, Update, Delete)
    {
        static string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\TaskManagerDataBase.accdb");
        static string connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databasePath};Persist Security Info=False;";


        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static void ShowMessage(string message)
        {
            MessageBox.Show(message, "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }
  
        public static void LoadTasks(DataGrid dataGrid, string query = "SELECT Id, TaskName, Priority, DueDate, IsCompleted FROM Tasks", params object[] parameters)
        {
            List<DataGridItems> todoItems = new List<DataGridItems>();

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand(query, connection);

                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue("", parameter);
                    }
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        todoItems.Add(new DataGridItems
                        {
                            Id = (int)reader["Id"],
                            TaskName = reader["TaskName"].ToString(),
                            Priority = reader["Priority"].ToString(),
                            DueDate = Convert.ToDateTime(reader["DueDate"]).ToString("yyyy-MM-dd"),
                            IsCompleted = (bool)reader["IsCompleted"]
                        });
                    }
                }
                dataGrid.ItemsSource = todoItems;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);

            }
        }

        internal static void DeleteSelectedTasks(List<DataGridItems> selectedItems)
        {
            try
            {
                if (MessageBox.Show("Are you sure?", "Notification", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        foreach (var item in selectedItems)
                        {
                            string query = "DELETE FROM Tasks WHERE Id = ?";
                            using (OleDbCommand command = new OleDbCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("Id", item.Id);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        internal static void AddTask(string ComboBoxValue, bool IsCompleteValue, string TextValue, string DateValue)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Tasks (IsCompleted, TaskName, Priority, DueDate) VALUES (?, ?, ?, ?)";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("IsCompleted", IsCompleteValue);
                        command.Parameters.AddWithValue("TaskName", TextValue);
                        command.Parameters.AddWithValue("Priority", ComboBoxValue);
                        command.Parameters.AddWithValue("DueDate", DateValue);
                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Tasks added successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }
        }

        internal static void SaveTasks(DataGrid dataGrid)
        {
            try
            {
                if (dataGrid.ItemsSource is List<DataGridItems> items)
                {
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();

                        foreach (var item in items)
                        {
                            string query = "UPDATE Tasks SET TaskName = ?, DueDate = ?, IsCompleted = ?, Priority = ? WHERE Id = ?";
                            using (OleDbCommand command = new OleDbCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("TaskName", item.TaskName);
                                command.Parameters.AddWithValue("DueDate", item.DueDate);
                                command.Parameters.AddWithValue("IsCompleted", item.IsCompleted);
                                command.Parameters.AddWithValue("Priority", item.Priority);
                                command.Parameters.AddWithValue("Id", item.Id);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                MessageBox.Show("Tasks saved successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }
    }
}
