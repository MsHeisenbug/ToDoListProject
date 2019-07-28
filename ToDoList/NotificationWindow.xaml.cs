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
using System.Windows.Shapes;
using Model;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        List<TaskModel> listOfTasksToDo = new List<TaskModel>();

        public NotificationWindow()
        {
            InitializeComponent();
        }

        public void LoadTasksToDo(DateTime dateTime)
        {
            listOfTasksToDo = null;
            listOfTasksToDo = DbDataAccess.LoadTasksFromDB(dateTime);
            lbTasksToDo.ItemsSource = listOfTasksToDo;
        }
    }
}
