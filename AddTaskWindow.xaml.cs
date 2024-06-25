using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TASKMANAGER
{
    /// <summary>
    /// Logika interakcji dla klasy AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        public event EventHandler TaskAdded;
        public MainWindow mainWindow;
        public AddTaskWindow()
        {
            InitializeComponent();
        }

        public AddTaskWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }
        private const string textinfield = "Enter your task here";
        private void CancelBT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PlaceholderTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text == textinfield)
            {
                textBox.Text = string.Empty;
                textBox.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Black);
            }
        }

        private void PlaceholderTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textinfield;
                textBox.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Gray);
            }
        }

        private void AddTaskBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var taskName = NewTB.Text.Trim();
                var priority = (NewComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
                var dueDate = NewDatePicker.SelectedDate;
                var isCompleted = NewIsComplete.IsChecked ?? false;

                if (taskName == textinfield)
                {
                    TaskOperations.ShowMessage("Please type something in a task field .");
                    return;
                }

                if (priority == null)
                {
                    TaskOperations.ShowMessage("Please select a priority.");
                    return;
                }

                if (!dueDate.HasValue)
                {
                    TaskOperations.ShowMessage("Please select a due date.");
                    return;
                }

                TaskOperations.AddTask(priority, isCompleted, taskName, dueDate.Value.ToString("yyyy-MM-dd"));
                TaskAdded?.Invoke(this, EventArgs.Empty);
                Close();
            }
            catch (Exception ex)
            {
                TaskOperations.ShowError(ex.Message);
            }
        }
    }
}