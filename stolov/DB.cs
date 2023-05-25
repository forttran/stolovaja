using NLog.Web;
using stolov.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Filters;
using NLog.Fluent;
using System.Windows.Forms;

namespace stolov {
	internal class DB {
		private static DB instance;
		public static SqlConnection con;
		public static Logger currentClassLogger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
		private DB(String ConnectionString) {
			try {
				con = new SqlConnection();
				if (ConnectionString == null)
					ConnectionString = Settings.Default.ConnectionString;
				con.ConnectionString = ConnectionString;
				con.Open();
			} catch(Exception e) {
				MessageBox.Show("Проблемы с подключением к серверу. Возможно некорректно заданы настройки сервера или отсутствует подключение к интернету");
				currentClassLogger.Error("Ошибка подключения к серверу: " + ConnectionString);
				currentClassLogger.Error("Ошибка: " + e.Message + "; Source: " + e.Source);
				Application.Exit();
			}
		}
		public static DB getInstance(String ConnectionString = null) {
			if(instance == null)
				instance = new DB(ConnectionString);
			return instance;
		}
		public static DataTable Query(String query) {
			try {
				SqlCommand cmd = new SqlCommand(query, con);
				SqlDataReader reader = cmd.ExecuteReader();
				DataTable dataTable = new DataTable();
				dataTable.Load(reader);
				return dataTable;
			}catch(Exception e) {
				MessageBox.Show("Проблемы с SQL запросом.");
				currentClassLogger.Error("DB.Query.Проблемы с SQL запросом.");
				currentClassLogger.Error("Ошибка: " + e.Message + "; Source: " + e.Source);
				return new DataTable();
			}
		}
	}
}
