using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DbDataAccess
    {
        public static List<TaskModel> LoadTasksFromDB(DateTime dateTime)
        {
            List<TaskModel> tasks = new List<TaskModel>();
            using (SQLiteConnection conn = new SQLiteConnection(loadConnectionString()))
            {
                tasks = conn.Query<TaskModel>("select * from Task where DateOfTask = '" + dateTime.ToShortDateString().Replace('.', '-') + "'").ToList();
            }
            return tasks;
        }

        public static void SaveTask(TaskModel task)
        {
            using (SQLiteConnection conn = new SQLiteConnection(loadConnectionString()))
            {
                conn.Open();
                if (conn.ExecuteScalar("select * from Task where ID = " + task.Id) != null)
                {
                    conn.Execute("update Task set DateOfTask = '" + task.DateOfTask.ToShortDateString().Replace('.', '-') + "' " +
                        ", Description = '" + task.Description + "' where ID = " + task.Id); ;
                }
                else
                {
                    string sql = "insert into Task (DateOfTask, Description) values ('" + task.DateOfTask.ToShortDateString().Replace('.', '-') + "', '" + task.Description + "')";
                    SQLiteCommand comm = new SQLiteCommand(sql, conn);
                    comm.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        public static void DeleteTask(TaskModel task)
        {
            using (SQLiteConnection conn = new SQLiteConnection(loadConnectionString()))
            {
                conn.Open();
                if (conn.ExecuteScalar("select * from Task where ID = " + task.Id) != null)
                {
                    string sql = "delete from Task where ID = " + task.Id;
                    SQLiteCommand comm = new SQLiteCommand(sql, conn);
                    comm.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        private static string loadConnectionString(string id = "default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static void CreateTableIfNotExists()
        {
            string sql = @"CREATE TABLE IF NOT EXISTS [Task] (
                          [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [DateOfTast] TEXT NOT NULL,
                          [Description] TEXT NOT  NULL
                          )";

            using (SQLiteConnection conn = new SQLiteConnection(loadConnectionString()))
            {
                conn.Open();

                SQLiteCommand comm = new SQLiteCommand(sql, conn);
                comm.ExecuteNonQuery();

                conn.Close();
            }

        }
    }
}
