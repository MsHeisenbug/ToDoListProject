using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TaskModel> listOfTasksToDo = new List<TaskModel>();
        bool isEdited = false;
        public MainWindow()
        {
            InitializeComponent();
            dpShowTasks.SelectedDate = DateTime.Today;
            dpDateOfTask.SelectedDate = DateTime.Today;
            DateTime date = dpShowTasks.SelectedDate.Value;
            loadTasksToDo(date);
        }

        private void loadTasksToDo(DateTime dateTime)
        {
            listOfTasksToDo = null;
            listOfTasksToDo = DbDataAccess.LoadTasksFromDB(dateTime);
            lbTasksToDo.ItemsSource = listOfTasksToDo;
        }

        private void BtnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            TaskModel task = lbTasksToDo.SelectedItem as TaskModel;
            DbDataAccess.DeleteTask(task);
            loadTasksToDo(dpShowTasks.SelectedDate.Value);
        }

        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            TaskModel task;
            if (!isEdited)
            {
                task = new TaskModel();
            }
            else
            {
                task = (TaskModel)lbTasksToDo.SelectedItem;             
            }

            task.DateOfTask = dpDateOfTask.SelectedDate.Value;
            task.Description = tbTaskDesc.Text;

            DbDataAccess.SaveTask(task);
            loadTasksToDo(DateTime.Today);
            isEdited = false;
        }

        private void DpShowTasks_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            loadTasksToDo(dpShowTasks.SelectedDate.Value);
        }

        private void BtnEditTask_Click(object sender, RoutedEventArgs e)
        {
            if(lbTasksToDo.SelectedItem != null)
            {
                isEdited = true;
                dpDateOfTask.SelectedDate = ((TaskModel)lbTasksToDo.SelectedItem).DateOfTask;
                tbTaskDesc.Text = ((TaskModel)lbTasksToDo.SelectedItem).Description;
            }
        }
    }
}
