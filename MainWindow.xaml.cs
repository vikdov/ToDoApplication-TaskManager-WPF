using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TASKMANAGER
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RefreshData();
        }

        private void RefreshData()
        {
            TaskOperations.LoadTasks(DataGrid);
        }

        private void BTAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTaskWindow addWindow = new AddTaskWindow(this);
            addWindow.TaskAdded += OnTaskAdded;
            addWindow.ShowDialog();
        }

        private void OnTaskAdded(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void BTDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItems = DataGrid.SelectedItems.Cast<DataGridItems>().ToList();
                if (selectedItems.Any())
                {
                    TaskOperations.DeleteSelectedTasks(selectedItems);
                    RefreshData();
                }
                else
                {
                    TaskOperations.ShowMessage("Please select at least one item to delete.");
                }
            }
            catch (Exception ex)
            {
                TaskOperations.ShowError(ex.Message);
            }
        }

        private void BTSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TaskOperations.SaveTasks(DataGrid);
                RefreshData();
            }
            catch (Exception ex)
            {
                TaskOperations.ShowError(ex.Message);
            }
        }

        private void BTClearFilter_Click(object sender, RoutedEventArgs e)
        {
            FilterPriority.SelectedIndex = 3;
            FilterIsComplete.SelectedIndex = 2;
            FilterTime.SelectedIndex = 3;
            RefreshData();
        }

        private void RefreshDataWithFilters()
        {
            if (DataGrid == null || FilterPriority == null || FilterIsComplete == null || FilterTime == null)
            {
                return;
            }

            string priority = (FilterPriority.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string status = (FilterIsComplete.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string timeFilter = (FilterTime.SelectedItem as ComboBoxItem)?.Content?.ToString();
            FilterMethods.ApplyFilters(DataGrid, priority, status, timeFilter);
        }

        private void FilterPriority_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDataWithFilters();
        }

        private void FilterIsComplete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDataWithFilters();
        }

        private void FilterTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDataWithFilters();
        }
    }
}
